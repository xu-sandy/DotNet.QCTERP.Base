namespace Qct.IServices
{
    /// <summary>
    /// 订单服务（提供对订单流水号的操作）
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 保存订单递增流水号
        /// </summary>
        void SaveOrderSnIncreasingNumber(int companyId, string storeId, string machineSn, int num);
        /// <summary>
        /// 获取订单递增流水号
        /// </summary>
        /// <returns></returns>
        int GetOrderSnIncreasingNumber(int companyId, string storeId, string machineSn);
        /// <summary>
        /// 初始化订单递增流水号
        /// </summary>
        void InitOrderSnIncreasingNumber(int companyId, string storeId, string machineSn);
    }
}
