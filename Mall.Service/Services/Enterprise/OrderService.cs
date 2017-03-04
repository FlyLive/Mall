using Mall.Interface.Enterprise;
using Mall.Service.DataBase;
using Mall.Service.Models;
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

        /// <summary>
        /// 返回该状态下的所有订单
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<Order> GetOrdersByState(int state)
        {
            List<Order> orders = _db.Order.Where(o => o.State == state).ToList();
            return orders;
        }

        /// <summary>
        /// 根据订单ID返回订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrderByOrderId(Guid orderId)
        {
            Order order = _db.Order.SingleOrDefault(o => o.OrderId == orderId);
            return order;
        }

        /// <summary>
        /// 接受订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool AcceptOrderByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            if (order.State == (int)StateOfOrder.State.ToAccept)
            {
                order.State = (int)StateOfOrder.State.ToDelivery;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 同意退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool AgreeRefundByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            if (order.State == (int)StateOfOrder.State.ToDelivery)
            {
                order.State = (int)StateOfOrder.State.Returning;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 同意退货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public void AgreeReturnByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);

            order.State = (int)StateOfOrder.State.Returning;
            _db.SaveChanges();
        }

        /// <summary>
        /// 同意发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ConfirmDeliverByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            if (order.State == (int)StateOfOrder.State.ToDelivery)
            {
                order.State = (int)StateOfOrder.State.ToReceipt;
                _db.SaveChanges();
                return true;
            }
            return false;
        }


        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        public bool ModifyRemarksByOrderId(Guid orderId, string remark)
        {
            Order order = GetOrderByOrderId(orderId);
            var state = order.State;
            if (state == (int)StateOfOrder.State.ToDelivery)
            {
                order.OrderRemark = remark;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public void RefuseRefundByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            order.State = (int)StateOfOrder.State.ReturnFailed;
            _db.SaveChanges();
        }

        public void RefuseReturnByOrderId(Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            order.State = (int)StateOfOrder.State.Returning;
            _db.SaveChanges();
        }


        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
