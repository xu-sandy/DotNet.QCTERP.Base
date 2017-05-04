using Qct.Infrastructure.MessageClient.ObjectModels;
using System;

namespace Qct.Infrastructure.MessageClient.Implementations
{
    /// <summary>
    /// 通用事件回调包装器
    /// </summary>
    public sealed class EventHandlerActionWrapper<TEvent> : BaseEventHandler<TEvent>
        where TEvent : IEvent, new()
    {
        public EventHandlerActionWrapper(Action<TEvent> callBack)
        {
            if (callBack == null)
            {
                throw new DomianEventException("事件回调处理方法不能为空！");
            }
            EventHandlerCallBack = callBack;
        }
        /// <summary>
        /// 事件回调
        /// </summary>
        private Action<TEvent> EventHandlerCallBack { get; set; }

        public override void Handler(TEvent _event)
        {
            EventHandlerCallBack(_event);
        }

    }
}
