using Qct.Infrastructure.MessageClient;
using Qct.Exceptions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Qct
{
    /// <summary>
    /// 订单流水号,支持订单号锁定
    /// </summary>
    [ComplexType]
    public class OrderSn
    {
        /// <summary>
        /// 订单流水号初始化
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="storeId">门店ID</param>
        /// <param name="machineSn">设备编号</param>
        public OrderSn(int companyId, string storeId, string machineSn)
        {
            if (IncreasingNumber == 0)
                throw new OrderException("订单流水数据尚未初始化，请在程序启动是加载历史数据，并设置初次启动的流水数！");
            CompanyId = companyId;
            StoreId = storeId;
            MachineSn = machineSn;
            LockIncreasingNumber = 0;
        }
        /// <summary>
        /// 订单递增数
        /// </summary>
        [NotMapped]
        private static int IncreasingNumber { get; set; }

        [Column("OrderSN")]
        public string StoreOrderSerialNumber { get { return GetStoreOrderSerialNumber(); } set { AnalyseStoreOrderSerialNumber(value); } }
        [NotMapped]
        /// <summary>
        /// 锁定订单递增数
        /// </summary>
        public int LockIncreasingNumber { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 门店ID
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineSn { get; set; }
        /// <summary>
        /// 获取设备下适用的订单流水号【2位设备号+8位日期+5位流水号】，并保证在本门店下不重复
        /// </summary>
        /// <returns></returns>
        public string GetMachineOrderSerialNumber()
        {

            return string.Format("{0}{1:yyyyMMdd}{2:00000}", MachineSn.PadLeft(2, '0'), DateTime.Now, LockIncreasingNumber == 0 ? IncreasingNumber : LockIncreasingNumber);
        }
        /// <summary>
        /// 获取门店下适用的订单流水号【5位门店号+2位设备号+8位日期+5位流水号】，并保证在全公司范围内不重复
        /// </summary>
        /// <returns></returns>
        public string GetStoreOrderSerialNumber()
        {
            return string.Format("{3}{0}{1:yyyyMMdd}{2:00000}", MachineSn.PadLeft(2, '0'), DateTime.Now, LockIncreasingNumber == 0 ? IncreasingNumber : LockIncreasingNumber, StoreId.PadLeft(5, '0'));
        }

        private void AnalyseStoreOrderSerialNumber(string orderSn)
        {
            var numText = orderSn.Substring(orderSn.Length - 5, 5);
            LockIncreasingNumber = int.Parse(numText);
        }
        /// <summary>
        /// 获取当前的递增流水号
        /// </summary>
        /// <returns></returns>
        public int GetCurrentIncreasingNumber()
        {
            return IncreasingNumber;
        }
        /// <summary>
        /// 锁定当前流水号
        /// </summary>
        public void SetLockIncreasingNumber()
        {
            LockIncreasingNumber = IncreasingNumber;
            NextNumber();
        }
        /// <summary>
        /// 订单流水数据+1
        /// </summary>
        public void NextNumber()
        {
            IncreasingNumber++;
            var publisher = PublisherFactory.Create();
            publisher.PublishAsync(string.Format("Qct.LocalPos.NextIncreasingNumber.{0}.{1}.{2}", CompanyId, StoreId, MachineSn), IncreasingNumber, true);
        }
        /// <summary>
        /// 重置订单流水数据
        /// </summary>
        /// <param name="num"></param>
        public void ResetNumber(int num)
        {
            if (IncreasingNumber <= num)
                IncreasingNumber = num;
            else
                throw new OrderException("设置的订单流水数据不能小于当前流水数据！");
        }

        public override string ToString()
        {
            return GetStoreOrderSerialNumber();
        }
    }
}
