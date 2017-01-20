using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.IManagers.Client
{
    public interface IOrderClientManager
    {
        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool CreateOrder(Order order);
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
        /// <summary>
        /// 根据顾客Id和商品Id和数量向购物车添加商品
        /// </summary>
        /// <param name="clientId"></param>
        void AddGoodsToShoppingCart(int clientId, int goodsId, int count = 1);
        /// <summary>
        /// 根据顾客Id和商品Id和数量向购物车添加商品
        /// </summary>
        /// <param name="cilentId"></param>
        void DeleteGoodsFromShoppingCart(int cilentId, int goodsId);
        /// <summary>
        /// 根据顾客Id和商品Id和数量向购物车修改商品数量
        /// </summary>
        /// <param name="cilentId"></param>
        void ModifyGoodsCountFromShoppingCart(int cilentId, int goodsId, int count);
    }
}
