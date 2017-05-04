using System;

namespace Qct.Infrastructure.Attributes
{
    /// <summary>
    /// 属性复制排除标志(用于自动属性复制)
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExcludeAttribute : Attribute
    {
        public ExcludeAttribute()
        {
        }
    }
}
