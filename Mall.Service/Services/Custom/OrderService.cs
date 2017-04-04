using Mall.Service.Services.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mall.Service.DataBase;
using Mall.Service.Models;
using Mall.Interface.Custom;

namespace Mall.Service.Services.Custom
{
    public class OrderService : IDisposable, IOrderClientApplicationData
    {
        private MallDBContext _db;
        private CustomService _customService;

        public OrderService()
        {
            _db = new MallDBContext();
            _customService = new CustomService();
        }

        /// <summary>
        /// 根据订单Id获取订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public Order GetOrderByOrderId(Guid orderId)
        {
            Order order = _db.Order
                .SingleOrDefault(o => o.OrderId == orderId);
            return order;
        }

        /// <summary>
        /// 根据订单Id获取客户订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public Order GetOrderById(int customId, Guid orderId)
        {
            var orders = GetAllOrderByClientId(customId);
            Order order = orders.SingleOrDefault(o => o.OrderId == orderId);
            return order;
        }

        /// <summary>
        /// 根据客户Id获取所有订单
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <returns></returns>
        public List<Order> GetAllOrderByClientId(int clientId)
        {
            List<Order> orders = _db.Order
                .Where(o => o.CustomId == clientId).ToList();
            return orders;
        }

        /// <summary>
        /// 根据订单状态获取订单集合
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public List<Order> GetOrdersByState(int customId, int orderState)
        {
            var orders = GetAllOrderByClientId(customId);
            List<Order> searchResult = orders.Where(o => o.State == orderState).ToList();
            if (orderState == 100)
            {
                return orders;
            }
            else if (orderState == (int)StateOfOrder.State.ToDelivery)
            {
                searchResult = orders.Where(o => o.State == (int)StateOfOrder.State.ToAccept ||
                    o.State == (int)StateOfOrder.State.ToDelivery).ToList();
            }
            else if (orderState == (int)StateOfOrder.State.Finish)
            {
                searchResult = orders.Where(o => o.State == (int)StateOfOrder.State.Finish ||
                    o.State == (int)StateOfOrder.State.ReturnFailed ||
                    o.State == (int)StateOfOrder.State.ToReply).ToList();
            }
            else if (orderState == (int)StateOfOrder.State.ApplyRefund)
            {
                searchResult = orders.Where(o => o.State == (int)StateOfOrder.State.ApplyRefund ||
                    o.State == (int)StateOfOrder.State.Refunded).ToList();
            }
            else if (orderState == (int)StateOfOrder.State.ApplyReturn)
            {
                searchResult = orders.Where(o => o.State == (int)StateOfOrder.State.ApplyReturn ||
                    o.State == (int)StateOfOrder.State.Returning ||
                    o.State == (int)StateOfOrder.State.ReturnSucceed ||
                    o.State == (int)StateOfOrder.State.ReturnFailed).ToList();
            }
            return searchResult;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ApplyRefundByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            int orderState = order.State;
            if (orderState == (int)StateOfOrder.State.ToDelivery || orderState == (int)StateOfOrder.State.ToAccept)
            {
                order.State = (int)StateOfOrder.State.ApplyRefund;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ApplyReturnByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            if ((order.State == (int)StateOfOrder.State.Finish
                || Model.State == (int)StateOfOrder.State.ToEvaluate
                || Model.State == (int)StateOfOrder.State.ToReply)
                && (DateTime.Now - order.ReceiptTime.Value).TotalDays <= 7)
            {
                order.State = (int)StateOfOrder.State.ApplyReturn;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据订单Id取消订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        public void CancleOrderByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            order.State = (int)StateOfOrder.State.Cancle;
            _db.SaveChanges();
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        public void ConfirmOrderByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            order.State = (int)StateOfOrder.State.ToEvaluate;
            _db.SaveChanges();
        }

        public Guid CreateOrder(int goodsId, int customId, int deliveryAddressId, int count, string customRemark)
        {
            GoodsInfo good = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);

            DataBase.Custom custom = _db.Custom.Include("User")
                .SingleOrDefault(c => c.CustomId == customId);

            DeliveryInfo delivery = _db.DeliveryInfo
                .SingleOrDefault(d => d.Id == deliveryAddressId);

            Order order = new Order
            {
                OrderId = Guid.NewGuid(),
                GoodsId = goodsId,
                GoodsName = good.GoodsName,
                CustomId = customId,
                Price = good.Price,
                Freight = good.freight,
                Count = count,
                Totla = good.Price * count + good.freight,
                Consignee = delivery.Consignee,
                PhoneNumber = delivery.PhoneNumber,
                DeliveryAddress = delivery.DetailedAddress,
                State = (int)StateOfOrder.State.ToPay,
                CreateTime = DateTime.Now,
                IsDelete = false,
                CustomRemark = customRemark,
            };

            custom.Order.Add(order);
            good.Order.Add(order);

            _db.Order.Add(order);
            _db.SaveChanges();

            return order.OrderId;
        }

        /// <summary>
        /// 从购物车创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Guid CreateOrderFromCart(int goodsId, int customId, int deliveryAddressId, string customRemark)
        {
            ShoppingCart cart = _db.ShoppingCart.Include("GoodsInfo")
                .SingleOrDefault(c => c.GoodsId == goodsId);
            Guid orderId = CreateOrder(goodsId, customId, deliveryAddressId, cart.Number, customRemark);

            _db.ShoppingCart.Remove(cart);
            _db.SaveChanges();
            return orderId;
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public bool PayByOrderId(int customId, Guid[] orderId)
        {
            List<Order> orders = new List<Order>();
            double totlaPay = 0;

            DataBase.Custom custom = _db.Custom.SingleOrDefault(c => c.CustomId == customId);

            orderId.ToList().ForEach(o => orders.Add(GetOrderById(customId, o)));

            orders.ForEach(o => totlaPay = o.Totla + totlaPay);

            if (custom.Wallet >= totlaPay)
            {
                custom.Wallet -= totlaPay;
                orders.ForEach(o => o.State = (int)StateOfOrder.State.ToAccept);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ConfirmReceipt(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            if (order != null)
            {
                order.State = (int)StateOfOrder.State.ToEvaluate;
                order.ReceiptTime = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="comment"></param>
        public void EvaluateOrder(int customId, Guid orderId, string evaluateContent)
        {
            DataBase.Custom custom = _db.Custom.SingleOrDefault(c => c.CustomId == customId);
            Order order = GetOrderByOrderId(orderId);
            order.State = (int)StateOfOrder.State.ToReply;
            order.GoodsInfo.CommentNumber++;
            _db.Comment.Add(
                new Comment
                {
                    CustomId = customId,
                    CommentDetail = evaluateContent,
                    CommentTime = DateTime.Now,
                    GoodsId = order.GoodsId,
                    OrderId = order.OrderId
                });
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}