namespace Qct.Infrastructure.MessageClient
{
    /// <summary>
    /// 事件订阅区块
    /// </summary>
    public interface ISubscribeArea
    {
        /// <summary>
        /// 注册区块
        /// </summary>
        /// <param name="eventPublisher">事件发布器</param>
        void RegisterArea(IPublisher eventPublisher);
    }
}
