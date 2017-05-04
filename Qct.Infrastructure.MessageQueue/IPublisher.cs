using Qct.Infrastructure.MessageClient.ObjectModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageClient
{

    /// <summary>
    /// 事件发布器
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// 发布器编号（初次初始化后，需要做持久化保存，保证程序再次启动不发生变化）
        /// </summary>
        Guid PublisherId { get; }

        /// <summary>
        /// 执行订阅区块
        /// </summary>
        /// <param name="areas">多个订阅区块</param>
        void Initialization(bool isWebSite = false, params ISubscribeArea[] areas);

        #region 事件推送(Publish)

        /// <summary>
        /// 事件推送
        /// </summary>
        /// <typeparam name="TEventContent">事件内容</typeparam>
        /// <param name="description">事件描述</param>
        /// <param name="domainEventContent">领域事件内容</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        void Publish<TEventContent>(string description, TEventContent domainEventContent, bool isLocalLoop = false);

        /// <summary>
        /// 事件推送
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="domainEvent">领域事件</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        void Publish<TEvent>(TEvent domainEvent, bool isLoop = false) where TEvent : IEvent, new();

        /// <summary>
        /// 异步事件推送
        /// </summary>
        /// <typeparam name="TEventContent">事件内容</typeparam>
        /// <param name="description">事件描述</param>
        /// <param name="domainEventContent">领域事件内容</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        /// <returns>异步任务</returns>
        Task PublishAsync<TEventContent>(string description, TEventContent domainEventContent, bool isLocalLoop = false);

        /// <summary>
        /// 异步事件推送
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="domainEvent">领域事件</param>
        /// <param name="isLocalLoop">是否为本地环内事件</param>
        /// <returns>异步任务</returns>
        Task PublishAsync<TEvent>(TEvent domainEvent, bool isLocalLoop = false) where TEvent : IEvent, new();

        #endregion 事件推送(Publish)

        #region 事件订阅(Subscribe)

        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        /// <returns>订阅编号</returns>
        Guid Subscribe<TEvent>(BaseEventHandler<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new();

        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        /// <returns>订阅编号</returns>
        Guid Subscribe<TEvent>(Action<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new();

        /// <summary>
        /// 异步事件订阅
        /// </summary>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        /// <returns>返回订阅编号的异步任务</returns>
        Task<Guid> SubscribeAsync<TEvent>(BaseEventHandler<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new();

        /// <summary>
        /// 异步事件订阅
        /// </summary>
        /// <typeparam name="TEvent">实现IEvent的事件</typeparam>
        /// <param name="handler">处理回调</param>
        /// <param name="mode">匹配过滤模式</param>
        /// <param name="descriptions">匹配事件描述</param>
        /// <returns>返回订阅编号的异步任务</returns>
        Task<Guid> SubscribeAsync<TEvent>(Action<TEvent> handler, string descriptions, FilterMode mode = FilterMode.StartsWith) where TEvent : IEvent, new();

        #endregion 事件订阅(Subscribe)

        #region 取消事件订阅(UnSubscribe)
        /// <summary>
        /// 取消事件订阅
        /// </summary>
        /// <param name="subscribeItemId">订阅id</param>
        void UnSubscribe(Guid subscribeItemId);

        /// <summary>
        /// 异步取消事件订阅
        /// </summary>
        /// <param name="subscribeItemId">订阅id</param>
        /// <returns>异步任务</returns>
        Task UnSubscribeAsync(Guid subscribeItemId);

        /// <summary>
        /// 取消所有事件订阅
        /// </summary>
        void UnsubscribeAll();

        /// <summary>
        /// 异步取消所有事件订阅
        /// </summary>
        /// <returns></returns>
        Task UnsubscribeAllAsync();
        #endregion 取消事件订阅(UnSubscribe)
    }
}
