using Qct.Infrastructure.Data.EntityInterface;
using Qct.OrderSystem.Pay;

namespace Qct.Domain.Objects
{
    public partial class PayWay : IntegerIdEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 支付类型（对应原v3版本的apicode）
        /// </summary>
        public PayType PayType { get; set; }
        /// <summary>
        /// 支付名称或者标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 可用图标
        /// </summary>
        public string EnableIcon { get; set; }
        /// <summary>
        /// 不可用图标
        /// </summary>
        public string DisableIcon { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNumber { get; set; }
        /// <summary>
        /// 支付模式【台账、后台对接支付接口、收银端对接支付接口】
        /// </summary>
        public PayMode PayMode { get; set; }

    }
}
