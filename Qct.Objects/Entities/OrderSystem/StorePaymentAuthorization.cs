using Qct.Infrastructure.Data.EntityInterface;
using Qct.Pay;

namespace Qct.Objects.Entities
{
    public partial class StorePaymentAuthorization : IntegerIdEntity
    {
        public int Id { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 门店Id
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 支付平台开通门店映射
        /// </summary>
        public string MapPaymentStoreId { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public PayType PayType { get; set; }
        /// <summary>
        /// 是否已经授权
        /// </summary>
        public bool State { get; set; }

    }
}
