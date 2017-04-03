using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Models
{
    public static class StateOfOrder
    {
        public enum State
        {
            Cancle = 0,//订单取消 交易取消
            Create = 1,//生成订单
            ToPay = 2,//待付款

            ApplyRefund = 3,//申请退款
            Refunded = 5,//完成退款

            ToAccept = 6,//待接受
            ToDelivery = 7,//待发货
            ToReceipt = 8,//待收货

            ToEvaluate = 9,//待评价 & 确认收货
            ToReply = 10,//待回复 & 交易完成

            ApplyReturn = 11,//申请退货
            Returning = 12,//退货中
            ReturnSucceed = 13,//退货成功
            ReturnFailed = 14,//退货失败

            Finish = 15,//交易完成
        }
    }
}
