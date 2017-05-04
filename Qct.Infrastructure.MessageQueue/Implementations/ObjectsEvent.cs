using Qct.Infrastructure.Json;

namespace Qct.Infrastructure.MessageClient.Implementations
{
    /// <summary>
    /// 通用事件模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectsEvent<T> : BaseEvent
    {
        public ObjectsEvent() : base()
        { }
        /// <summary>
        /// 事件内容
        /// </summary>
        public T Content { get; set; }
    }

}
