using System;

namespace Qct.Infrastructure.MessageServer.Extensions
{
    /// <summary>
    /// 事件描述扩展方法类
    /// </summary>
    public static class DescriptionExtensions
    {
        /// <summary>
        /// 事件描述/订阅描述分隔符
        /// </summary>
        internal const string Separator = ".";
        /// <summary>
        /// 比较两个描述
        /// </summary>
        /// <param name="descriptionsA">A描述</param>
        /// <param name="descriptionsB">B描述</param>
        /// <returns>是否等同</returns>
        public static bool EqualsTo(this string descriptionsA, string descriptionsB)
        {
            return descriptionsA == descriptionsB;
        }

        /// <summary>
        /// 确定此事件描述实例的开头是否与指定的事件描述匹配。
        /// </summary>
        /// <param name="descriptionsA">A事件描述</param>
        /// <param name="descriptionsB">B事件描述</param>
        /// <returns>如果B事件描述与A事件描述的开头匹配，则为 true；否则为 false。</returns>
        public static bool StartsWith(this string descriptionsA, string descriptionsB)
        {
            return descriptionsA.StartsWith(descriptionsB);
        }


        /// <summary>
        /// 获取事件描述的第一项作为事件的分组
        /// </summary>
        /// <param name="descriptions">事件描述</param>
        /// <returns>返回事件分组</returns>
        public static string GetGroup(this string descriptions)
        {
            if (descriptions == null || descriptions.Length < 1)
                throw new IndexOutOfRangeException("描述信息不全！");
            return descriptions.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}

