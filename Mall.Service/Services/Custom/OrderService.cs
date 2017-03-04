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
        public Order GetOrderById(int clientId, Guid orderId)
        {
            var orders = GetAllOrderByClientId(clientId);
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
        public List<Order> GetOrdersByState(int clientId, int orderState)
        {
            var orders = GetAllOrderByClientId(clientId);
            if (orderState == 100)
            {
                return orders;
            }
            List<Order> searchResult = orders.Where(o => o.State == orderState).ToList();
            return searchResult;
        }

        public bool ApplyRefundByOrderId(Guid orderId)
        {
            return true;
        }

        public bool ApplyReturnByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Guid CreateOrder(int goodsId, int customId, int deliveryAddressId, int count, string clientRemark)
        {
            GoodsInfo good = _db.GoodsInfo
                .SingleOrDefault(g => g.GoodsId == goodsId);
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
                PhoneNumber = custom.User.PhoneNumber,
                DeliveryAddress = delivery.DetailedAddress,
                State = (int)StateOfOrder.State.Create,
                CreateTime = DateTime.Now,
                IsDelete = false,
            };
            _db.Order.Add(order);
            _db.SaveChanges();
            return order.OrderId;
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="comment"></param>
        public void EvaluateOrder(Comment comment)
        {
            _db.Comment.Add(comment);
            _db.SaveChanges();
        }

        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public bool PayByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            DataBase.Custom custom = order.Custom;

            if (custom.Wallet >= order.Price)
            {
                custom.Wallet -= order.Price;
                order.State = (int)StateOfOrder.State.ToDelivery;

                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}