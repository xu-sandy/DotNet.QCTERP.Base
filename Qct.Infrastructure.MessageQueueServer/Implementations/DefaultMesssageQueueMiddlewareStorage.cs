using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Qct.Infrastructure.MessageClient.ObjectModels;
using Qct.Infrastructure.MessageServer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Qct.Infrastructure.MessageServer.Implementations
{
    public class DefaultMesssageQueueMiddlewareStorage : IMQMStorage
    {
        static MongoClient client = null;
        static IMongoDatabase database = null;
        IMongoDatabase GetConnection()
        {
            if (database == null)
            {
                var connectionString = MQMConfigurationSection.GetConfig().StorageConnectionString;
                if (client == null)
                {
                    client = new MongoClient(connectionString);
                }
                database = client.GetDatabase(new MongoUrl(connectionString).DatabaseName);
            }
            return database;
        }
        IMongoCollection<T> GetEntities<T>(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                name = typeof(T).Name;
            return GetConnection().GetCollection<T>(name);
        }

        #region 客户端凭证
        /// <summary>
        /// 查找订阅的客户端凭证
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="descriptions">事件描述</param>
        /// <param name="IngoreFilterMode">是否忽略过滤模式</param>
        /// <returns>订阅客户端凭证</returns>
        public IEnumerable<ClientIndetity> FindSubscribers(Guid agentId, string descriptions)
        {
            Expression<Func<SubscribeItem, bool>> predicate;

            predicate = x => (x.FilterMode == FilterMode.WholeMatched && x.Descriptions == descriptions) || (x.FilterMode == FilterMode.StartsWith && x.Descriptions.Contains(descriptions));
            var ids = GetEntities<SubscribeItem>().AsQueryable().Where(predicate).Select(o => o.PublisherId).ToList();//查询订阅信息获取订阅者编号
            var clients = GetEntities<ClientIndetity>().AsQueryable().Where(x => ids.Contains(x.PublisherId) && x.AgentId == agentId).ToList();//通过订阅者编号查询身份信息
            return clients;
        }

        /// <summary>
        /// 查找订阅指定描述主题的客户端凭证是否存在
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="group">事件描述</param>
        /// <param name="IngoreFilterMode">是否忽略过滤模式</param>
        /// <returns>是否存在指定的订阅信息项目</returns>
        public bool HasSubscribers(Guid agentId, string group)
        {
            var ids = GetEntities<ClientIndetity>().AsQueryable().Where(o => o.AgentId == agentId).ToList().Select(o => o.PublisherId);
            Expression<Func<SubscribeItem, bool>> predicate = x => x.Descriptions.Contains(group) && ids.Contains(x.PublisherId);
            var task = GetEntities<SubscribeItem>().AsQueryable().AnyAsync(predicate);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 新增客户端凭证
        /// </summary>
        /// <param name="subscriber">客户端凭证</param>
        /// <returns>是否保存成功</returns>
        public bool AddClientIndetity(ClientIndetity subscriber)
        {
            try
            {
                GetEntities<ClientIndetity>().InsertOne(subscriber);
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 保存已更新的客户端凭证
        /// </summary>
        /// <param name="subscriber">客户端凭证</param>
        /// <returns>保存是否成功</returns>
        public bool SaveUpdateClientIndetity(ClientIndetity subscriber)
        {
            try
            {
                GetEntities<ClientIndetity>().ReplaceOne(o => o.PublisherId == subscriber.PublisherId, subscriber, new UpdateOptions { IsUpsert = true });
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 尝试获取客户端凭证
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="subscriber">客户端凭证</param>
        /// <returns>是否存在</returns>
        public bool TryGetClientIndetity(Guid publisherId, out ClientIndetity subscriber)
        {
            subscriber = null;
            try
            {
                subscriber = GetEntities<ClientIndetity>().Find(o => o.PublisherId == publisherId).FirstOrDefault();
                return subscriber != null;
            }
            catch { }
            return false;
        }
        #endregion 客户端凭证

        #region 订阅项目信息
        public IEnumerable<SubscribeItem> GetSubscribeItems(Guid agentId)
        {
            var clients = GetEntities<ClientIndetity>().Find(o => o.AgentId == agentId).ToList().Select(o => o.PublisherId);

            return GetEntities<SubscribeItem>().Find(Builders<SubscribeItem>.Filter.In(o => o.PublisherId, clients)).ToList();
        }
        /// <summary>
        /// 尝试获取订阅项目信息
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="descriptions">事件描述</param>
        /// <param name="filterMode">匹配过滤方式</param>
        /// <param name="subscribeItem">订阅项目信息</param>
        /// <returns>是否存在订阅项目</returns>
        public bool TryGetSubscribeItem(Guid publisherId, string descriptions, FilterMode filterMode, out SubscribeItem subscribeItem)
        {
            subscribeItem = GetEntities<SubscribeItem>().AsQueryable().Where(x => x.PublisherId == publisherId && x.Descriptions == descriptions && x.FilterMode == filterMode).FirstOrDefault(); ;
            return subscribeItem != null;

        }
        /// <summary>
        /// 新增订阅项目
        /// </summary>
        /// <param name="subscribeItem">订阅项目信息</param>
        /// <returns>是否保存成功</returns>
        public bool AddSubscribeItem(SubscribeItem subscribeItem)
        {
            try
            {
                GetEntities<SubscribeItem>().InsertOne(subscribeItem);
                return true;
            }
            catch { }
            return false;

        }

        /// <summary>
        /// 移除已订阅项目信息
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="subscribeItem">订阅项目信息</param>
        /// <returns>是否保存成功</returns>
        public bool RemoveSubscribeItem(Guid publisherId, SubscribeItem subscribeItem)
        {
            try
            {
                GetEntities<SubscribeItem>().DeleteOne(o => o._id == subscribeItem._id);
                return true;
            }
            catch { }
            return false;

        }
        #endregion 订阅项目信息

        #region 远程代理订阅记录
        /// <summary>
        /// 新增远程代理订阅记录
        /// </summary>
        /// <param name="agentId">消息队列代理器</param>
        /// <param name="group">订阅分组</param>
        /// <returns>保存是否成功</returns>
        public bool AddRemoteSubscribe(Guid agentId, string group)
        {
            try
            {
                GetEntities<RemoteSubscribe>().InsertOne(new RemoteSubscribe() { AgentId = agentId, Group = group });
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 是否已订阅远程订阅分组
        /// </summary>
        /// <param name="agentId">消息队列代理器</param>
        /// <param name="group">订阅分组</param>
        /// <returns>是为true,否则为false</returns>
        public bool HasRemoteSubscribe(Guid agentId, string group)
        {
            return GetEntities<RemoteSubscribe>().Find(o => o.AgentId == agentId && o.Group == group).Any();
        }
        /// <summary>
        /// 删除远程订阅分组
        /// </summary>
        /// <param name="agentId">消息队列代理器</param>
        /// <param name="group">订阅分组</param>
        /// <returns>保存是否成功</returns>
        public bool RemoveRemoteSubscribe(Guid agentId, string group)
        {
            try
            {
                if (string.IsNullOrEmpty(group))
                {
                    GetEntities<RemoteSubscribe>().DeleteMany(o => o.AgentId == agentId);
                }
                else
                {
                    GetEntities<RemoteSubscribe>().DeleteOne(o => o.AgentId == agentId && o.Group == group);
                }
                return true;
            }
            catch { }
            return false;

        }
        #endregion 远程代理订阅记录
        /// <summary>
        /// 远程订阅回调失败记录
        /// </summary>
        /// <param name="publisherId">订阅事件发布器Id</param>
        /// <param name="Content">消息</param>
        public bool AddRemoteSubscriptionCallbackFailureRecord(Guid publisherId, string Content)
        {
            try
            {
                GetEntities<RemoteSubscriptionCallbackFailureRecord>().InsertOne(new RemoteSubscriptionCallbackFailureRecord() { PublisherId = publisherId, Content = Content });
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 查找远程订阅回调失败记录
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <returns>消息内容</returns>
        public IEnumerable<RemoteSubscriptionCallbackFailureRecord> FindRemoteSubscriptionCallbackFailureRecord(Guid publisherId)
        {
            return GetEntities<RemoteSubscriptionCallbackFailureRecord>().Find(o => o.PublisherId == publisherId).ToList();
        }
        /// <summary>
        /// 删除远程订阅回调失败记录
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="item">消息内容</param>
        public bool RemoveRemoteSubscriptionCallbackFailureRecord(ObjectId id)
        {
            try
            {
                GetEntities<RemoteSubscriptionCallbackFailureRecord>().DeleteOne(o => o._id == id);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 新增远程推送失败记录
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="publishItem">推送内容</param>
        public bool AddRemotePushFailureRecord(Guid agentId, PublishItem publishItem)
        {
            try
            {
                GetEntities<RemotePushFailureRecord>().InsertOne(new RemotePushFailureRecord() { Content = publishItem, AgentId = agentId });
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 获取远程推送失败记录
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <returns>推送信息</returns>
        public IEnumerable<RemotePushFailureRecord> GetRemotePushFailureRecords(Guid agentId)
        {
            return GetEntities<RemotePushFailureRecord>().Find(o => o.AgentId == agentId).ToList();
        }

        /// <summary>
        /// 移除远程推送失败记录
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="publishItem">推送内容</param>
        public bool RemoveRemotePushFailureRecord(ObjectId id)
        {
            try
            {
                GetEntities<RemotePushFailureRecord>().DeleteOne(o => o._id == id);
                return true;
            }
            catch { }
            return false;

        }
    }
}
