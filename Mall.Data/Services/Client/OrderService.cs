using Mall.Data.Interface.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mall.Data.DataBase;
using Mall.Data.Models;

namespace Mall.Data.Services.Client
{
    public class OrderService : IDisposable, IOrderClientApplicationService
    {
        private MallDBContext _db;
        private ClientService _clientService;

        public OrderService()
        {
            _db = new MallDBContext();
            _clientService = new ClientService();
        }

        /// <summary>
        /// 根据订单Id获取订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public Order GetOrderById(Guid orderId)
        {
            Order order = _db.Order.SingleOrDefault(o => o.OrderId == orderId);
            return order;
        }

        /// <summary>
        /// 根据客户Id获取所有订单
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <returns></returns>
        public List<Order> GetAllOrderByClientId(int clientId)
        {
            List<Order> orders = _db.Order.Where(o => o.ClientId == clientId).ToList();
            return orders;
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
            Order order = GetOrderById(orderId);
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
            Order order = GetOrderById(orderId);
            order.State = (int)StateOfOrder.State.Cancle;
            _db.SaveChanges();
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        public void ConfirmOrderByOrderId(Guid orderId)
        {
            Order order = GetOrderById(orderId);
            order.State = (int)StateOfOrder.State.ToEvaluate;
            _db.SaveChanges();
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool CreateOrder(Order order)
        {
            _db.Order.Add(order);
            _db.SaveChanges();
            return true;
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
            ShoppingCart cart = _db.ShoppingCart.SingleOrDefault(c => c.ClientId == clientId && c.GoodsId == goodsId);
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
            Order order = GetOrderById(orderId);
            DataBase.Client client = order.Client;

            if(client.Wallet >= order.Price)
            {
                client.Wallet -= order.Price;
                order.State = 2 ;

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