using Mall.Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Interface.Custom
{
    public interface IOrderClientApplicationData
    {
        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Guid CreateOrder(int goodsId, int clientId, int deliveryAddressId, int count, string clientRemark);
        /// <summary>
        /// 根据订单Id取消订单
        /// </summary>
        /// <param name="orderId"></param>
        void CancleOrderByOrderId(Guid orderId);
        /// <summary>
        /// 根据订单Id付款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool PayByOrderId(Guid orderId);
        /// <summary>
        /// 根据订单Id申请退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool ApplyRefundByOrderId(Guid orderId);
        /// <summary>
        /// 根据订单Id确认订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        void ConfirmOrderByOrderId(Guid orderId);
        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        void EvaluateOrder(Comment comment);
        /// <summary>
        /// 根据订单Id申请退货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool ApplyReturnByOrderId(Guid orderId);
    }
}
