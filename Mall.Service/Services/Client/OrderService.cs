using Mall.Service.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mall.Service.DataBase;
using Mall.Service.Models;
using Mall.Interface.Client;

namespace Mall.Service.Services.Client
{
    public class OrderService : IDisposable, IOrderClientApplicationData
    {
        private MallDBContext _db;
        private CustomService _clientData;

        public OrderService()
        {
            _db = new MallDBContext();
            _clientData = new CustomService();
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
                .Where(o => o.ClientId == clientId).ToList();
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

        /// <summary>
        /// 向购物车添加商品
        /// </summary>
        /// <param name="clientId">客户ID</param>
        /// <param name="goodsId">商品ID</param>
        /// <param name="count">数量</param>
        public void AddGoodsToShoppingCart(int clientId, int goodsId, int count = 1)
        {
            DataBase.Client client = _db.Client.Include("ShoppingCart").SingleOrDefault(c => c.ClientId == clientId);
            client.ShoppingCart.Add(new ShoppingCart
            {
                GoodsId = goodsId,
                CreateTime = DateTime.Now,
                Number = count,
            });
            _db.SaveChanges();
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
        public Guid CreateOrder(int goodsId, int clientId,int deliveryAddressId, int count, string clientRemark)
        {
            GoodsInfo good = _db.GoodsInfo
                .SingleOrDefault(g => g.GoodsId == goodsId);
            DataBase.Client client = _db.Client.Include("User")
                .SingleOrDefault(c => c.ClientId == clientId);
            DeliveryInfo delivery = _db.DeliveryInfo
                .SingleOrDefault(d => d.Id == deliveryAddressId);
            Order order = new Order
            {
                OrderId = Guid.NewGuid(),
                GoodsId = goodsId,
                GoodsName = good.GoodsName,
                ClientId = clientId,
                Price = good.Price,
                Freight = good.freight,
                Count = count,
                Totla = good.Price * count + good.freight,
                Consignee = delivery.Consignee,
                PhoneNumber = client.User.PhoneNumber,
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
        /// 删除购物车中商品
        /// </summary>
        /// <param name="cilentId"></param>
        /// <param name="goodsId"></param>
        public void DeleteGoodsFromShoppingCart(int clientId, int goodsId)
        {
            DataBase.Client client = _db.Client.Include("ShoppingCart").SingleOrDefault(c => c.ClientId == clientId);
            client.ShoppingCart.Remove(
                client.ShoppingCart.
                    SingleOrDefault(s => s.GoodsId == goodsId)
            );
            _db.SaveChanges();
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
        /// 修改购物车中商品数量
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="goodsId"></param>
        /// <param name="count"></param>
        public void ModifyGoodsCountFromShoppingCart(int clientId, int goodsId, int count)
        {
            ShoppingCart cart = _db.ShoppingCart
                .SingleOrDefault(c => c.ClientId == clientId && c.GoodsId == goodsId);
            cart.Number = count;
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
            DataBase.Client client = order.Client;

            if (client.Wallet >= order.Price)
            {
                client.Wallet -= order.Price;
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