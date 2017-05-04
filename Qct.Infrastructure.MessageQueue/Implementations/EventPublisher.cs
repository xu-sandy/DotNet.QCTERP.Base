using Qct.Infrastructure.MessageClient.ObjectModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageClient.Implementations
{
    /// <summary>
    /// 事件发布器
    /// </summary>
    public class EventPublisher : IPublisher
    {
        public EventPublisher(Guid id)
        {
            PublisherId = id;
        }
        /// <summary>
        /// 本地订阅项目
        /// </summary>
        internal static List<LocalSubscribeItem> subscribes = new List<LocalSubscribeItem>();
        /// <summary>
        /// 远程订阅代理
        /// </summary>
        IRemoteServiceAgent RemoteServiceAgent { get; set; }

        /// <summary>
        /// 发布器编号（初次初始化后，需要做持久化保存，保证程序再次启动不发生变化）
        /// </summary>
        public Guid PublisherId { get; private set; }
        /// <summary>
        /// 初始化事件发布器，并执行订阅区块，自动加载SubscribeAttribute标识的订阅内容
        /// </summary>
        /// <param name="areas">多个订阅区块</param>
        public void Initialization(bool isWebSite = false, params ISubscribeArea[] areas)
        {
            if (areas != null && areas.Length > 0)
            {
                foreach (var item in areas)
                {
                    item.RegisterArea(this);
                }
            }
            Task.Factory.StartNew(() =>
            {
                MessageQueueConfigurationSection config = MessageQueueConfigurationSection.GetConfig();
                if (config.EnaleExchange)
                {
                    var exchange = new Exchanger(PublisherId, isWebSite, RemoteSubscribe);
                    RemoteServiceAgent = exchange;
                    //Subscribe(exchange, "*", FilterMode.FullLocalEvent);
                }
            });
        }

        private void RemoteSubscribe(object sender, EventArgs e)
        {
            lock (subscribes)
            {
                foreach (var item in subscribes)
                {
                    RemoteServiceAgent = sender as IRemoteServiceAgent;
                    RemoteServiceAgent?.Subscribe(new SubscribeItem() { Descriptions = item.Descriptions, FilterMode = item.FilterMode, PublisherId = PublisherId });
                }
            }
        }

        #region 事件推送(Publish)

        /// <summary>
        /// 事件推送
        /// </summary>
        /// <typeparam name="TEventContent">事件内容</typeparam>
        /// <param name="description">事件描述</param>
        /// <param name="domainEventContent">领域事件内容</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        public void Publish<TEventContent>(string description, TEventContent domainEventContent, bool isLocalLoop = false)
        {
            var task = PublishAsync(description, domainEventContent, isLocalLoop);
            task.Wait();
        }

        /// <summary>
        /// 事件推送
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="domainEvent">领域事件</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        public void Publish<TEvent>(TEvent domainEvent, bool isLocalLoop = false) where TEvent : IEvent, new()
        {
            var task = PublishAsync(domainEvent, isLocalLoop);
            task.Wait();
        }

        /// <summary>
        /// 异步事件推送
        /// </summary>
        /// <typeparam name="TEventContent">事件内容</typeparam>
        /// <param name="description">事件描述</param>
        /// <param name="domainEventContent">领域事件内容</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        /// <returns>异步任务</returns>
        public Task PublishAsync<TEventContent>(string description, TEventContent domainEventContent, bool isLocalLoop = false)
        {
            var domainEvent = new ObjectsEvent<TEventContent>() { Content = domainEventContent, Descriptions = description };
            return PublishAsync(domainEvent, isLocalLoop);
        }

        /// <summary>
        /// 异步事件推送
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="domainEvent">领域事件</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        /// <returns>异步任务</returns>
        public Task PublishAsync<TEvent>(TEvent domainEvent, bool isLocalLoop = false) where TEvent : IEvent, new()
        {
            return Task.Factory.StartNew(() =>
            {

                if (domainEvent.Equals(default(TEvent)))
                {
                    throw new DomianEventException("事件不能为null!");
                }

                if (domainEvent.PublisherId == Guid.Empty || domainEvent.PublisherId == default(Guid))
                {
                    domainEvent.PublisherId = PublisherId;
                }

                IEnumerable<LocalSubscribeItem> effectiveSubscribes;
                lock (subscribes)//防止多线程并发
                {
                    effectiveSubscribes = subscribes.Where(o => o.IsActive(domainEvent, isLocalLoop, PublisherId)).ToList();
                }
                foreach (var item in effectiveSubscribes)
                {
                    try
                    {
                        if (domainEvent is EventJsonWrapper)
                        {
                            var wrapper = domainEvent as EventJsonWrapper;
                            var _event = item.Handler.JsonToEvent(wrapper.EventObject);
                            ExcuteHandler(item.Handler, _event);
                        }
                        else
                        {
                            ExcuteHandler(item.Handler, domainEvent);
                        }

                    }
                    catch (Exception)
                    {

                    }
                }
                if (!isLocalLoop && domainEvent.PublisherId == PublisherId && RemoteServiceAgent != null)//处理转发
                {
                    Task.Factory.StartNew(() =>
                    {
                        var wrapper = new EventJsonWrapper(domainEvent);
                        wrapper.IsLocalLoop = isLocalLoop;
                        ExcuteHandler(RemoteServiceAgent, wrapper);
                    });
                }
            });
        }
        /// <summary>
        /// 执行订阅回调
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="handler"></param>
        /// <param name="_event"></param>
        private void ExcuteHandler<TEvent>(IEventHandler handler, TEvent _event) where TEvent : IEvent, new()
        {
            if (handler == null || _event == null)
            {
                return;
            }
            var pHandler = handler as IEventHandler<TEvent>;
            pHandler.Handler(_event);
        }
        #endregion 事件推送(Publish)

        #region 事件订阅(Subscribe)

        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        public Guid Subscribe<TEvent>(BaseEventHandler<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new()
        {
            var task = SubscribeAsync(handler, descriptions, mode);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        public Guid Subscribe<TEvent>(Action<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new()
        {
            var task = SubscribeAsync(handler, descriptions, mode);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 异步事件订阅
        /// </summary>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        /// <returns>异步任务</returns>
        public Task<Guid> SubscribeAsync<TEvent>(BaseEventHandler<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new()
        {
            return Task.Factory.StartNew(() =>
            {
                if (handler == null)
                {
                    throw new DomianEventException("事件处理回调不能为空！");
                }
                if (descriptions == null || descriptions.Length == 0)
                {
                    throw new DomianEventException("订阅事件至少需要一条描述！");
                }
                //if (mode == FilterMode.FullLocalEvent && !(handler is BaseEventHandler<EventJsonWrapper>))
                //{
                //    throw new DomianEventException("订阅本地全部事件时，请实现BaseEventHandler<EventJsonWrapper>！");
                //}
                var id = Guid.NewGuid();
                lock (subscribes)
                {
                    subscribes.Add(new LocalSubscribeItem()
                    {
                        Handler = handler,
                        Descriptions = descriptions,
                        FilterMode = mode,
                        Id = id
                    });
                }
                //  if (FilterMode.FullLocalEvent != mode)
                RemoteServiceAgent?.Subscribe(new SubscribeItem() { Descriptions = descriptions, FilterMode = mode, PublisherId = PublisherId });
                return id;
            });
        }

        /// <summary>
        /// 异步事件订阅
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        /// <returns>异步任务</returns>
        public Task<Guid> SubscribeAsync<TEvent>(Action<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new()
        {
            var wrapper = new EventHandlerActionWrapper<TEvent>(handler);
            return SubscribeAsync(wrapper, descriptions, mode);
        }

        #endregion 事件订阅(Subscribe)

        #region 取消事件订阅(UnSubscribe)
        /// <summary>
        /// 取消事件订阅
        /// </summary>
        /// <param name="subscribeItemId">订阅id</param>
        public void UnSubscribe(Guid subscribeItemId)
        {
            lock (subscribes)
            {
                var item = subscribes.FirstOrDefault(o => o.Id == subscribeItemId);
                if (item != null)
                {
                    item.IsUnSubscribe = true;
                    subscribes.Remove(item);
                }
            }
        }

        /// <summary>
        /// 异步取消事件订阅
        /// </summary>
        /// <param name="subscribeItemId">订阅id</param>
        /// <returns>异步任务</returns>
        public Task UnSubscribeAsync(Guid subscribeItemId)
        {
            return Task.Factory.StartNew(() =>
            {
                UnSubscribe(subscribeItemId);
            });
        }

        /// <summary>
        /// 取消所有事件订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            lock (subscribes)
            {
                foreach (var item in subscribes)
                {
                    item.IsUnSubscribe = true;
                }
            }
            subscribes = new List<LocalSubscribeItem>();
        }

        /// <summary>
        /// 异步取消所有事件订阅
        /// </summary>
        /// <returns></returns>
        public Task UnsubscribeAllAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                UnsubscribeAll();
            });
        }
        #endregion 取消事件订阅(UnSubscribe)


    }
}
