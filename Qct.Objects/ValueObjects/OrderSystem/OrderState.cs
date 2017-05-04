using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct
{
    public enum OrderState
    {
        /// <summary>
        /// 订单刚被创建
        /// </summary>
        Created = 1,
        /// <summary>
        /// 已支付下单【POS销售时，支付完成保存订单，消息队列发送订单支付完成消息，系统自动完成该订单】
        /// </summary>
        Paid = 2,
        /// <summary>
        /// 订单取消
        /// </summary>
        Cannel = 3,
        /// <summary>
        /// 已完成订单（数字为8，为配货送货上门预留）
        /// </summary>
        Complete = 8


    }
}
