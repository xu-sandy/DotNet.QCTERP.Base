using System;

namespace Qct.Infrastructure.MessageClient.ObjectModels
{
    /// <summary>
    /// 推送匹配模式
    /// </summary>
    [Serializable]
    public enum FilterMode
    {
        /// <summary>
        /// 全匹配（EventDescription 描述内容完成相符）
        /// </summary>
        WholeMatched = 0,
        /// <summary>
        ///订阅描述/事件描述按顺序与固定字符串（默认为#@.@#）进行拼接， 事件描述从字符串头部开始包含订阅描述指定内容
        /// </summary>
        StartsWith = 1,
        ///// <summary>
        ///// 当EventDescription描述内容为*，FilterMode.FullEvent 所有事件都满足,只匹配系统内部事件，不能匹配远程事件
        ///// </summary>
        //FullLocalEvent = 2
    }
}
