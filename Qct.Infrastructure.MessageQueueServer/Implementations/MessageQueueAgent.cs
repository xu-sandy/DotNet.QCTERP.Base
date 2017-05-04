using log4net;
using Qct.Infrastructure.MessageClient.ObjectModels;
using Qct.Infrastructure.MessageServer.Exceptions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Qct.Infrastructure.MessageServer.Extensions;

namespace Qct.Infrastructure.MessageServer.Implementations
{

    /// <summary>
    /// 消息队列代理器
    /// </summary>
    public class MessageQueueAgent : IMessageQueueAgent
    {
        /// <summary>
        /// 消息队列代理器ID记录文件名
        /// </summary>
        private static readonly string AGENTID = "MessageQueueAgentId.greenery";


        /// <summary>
        /// 初始化消息队列代理器
        /// </summary>
        /// <param name="server"></param>
        public MessageQueueAgent(MQMServer server)
        {
            Server = server;
            Logger = LogManager.GetLogger(GetType());
            Adapter = new RedisMessageQueueAdapter();
            Storage = new DefaultMesssageQueueMiddlewareStorage();

            var subscribeItems = Storage.GetSubscribeItems(AgentId);//重载订阅数据，并进行远程订阅
            Storage.RemoveRemoteSubscribe(AgentId, null);
            foreach (var item in subscribeItems)
            {
                var group = item.Descriptions.GetGroup();
                if (!string.IsNullOrEmpty(group))
                {
                    RemoteSubscribe(group);//处理远程订阅
                }
            }

            RetryRemotePushFailure();
        }

        ILog Logger { get; set; }
        /// <summary>
        /// 数据存储提供程序
        /// </summary>
        IMQMStorage Storage { get; set; }

        /// <summary>
        /// 消息中间件服务器对象
        /// </summary>
        MQMServer Server { get; set; }
        /// <summary>
        /// 消息队列适配器对象
        /// </summary>
        IMessageQueueAdapter Adapter { get; set; }

        Guid agentId = Guid.Empty;
        /// <summary>
        /// 消息队列代理器Id
        /// </summary>
        public Guid AgentId
        {
            get
            {
                if (agentId == Guid.Empty)
                {
                    InitAgentId();
                }
                return agentId;
            }
        }

        /// <summary>
        /// 初始化消息队列代理器Id,如果文件存在则重新加载历史Id,否则新建并保存Id
        /// </summary>
        private void InitAgentId()
        {
            var fileFullName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AGENTID);
            lock (AGENTID)
            {
                if (File.Exists(fileFullName))
                {
                    var agentIdIdBytes = File.ReadAllBytes(fileFullName);
                    agentId = new Guid(agentIdIdBytes);
                }
                else
                {
                    agentId = Guid.NewGuid();
                    File.WriteAllBytes(fileFullName, agentId.ToByteArray());
                }
            }
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="subscriber">订阅对象信息</param>
        /// <param name="key">密钥</param>
        /// <returns>是否成功</returns>
        public bool Authentication(ClientIndetity subscriber, string key)
        {
            if (key.Trim() != MQMConfigurationSection.GetConfig().Password.Trim())
            {
                throw new NotAuthenticationException("授权验证失败，请提供正确的授权码！");
            }
            ClientIndetity old;
            if (Storage.TryGetClientIndetity(subscriber.PublisherId, out old))
            {
                old.SessionId = subscriber.SessionId;
                old.IsWebSite = subscriber.IsWebSite;
                old.WebSiteHost = subscriber.WebSiteHost;
                old.AgentId = AgentId;
                Storage.SaveUpdateClientIndetity(old);
            }
            else
            {
                subscriber.AgentId = AgentId;
                Storage.AddClientIndetity(subscriber);
            }
            return true;
        }
        /// <summary>
        /// 代理远程订阅
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="subscribeItem">订阅信息</param>
        public void Subscribe(Guid publisherId, SubscribeItem subscribeItem)
        {
            if (subscribeItem == null || subscribeItem.Descriptions.Length < 1)
            {
                throw new SubscribeItemNotFoundException("订阅信息不全");
            }
            var group = subscribeItem.Descriptions.GetGroup();
            if (string.IsNullOrEmpty(group))
            {
                throw new SubscribeItemNotFoundException("订阅信息不全");
            }
            ClientIndetity subscriber;
            if (Storage.TryGetClientIndetity(publisherId, out subscriber))
            {
                SubscribeItem old;
                if (!Storage.TryGetSubscribeItem(publisherId, subscribeItem.Descriptions, subscribeItem.FilterMode, out old))
                {
                    Storage.AddSubscribeItem(subscribeItem);
                }
                RemoteSubscribe(group);

                Task.Factory.StartNew(() =>
                {
                    RetryRemoteSubscriptionCallbackFailure(subscriber);
                });
            }
            else
            {
                throw new NotAuthenticationException();
            }
        }

