using System;
using Qct.Infrastructure.Json;
using Qct.Infrastructure.MessageClient.Implementations;

namespace Qct.Infrastructure.MessageClient
{
    /// <summary>
    /// 事件处理器
    /// </summary>
    interface IEventHandler
    {
        dynamic JsonToEvent(string json);
    }
    /// <summary>
    /// 事件处理器
    /// </summary>
    interface IEventHandler<TEvent> : IEventHandler where TEvent : IEvent, new()
    {
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="_event">事件</param>
        void Handler(TEvent _event);

    }
    /// <summary>
    /// 事件处理器
    /// </summary>
    public abstract class BaseEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent, new()
    {
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="_event">事件</param>
        public abstract void Handler(TEvent _event);

        public dynamic JsonToEvent(string json)
        {
            return JsonHelper.ToObject<TEvent>(json);
        }
    }



}
