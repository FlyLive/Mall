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
    public class OrderService : IDisposable, IOrderClientApplicationService
    {
        private MallDBContext _db;
        private CustomService _customService;

        public OrderService()
        {
            _db = new MallDBContext();
            _customService = new CustomService();
        }

        /// <summary>
        /// 根据订单状态获取订单集合
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public List<Order> GetOrdersByState(int customId, int orderState)
        {
            var orders = GetAllOrderByCustomId(customId);
            List<Order> searchResult = orders.Where(o => o.State == orderState).ToList();
            if (orderState == 100)
            {
                searchResult = orders;
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
            return searchResult.OrderByDescending(o => o.CreateTime).ToList();
        }

        /// <summary>
        /// 根据订单Id获取订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        private Order GetOrderByOrderId(Guid orderId)
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
            var orders = GetAllOrderByCustomId(customId);
            Order order = orders.SingleOrDefault(o => o.OrderId == orderId);
            return order;
        }

        /// <summary>
        /// 根据客户Id获取所有订单
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <returns></returns>
        public List<Order> GetAllOrderByCustomId(int customId)
        {
            List<Order> orders = _db.Order.Include("GoodsInfo")
                .Where(o => o.CustomId == customId).ToList();
            return orders;
        }

        /// <summary>
        /// 根据客户Id获取未完成订单的数量
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <returns></returns>
        public int GetUnfinishedOrderByCustomId(int customId)
        {
            List<Order> orders = _db.Order
                .Where(o => o.CustomId == customId
                    && o.State != (int)StateOfOrder.State.Cancle
                    && o.State != (int)StateOfOrder.State.Refunded
                    && o.State != (int)StateOfOrder.State.Finish
                    && o.State != (int)StateOfOrder.State.ReturnFailed
                    && o.State != (int)StateOfOrder.State.ReturnSucceed
                ).ToList();

            return orders.Count;
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
                || order.State == (int)StateOfOrder.State.ToEvaluate
                || order.State == (int)StateOfOrder.State.ToReply)
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
        public bool CancleOrderByOrderId(Guid orderId)
        {
            try
            {
                Order order = GetOrderByOrderId(orderId);
                order.State = (int)StateOfOrder.State.Cancle;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        public bool ConfirmOrderByOrderId(Guid orderId)
        {
            try
            {
                Order order = GetOrderByOrderId(orderId);
                if (order.State == (int)StateOfOrder.State.ToReceipt)
                {
                    order.State = (int)StateOfOrder.State.ToEvaluate;
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        public void SendOrderEmail(string email, string goodsName, Guid orderId)
        {
            try
            {
                StringBuilder strBody = new StringBuilder();
                strBody.Append("您好，您在本商城下单的商品:&emsp;<h4 style='color:red'>" + goodsName + "</h4>&emsp;</br>");
                strBody.Append("本次交易<h3>订单编号:&emsp;" + orderId + "</h3></br>");
                strBody.Append("感谢您使用云翳商城购物！");
                _customService.SendEmail(email, "订单信息", strBody.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Guid CreateOrder(int goodsId, int customId, int deliveryAddressId, int count, string customRemark)
        {
            try
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
                    Freight = good.Freight,
                    Count = count,
                    Totla = good.Price * count + good.Freight,
                    Consignee = delivery.Consignee,
                    PhoneNumber = delivery.PhoneNumber,
                    DeliveryAddress = delivery.DetailedAddress,
                    State = (int)StateOfOrder.State.ToPay,
                    CreateTime = DateTime.Now,
                    IsDelete = false,
                    CustomRemark = customRemark,
                };

                _db.Order.Add(order);
                _db.SaveChanges();

                SendOrderEmail(custom.User.Email, good.GoodsName, order.OrderId);

                return order.OrderId;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return new Guid();
            }
        }

        /// <summary>
        /// 从购物车创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Guid CreateOrderFromCart(int goodsId, int customId, int deliveryAddressId, string customRemark)
        {
            ShoppingCart cart = _db.ShoppingCart.Include("GoodsInfo")
                .SingleOrDefault(c => c.CustomId == customId && c.GoodsId == goodsId);
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
            try
            {
                List<Order> orders = new List<Order>();
                double totlaPay = 0;

                DataBase.Custom custom = _db.Custom.SingleOrDefault(c => c.CustomId == customId);

                orderId.ToList().ForEach(o => orders.Add(GetOrderById(customId, o)));

                orders.ForEach(o => totlaPay = o.Totla + totlaPay);

                foreach (var o in orders)
                {
                    var goodState = o.GoodsInfo.State;
                    if (goodState == (int)StateOfGoods.State.Delet || goodState == (int)StateOfGoods.State.OffShelves)
                    {
                        return false;
                    }
                }

                if (custom.Wallet >= totlaPay)
                {
                    custom.Wallet -= totlaPay;
                    orders.ForEach(o => o.State = (int)StateOfOrder.State.ToAccept);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
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
            try
            {
                order.State = (int)StateOfOrder.State.ToEvaluate;
                order.ReceiptTime = DateTime.Now;
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="comment"></param>
        public bool EvaluateOrder(int customId, Guid orderId, string evaluateContent)
        {
            try
            {
                DataBase.Custom custom = _db.Custom.SingleOrDefault(c => c.CustomId == customId);
                Order order = GetOrderByOrderId(orderId);
                order.State = (int)StateOfOrder.State.ToReply;
                order.GoodsInfo.CommentNumber++;
                _db.Comment.Add(
                    new Comment
                    {
                        CustomId = customId,
                        CommentDetail = evaluateContent == null ? "该用户没有任何评论" : evaluateContent,
                        CommentTime = DateTime.Now,
                        GoodsId = order.GoodsId,
                        OrderId = order.OrderId
                    });
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}