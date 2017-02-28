using Mall.Interface.Enterprise;
using Mall.Service.DataBase;
using Mall.Service.Services.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Services.Enterprise
{
    public class OrderService : IDisposable, IOrderEntepriseApplicationData
    {
        private MallDBContext _db;

        public OrderService()
        {
            _db = new MallDBContext();
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = _db.Order.ToList();
            return orders;
        }

        public Order GetOrderByOrderId(Guid orderId)
        {
            Order order = _db.Order.SingleOrDefault(o => o.OrderId == orderId);
            return order;
        }


        public void AcceptOrderByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            _db.SaveChanges();
        }

        public void AgreeRefundByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            _db.SaveChanges();
        }

        public void AgreeReturnByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            _db.SaveChanges();
        }

        public void ConfirmDeliverByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            _db.SaveChanges();
        }


        /// <summary>
        /// 回复评价
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        public void ModifyRemarksByOrderId(Guid orderId, string remark)
        {
            Order order = GetOrderByOrderId(orderId);
            order.OrderRemark = remark;
            _db.SaveChanges();
        }

        public void RefuseRefundByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            _db.SaveChanges();
        }

        public void RefuseReturnByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
