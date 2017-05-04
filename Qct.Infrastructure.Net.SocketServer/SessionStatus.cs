namespace Qct.SuperSocketProtocol
{
    /// <summary>
    /// 会话状态
    /// </summary>
    public enum SessionStatus
    {
        /// <summary>
        /// 正在初始化
        /// </summary>
        Initializing = 0,
        /// <summary>
        /// 已连接
        /// </summary>
        Started = 1,
        /// <summary>
        /// 正在关闭
        /// </summary>
        Closing = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        Closed = 3
    }
}