        private void RemoteSubscribe(string group)
        {
            if (!Storage.HasRemoteSubscribe(AgentId, group))//处理远程订阅
            {
                Adapter.Subscribe(group, RemotePublishCallBack);
                Storage.AddRemoteSubscribe(AgentId, group);
            }
        }
        /// <summary>
        /// 取消远程代理订阅
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="subscribeItem">订阅信息</param>
        public void UnSubscribe(Guid publisherId, SubscribeItem subscribeItem)
        {
            if (subscribeItem == null || subscribeItem.Descriptions.Length < 1)
            {
                throw new SubscribeItemNotFoundException("订阅信息不全");
            }
            var group = subscribeItem.Descriptions.GetGroup();
            if (string.IsNullOrEmpty(group))
            {
                throw new SubscribeItemNotFoundException("订阅信息不全");
            }
            ClientIndetity subscriber;
            if (Storage.TryGetClientIndetity(publisherId, out subscriber))
            {
                SubscribeItem old;
                if (Storage.TryGetSubscribeItem(publisherId, subscribeItem.Descriptions, subscribeItem.FilterMode, out old))
                {
                    Storage.RemoveSubscribeItem(publisherId, old);
                    if (!Storage.HasSubscribers(AgentId, group))
                    {
                        Storage.RemoveRemoteSubscribe(AgentId, group);
                    }
                }
                else
                {
                    throw new SubscribeItemNotFoundException("取消事件远程代理订阅失败，未能找到订阅项目！");
                }
            }
            else
            {
                throw new NotAuthenticationException();
            }
        }
        /// <summary>
        /// 事件代理远程推送
        /// </summary>
        /// <param name="publishItem">推送信息</param>
        public void Publish(PublishItem publishItem)
        {
            try
            {
                Adapter.Publish(publishItem.Descriptions, publishItem.Content);
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew((e) =>
                {
                    var exception = e as Exception;
                    if (exception != null && Logger != null)
                    {
                        Logger.Error("事件代理远程推送失败！", exception);
                    }
                }, ex);
                Storage.AddRemotePushFailureRecord(AgentId, publishItem);
            }
        }
        private void RetryRemotePushFailure()
        {
            var records = Storage.GetRemotePushFailureRecords(AgentId);
            foreach (var item in records)
            {
                try
                {
                    Adapter.Publish(item.Content.Descriptions, item.Content.Content);
                    Storage.RemoveRemotePushFailureRecord(item._id);
                }
                catch { }
            }
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(MQMConfigurationSection.GetConfig().RetryRemotePushFailureInterval);
                RetryRemotePushFailure();
            });
        }
        /// <summary>
        /// 远程订阅推送回调
        /// </summary>
        /// <param name="descriptions">事件描述</param>
        /// <param name="content">事件内容</param>
        public void RemotePublishCallBack(string descriptions, string content)
        {
            try
            {
                var channel = new SendToClientChannel(Server, Logger);
                var receivers = Storage.FindSubscribers(AgentId, descriptions);
                foreach (var item in receivers)
                {
                    if (!channel.SendMesssage(item, content))
                    {
                        Storage.AddRemoteSubscriptionCallbackFailureRecord(item.PublisherId, content);
                    }
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew((e) =>
                {
                    var exception = e as Exception;
                    if (exception != null && Logger != null)
                    {
                        Logger.Error("远程订阅推送回调时失败！", exception);
                    }
                }, ex);
            }
        }
        /// <summary>
        /// 重试远程订阅失败回调
        /// </summary>
        /// <param name="subscriber">订阅客户端凭证</param>

        public void RetryRemoteSubscriptionCallbackFailure(ClientIndetity subscriber)
        {
            try
            {
                var channel = new SendToClientChannel(Server, Logger);
                var records = Storage.FindRemoteSubscriptionCallbackFailureRecord(subscriber.PublisherId);
                foreach (var item in records)
                {
                    if (channel.SendMesssage(subscriber, item.Content))
                    {
                        Storage.RemoveRemoteSubscriptionCallbackFailureRecord(item._id);
                    }
                }
            }
            catch (Exception ex)
            {
                Task.Factory.StartNew((e) =>
                {
                    var exception = e as Exception;
                    if (exception != null && Logger != null)
                    {
                        Logger.Error("失败消息重试推送时失败！", exception);
                    }
                }, ex);
            }
        }
    }
}
