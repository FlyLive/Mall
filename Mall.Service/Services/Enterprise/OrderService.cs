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
            List<Order> orders = null;
            orders = _db.Order.Where(o => o.State == state).ToList();
            if(state == (int)StateOfOrder.State.ApplyRefund)
            {
                orders = _db.Order.Where(o => o.State == state
                    || o.State == (int)StateOfOrder.State.ApplyRefund
                    || o.State == (int)StateOfOrder.State.Refunded).ToList();
            }
            else if (state == (int)StateOfOrder.State.ApplyRefund)
            {
                orders = _db.Order.Where(o => o.State == state
                    || o.State == (int)StateOfOrder.State.ApplyRefund
                    || o.State == (int)StateOfOrder.State.Refunded).ToList();
            }
            
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
        public bool AcceptOrderByOrderId(int employeeId,Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
            if (order.State == (int)StateOfOrder.State.ToAccept)
            {
                order.State = (int)StateOfOrder.State.ToDelivery;
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "接受订单(Accept)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "接受订单:" + orderId + ",等待发货！",
                    Operater = employee.User.RealName,
                    Object = "订单状态",
                    Style = "修改",
                });
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
        public bool AgreeRefundByOrderId(int employeeId,Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
            DataBase.Custom custom = _db.Custom.SingleOrDefault(c => c.CustomId == order.CustomId);

            if (order.State == (int)StateOfOrder.State.ApplyRefund)
            {
                order.State = (int)StateOfOrder.State.Refunded;
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "退款处理(Refund)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "同意订单:" + orderId + "申请的退款处理,并返还用户所支付金额！",
                    Operater = employee.User.RealName,
                    Object = "订单状态",
                    Style = "修改",
                });
                custom.Wallet += order.Totla;
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
        public void AgreeReturnByOrderId(int employeeId,Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
            DataBase.Custom custom = _db.Custom.SingleOrDefault(c => c.CustomId == order.CustomId);

            order.State = (int)StateOfOrder.State.Returning;
            _db.AdminLog.Add(new AdminLog
            {
                EmployeeId = employee.EmployeeId,
                Permission = "退货处理(Return)",
                OperationTime = DateTime.Now,
                OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "同意订单:" + orderId + "的申请退货处理！",
                Operater = employee.User.RealName,
                Object = "订单状态",
                Style = "修改",
            });
            custom.Wallet += order.Totla;
            _db.SaveChanges();
        }

        /// <summary>
        /// 确认发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ConfirmDeliverByOrderId(int employeeId,Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            GoodsInfo goods = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == order.GoodsId);
            Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
            if (order.State == (int)StateOfOrder.State.ToDelivery && goods.Stock > 0)
            {
                order.State = (int)StateOfOrder.State.ToReceipt;
                goods.Stock--;
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "确认发货(Delivery)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "对订单:" + orderId + "进行出仓发货处理！",
                    Operater = employee.User.RealName,
                    Object = "订单状态",
                    Style = "修改",
                });
                order.DeliveryTime = DateTime.Now;
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
        public bool ModifyRemarksByOrderId(int employeeId,Guid orderId, string remark)
        {
            Order order = GetOrderByOrderId(orderId);
            var state = order.State;
            if (state == (int)StateOfOrder.State.ToAccept)
            {
                Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "确认订单(Accept)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "对用户的备注进行了修改，修改前内容为:" + order.CustomRemark + "修改后为:" + remark,
                    Operater = employee.User.RealName,
                    Object = "订单备注",
                    Style = "修改",
                });
                order.CustomRemark = remark;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 拒绝退货
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="orderId"></param>
        public void RefuseReturnByOrderId(int employeeId,Guid orderId)
        {
            Order order = GetOrderByOrderId(orderId);
            if(order.State == (int)StateOfOrder.State.ApplyReturn)
            {
                Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
                order.State = (int)StateOfOrder.State.Returning;
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "退货管理(Return)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "拒绝订单:" + orderId + "的退货申请！",
                    Operater = employee.User.RealName,
                    Object = "订单状态",
                    Style = "修改",
                });
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 回复评价
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="orderId"></param>
        /// <param name="replyContent"></param>
        /// <param name="commentId"></param>
        public void ReplyByOrderId(int employeeId, Guid orderId, string replyContent, int commentId)
        {
            Employee employee = _db.Employee.Include("AdminLog").Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
            Order order = GetOrderByOrderId(orderId);
            Comment comment = _db.Comment.SingleOrDefault(c => c.CommentId == commentId);

            comment.Reply = replyContent;
            order.State = (int)StateOfOrder.State.Finish;
            _db.AdminLog.Add(new AdminLog
            {
                EmployeeId = employee.EmployeeId,
                Permission = "回复评价(Reply)",
                OperationTime = DateTime.Now,
                OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "对用户的评价进行了回复:" + replyContent,
                Operater = employee.User.RealName,
                Object = "用户的评价",
                Style = "修改",
            });

            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
