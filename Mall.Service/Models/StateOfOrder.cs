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
            ReFunding = 4,//退款中 & 订单取消

            ToAccept = 5,//待接受
            ToDelivery = 6,//待发货
            ToReceipt = 7,//待收货

            ToEvaluate = 8,//待评价 & 确认收货
            ToRefund = 9,//待回复 & 交易完成

            ApplyReturn = 10,//申请退货
            Returning = 11,//退货中 & 订单取消

            ReturnFailed = 12,//申请退货失败 & 交易完成

            Finish = 13,//交易完成
        }
    }
}
