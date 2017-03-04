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
        private OrderService _orderService = new OrderService();

        #region 订单管理
        [HttpGet]
        public ActionResult AllOrders(int page = 1, int pageSize = 10)
        {
            List<Order> orders = _orderService.GetAllOrders();
            IPagedList<OrderViewModel> orderDTO = DataToDTO(orders).ToPagedList(page, pageSize);
            return View(orders);
        }

        #region 接受订单
        [HttpGet]
        [PermissionAuthorize("Accept")]
        public ActionResult ToAccept()
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ToAccept);
            List<OrderViewModel> orderDTO = DataToDTO(orders);
            return View(orderDTO);
        }

        public bool AcceptOrder(Guid orderId)
        {
            var result = _orderService.AcceptOrderByOrderId(orderId);
            return result;
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="marks"></param>
        /// <returns></returns>
        public bool ModifyRemark(Guid orderId, string marks)
        {
            _orderService.ModifyRemarksByOrderId(orderId, marks);
            return true;
        }
        #endregion

        #region 确认发货
        [HttpGet]
        [PermissionAuthorize("Delivery")]
        public ActionResult ToDelivery()
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ToDelivery);
            List<OrderViewModel> orderDTO = DataToDTO(orders);
            return View(orderDTO);
        }

        [HttpPost]
        public bool DeliveryOrder(Guid orderId)
        {
            _orderService.ConfirmDeliverByOrderId(orderId);
            return true;
        }
        #endregion

        #region 回复评价
        [HttpGet]
        [PermissionAuthorize("Reply")]
        public ActionResult ToReply()
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ToRefund);
            List<OrderViewModel> orderDTO = DataToDTO(orders);
            return View(orderDTO);
        }

        [HttpGet]
        [PermissionAuthorize("Reply")]
        public ActionResult ReplyEvaluate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReplyEvaluate(Guid orderId,string refund)
        {
            return View();
        }
        #endregion

        #region 确认退款
        [HttpGet]
        [PermissionAuthorize("Refund")]
        public ActionResult RefundConfirm()
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ApplyRefund);
            List<OrderViewModel> orderDTO = DataToDTO(orders);
            return View(orderDTO);
        }

        public bool AcceptRefund(Guid orderId)
        {
            var result = _orderService.AgreeRefundByOrderId(orderId);
            
            return true;
        }
        #endregion

        #region 退货
        [HttpGet]
        [PermissionAuthorize("Return")]
        public ActionResult ReturnConfirm()
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.ApplyReturn);
            List<OrderViewModel> orderDTO = DataToDTO(orders);
            return View(orderDTO);
        }

        public bool AcceptReturn(Guid orderId)
        {
            _orderService.AgreeReturnByOrderId(orderId);
            return true;
        }

        public bool RefuseReturn(Guid orderId)
        {
            _orderService.RefuseReturnByOrderId(orderId);
            return true;
        }
        #endregion

        #region 交易完成
        [HttpGet]
        public ActionResult FinishedOrder()
        {
            List<Order> orders = _orderService
                .GetOrdersByState((int)StateOfOrder.State.Finish);
            List<OrderViewModel> orderDTO = DataToDTO(orders);
            return View(orderDTO);
        }
        #endregion

        public List<OrderViewModel> DataToDTO(List<Order> orders)
        {
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
                CreateTime = o.CreateTime,
                PaymentTime = (DateTime)o.PaymentTime,
                DeliveryTime = (DateTime)o.DeliveryTime,
                IsDelete = o.IsDelete,
                ClientRemark = o.ClientRemark,
                OrderRemark = o.OrderRemark,
            }).ToList();
            return orderDTO;
        }
        #endregion
    }
}