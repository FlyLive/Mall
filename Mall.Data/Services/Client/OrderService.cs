using Mall.Data.IManagers.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mall.Data.DataBase;

namespace Mall.Data.Services.Client
{
    public class OrderService : IDisposable, IOrderClientManager
    {
        private MallDBContext _db;
        private ClientService _slientService;

        public OrderService()
        {
            _db = new MallDBContext();
            _slientService = new ClientService();
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

        public void AddGoodsToShoppingCart(int clientId, int goodsId, int count = 1)
        {
            DataBase.Client client = _db.Client.SingleOrDefault(c => c.ClientId == clientId);
            

            //client.ShoppingCart
            
        }

        public bool ApplyRefundByOrderId(Guid orderId)
        {
            //Order order = 
            return true;
        }

        public bool ApplyReturnByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void CancleOrderByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void ConfirmOrderByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public bool CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void DeleteGoodsFromShoppingCart(int cilentId, int goodsId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void EvaluateOrder(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void ModifyGoodsCountFromShoppingCart(int cilentId, int goodsId, int count)
        {
            throw new NotImplementedException();
        }

        public bool PayByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
