using Qct.Exceptions;

namespace Qct.Objects.Entities
{
    public partial class PayConfiguration
    {
        public void VerfyState()
        {
            if (!State)
                throw new OrderException("支付配置尚未启用，无法授权支付！");
        }
    }
}
