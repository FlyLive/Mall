using Mall.Service.DataBase;
using Mall.Service.Models;
using Mall.Service.Services.Enterprise;
using Mall.Web.Back.Filter;
using Mall.Web.Back.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class OrderController : Controller
    {
        private EnterpriseService _enterpriseService = new EnterpriseService();
        private MenuViewService _menuViewService = new MenuViewService();
        private OrderService _orderService = new OrderService();

        #region 订单管理
        [HttpGet]
        public ActionResult AllOrders(int page = 1, int pageSize = 10)
        {
            LoginTest();
            List<Order> orders = _orderService.GetAllOrders();
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page, pageSize);
            return View(orderDTO);
        }

        #region 接受订单
        [HttpGet]
        [PermissionAuthorize("Accept")]
        public ActionResult ToAccept(int page = 1,int pageSize = 10)
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ToAccept);
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page, pageSize);
            return View(orderDTO);
        }

        [HttpPost]
        [PermissionAuthorize("Accept")]
        public bool AcceptOrder(Guid orderId)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            var result = _orderService.AcceptOrderByOrderId(employee.EmployeeId,orderId);
            return result;
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="marks"></param>
        /// <returns></returns>
        public bool ModifyRemark(Guid orderId, string mark)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];

            _orderService.ModifyRemarksByOrderId(employee.EmployeeId,orderId, mark);
            return true;
        }
        #endregion

        #region 确认发货
        [HttpGet]
        [PermissionAuthorize("Delivery")]
        public ActionResult ToDelivery(int page = 1, int pageSize = 10)
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ToDelivery);
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page, pageSize);
            return View(orderDTO);
        }

        [HttpPost]
        [PermissionAuthorize("Delivery")]
        public bool DeliveryOrder(Guid orderId)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            var result = _orderService.ConfirmDeliverByOrderId(employee.EmployeeId,orderId);
            return result;
        }
        #endregion

        #region 确认退款
        [HttpGet]
        [PermissionAuthorize("Refund")]
        public ActionResult RefundConfirm(int page = 1,int pageSize = 10)
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ApplyRefund);
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page,pageSize);
            return View(orderDTO);
        }

        [PermissionAuthorize("Refund")]
        public bool AcceptRefund(Guid orderId)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            var result = _orderService.AgreeRefundByOrderId(employee.EmployeeId,orderId);
            
            return true;
        }
        #endregion

        #region 回复评价
        [HttpGet]
        [PermissionAuthorize("Reply")]
        public ActionResult ToReply(int page = 1,int pageSize = 10)
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ToReply);
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page,pageSize);
            return View(orderDTO);
        }

        [HttpGet]
        [PermissionAuthorize("Reply")]
        public ActionResult ReplyEvaluate(Guid orderId)
        {
            Order order = _orderService.GetOrderByOrderId(orderId);
            Comment comment = order.Comment.ElementAt(0);
            CommentViewModel commentDTO = new CommentViewModel
            {
                CommentId = comment.CommentId,
                CustomId = (int)comment.CustomId,
                CommentDetail = comment.CommentDetail,
                CommentTime = comment.CommentTime,
                GoodsId = (int)comment.GoodsId,
                OrderId = comment.OrderId,
                Reply = comment.Reply
            };
            return View(commentDTO);
        }

        [HttpPost]
        [PermissionAuthorize("Reply")]
        public ActionResult ReplyEvaluate(Guid orderId,int commentId,string replyContent)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if(employee == null)
            {
                return RedirectToAction("Index","Admin");
            }
            _orderService.ReplyByOrderId(employee.EmployeeId, orderId, replyContent,commentId);
            return RedirectToAction("AllOrders", "Order");
        }

        [HttpGet]
        public ActionResult GetBuyerInfo(Guid orderId)
        {
            Order order = _orderService.GetOrderByOrderId(orderId);
            User user = order.Custom.User;
            UserViewModel userDTO = AdminController.DataUserToDTO(user);
            return PartialView(userDTO);
        }

        [HttpGet]
        public ActionResult GetOrderInfo(Guid orderId)
        {
            Order order = _orderService.GetOrderByOrderId(orderId);
            OrderViewModel orderDTO = DataToDTO(new List<Order> { order }).ElementAt(0);

            return PartialView(orderDTO);
        }
        #endregion

        #region 退货
        [HttpGet]
        [PermissionAuthorize("Return")]
        public ActionResult ReturnConfirm(int page = 1,int pageSize = 10)
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ApplyReturn);
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page,pageSize);
            return View(orderDTO);
        }

        [PermissionAuthorize("Return")]
        public bool AcceptReturn(Guid orderId)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            _orderService.AgreeReturnByOrderId(employee.EmployeeId,orderId);
            return true;
        }

        [PermissionAuthorize("Return")]
        public bool RefuseReturn(Guid orderId)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            _orderService.RefuseReturnByOrderId(employee.EmployeeId,orderId);
            return true;
        }
        #endregion

        #region 交易完成
        [HttpGet]
        public ActionResult FinishedOrder(int page = 1, int pageSize = 10)
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.Finish);
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page, pageSize);
            return View(orderDTO);
        }
        #endregion

        public void LoginTest()
        {
            Employee employee = _enterpriseService.Login("001", "123456");
            List<Permissions> permissions = _menuViewService.GetAllPermissionsByEmployeeId(employee.EmployeeId);
            EmployeeViewModel employeeDTO = new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                UserId = employee.UserId,
                ManagePassword = employee.ManagePassword,
            };
            UserViewModel userDTO = AdminController.DataUserToDTO(employee.User);
            List<PermissionsViewModel> permissionsDTO = permissions.Select(p => new PermissionsViewModel
            {
                Id = p.PermissionId,
                Name = p.Name,
                Code = p.Code
            }).ToList();

            Session.Add("Employee", employeeDTO);
            Session.Add("User", userDTO);
            Session.Add("Permissions", permissionsDTO);
        }

        public List<OrderViewModel> DataToDTO(List<Order> orders)
        {
            if(orders == null)
            {
                return new List<OrderViewModel>();
            }
            List<OrderViewModel> orderDTO = orders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                GoodsId = o.GoodsId,
                CustomId = o.CustomId,
                GoodsName = o.GoodsName,
                Price = o.Price,
                Freight = o.Freight,
                Count = o.Count,
                Totla = o.Totla,
                Consignee = o.Consignee,
                PhoneNumber = o.PhoneNumber,
                DeliveryAddress = o.DeliveryAddress,
                State = o.State,
                CreateTime = o.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                PaymentTime = o.PaymentTime == null ? "0000-00-00 00:00:00" : o.PaymentTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                DeliveryTime = o.DeliveryTime == null ? "0000-00-00 00:00:00" : o.DeliveryTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                ReceiptTime = o.ReceiptTime == null ? "0000-00-00 00:00:00" : o.ReceiptTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                IsDelete = o.IsDelete,
                ClientRemark = o.CustomRemark == null ? "":o.CustomRemark,
                OrderRemark = o.OrderRemark == null ? "":o.OrderRemark,
            }).ToList();
            return orderDTO;
        }
        #endregion
    }
}