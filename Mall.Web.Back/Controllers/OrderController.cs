using Mall.Service.DataBase;
using Mall.Service.Services.Enterprise;
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
            IPagedList<OrderViewModel> orders = _orderService
                .GetAllOrders().Select(o => new OrderViewModel
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
                }).ToPagedList(page, pageSize);
            return View(orders);
        }

        public bool AcceptOrder(Guid orderId)
        {
            _orderService.AcceptOrderByOrderId(orderId);
            return true;
        }

        public bool ModifyRemark(Guid orderId, string marks)
        {
            _orderService.ModifyRemarksByOrderId(orderId, marks);
            return true;
        }

        public bool ConfirmDeliver(Guid orderId)
        {
            _orderService.ConfirmDeliverByOrderId(orderId);
            return true;
        }

        
        public ActionResult ReplyEvaluate()
        {
            return View();
        }
        #endregion
    }
}