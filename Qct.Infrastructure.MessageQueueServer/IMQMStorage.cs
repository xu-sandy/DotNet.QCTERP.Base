using MongoDB.Bson;
using Qct.Infrastructure.MessageClient.ObjectModels;
using Qct.Infrastructure.MessageServer.Objects;
using System;
using System.Collections.Generic;

namespace Qct.Infrastructure.MessageServer
{
    public interface IMQMStorage
    {
        #region 客户端凭证
        /// <summary>
        /// 查找订阅指定描述主题的客户端凭证
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="descriptions">事件描述</param>
        /// <param name="IngoreFilterMode">是否忽略过滤模式</param>
        /// <returns>订阅指定描述主题的客户端凭证</returns>
        IEnumerable<ClientIndetity> FindSubscribers(Guid agentId, string descriptions);

        /// <summary>
        /// 查找订阅指定描述主题的客户端凭证是否存在
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="group">事件最顶级描述</param>
        /// <returns>是否存在指定的订阅信息项目</returns>
        bool HasSubscribers(Guid agentId, string group);

        /// <summary>
        /// 新增客户端凭证
        /// </summary>
        /// <param name="subscriber">客户端凭证</param>
        /// <returns>是否保存成功</returns>
        bool AddClientIndetity(ClientIndetity subscriber);
        /// <summary>
        /// 保存已更新的客户端凭证
        /// </summary>
        /// <param name="subscriber">客户端凭证</param>
        /// <returns>保存是否成功</returns>
        bool SaveUpdateClientIndetity(ClientIndetity subscriber);
        /// <summary>
        /// 尝试获取客户端凭证
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="subscriber">客户端凭证</param>
        /// <returns>是否存在</returns>
        bool TryGetClientIndetity(Guid publisherId, out ClientIndetity subscriber);
        #endregion 客户端凭证

        #region 订阅项目信息
        IEnumerable<SubscribeItem> GetSubscribeItems(Guid agentId);
        /// <summary>
        /// 尝试获取订阅项目信息
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="descriptions">事件描述</param>
        /// <param name="filterMode">匹配过滤方式</param>
        /// <param name="subscribeItem">订阅项目信息</param>
        /// <returns>是否存在订阅项目</returns>
        bool TryGetSubscribeItem(Guid publisherId, string descriptions, FilterMode filterMode, out SubscribeItem subscribeItem);
        /// <summary>
        /// 新增订阅项目
        /// </summary>
        /// <param name="subscribeItem">订阅项目信息</param>
        /// <returns>是否保存成功</returns>
        bool AddSubscribeItem(SubscribeItem subscribeItem);

        /// <summary>
        /// 移除已订阅项目信息
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="subscribeItem">订阅项目信息</param>
        /// <returns>是否保存成功</returns>
        bool RemoveSubscribeItem(Guid publisherId, SubscribeItem subscribeItem);
        #endregion 订阅项目信息

        #region 远程代理订阅记录
        /// <summary>
        /// 新增远程代理订阅记录
        /// </summary>
        /// <param name="agentId">消息队列代理器</param>
        /// <param name="group">订阅分组</param>
        /// <returns>保存是否成功</returns>
        bool AddRemoteSubscribe(Guid agentId, string group);
        /// <summary>
        /// 是否已订阅远程订阅分组
        /// </summary>
        /// <param name="agentId">消息队列代理器</param>
        /// <param name="group">订阅分组</param>
        /// <returns>是为true,否则为false</returns>
        bool HasRemoteSubscribe(Guid agentId, string group);
        /// <summary>
        /// 删除远程订阅分组
        /// </summary>
        /// <param name="agentId">消息队列代理器</param>
        /// <param name="group">订阅分组</param>
        /// <returns>保存是否成功</returns>
        bool RemoveRemoteSubscribe(Guid agentId, string group);
        #endregion 远程代理订阅记录
        /// <summary>
        /// 远程订阅回调失败记录
        /// </summary>
        /// <param name="publisherId">订阅事件发布器Id</param>
        /// <param name="Content">消息</param>
        bool AddRemoteSubscriptionCallbackFailureRecord(Guid publisherId, string Content);

        /// <summary>
        /// 查找远程订阅回调失败记录
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <returns>消息内容</returns>
        IEnumerable<RemoteSubscriptionCallbackFailureRecord> FindRemoteSubscriptionCallbackFailureRecord(Guid publisherId);
        /// <summary>
        /// 删除远程订阅回调失败记录
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="item">消息内容</param>
        bool RemoveRemoteSubscriptionCallbackFailureRecord(ObjectId id);

        /// <summary>
        /// 新增远程推送失败记录
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="publishItem">推送内容</param>
        bool AddRemotePushFailureRecord(Guid agentId, PublishItem publishItem);

        /// <summary>
        /// 获取远程推送失败记录
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <returns>推送信息</returns>
        IEnumerable<RemotePushFailureRecord> GetRemotePushFailureRecords(Guid agentId);

        /// <summary>
        /// 移除远程推送失败记录
        /// </summary>
        /// <param name="agentId">消息队列代理器Id</param>
        /// <param name="publishItem">推送内容</param>
        bool RemoveRemotePushFailureRecord(ObjectId id);


    }
}
