using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.MessageServer
{
    public delegate void RemotePublish(string descriptions, string content);
    public interface IMessageQueueAdapter
    {
        /// <summary>
        /// 事件代理远程订阅
        /// </summary>
        /// <param name="group">订阅范围主题或分组</param>
        /// <param name="descriptions">订阅描述</param>
        void Subscribe(string group, RemotePublish callback);

        /// <summary>
        /// 事件代理远程推送
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="descriptions">订阅描述</param>
        /// <param name="content">事件内容</param>
        void Publish(string descriptions, string content);
        /// <summary>
        /// 取消事件远程代理订阅
        /// </summary>
        /// <param name="publisherId">事件发布器Id</param>
        /// <param name="mode">过滤模式</param>
        /// <param name="descriptions">订阅描述</param>
        void UnSubscribe(string group);
    }
}
