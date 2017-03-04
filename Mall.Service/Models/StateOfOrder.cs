﻿using System;
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
            ReFunding = 4,//退款中 -->订单取消

            ToAccept = 5,//待接受
            ToDelivery = 6,//待发货
            ToReceipt = 7,//待收货
            ConfirmReceipt = 8, //确认收货

            ToEvaluate = 9,//待评价
            ToRefund = 10,//待回复 -->交易完成

            ApplyReturn = 11,//申请退货
            Returning = 12,//退货中 -->订单取消

            ReturnFailed = 13,//申请退货失败 -->交易完成

            Finish = 14,//交易完成
        }
    }
}
