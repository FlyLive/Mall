using Mall.Service.DataBase;
using Mall.Service.Services.Client;
using Mall.Web.Front.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class OrderController : Controller
    {
        public OrderService _orderService = new OrderService();
        public CustomService _customService = new CustomService();

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="count"></param>
        /// <param name="deliveryAddressId"></param>
        /// <param name="clientRemark"></param>
        /// <returns></returns>
        public ActionResult CreateOrder(int goodsId, int count, int deliveryAddressId, string clientRemark)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            else
            {
                var orderId = _orderService.CreateOrder(goodsId, custom.CustomId, deliveryAddressId, count, clientRemark);
                return RedirectToAction("BuyNow",orderId);
            }
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BuyNow(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        /// <summary>
        /// 支付订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public bool PayOrder(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return false;
            }
            bool result = _orderService.PayByOrderId(orderId);
            return result;
        }

        public ActionResult AllOrders()
        {
            Custom custom = _customService.Login("Blank", "654321");
            CustomViewModel customDTO = new CustomViewModel
            {
                CustomId = custom.CustomId,
                UserId = custom.UserId,
                Wallet = custom.Wallet,
                PayPassword = custom.PayPassword,
            };
            Session.Add("Custom", customDTO);
            customDTO = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            List<Order> orders = _orderService.GetAllOrderByClientId(custom.CustomId);
            IPagedList<OrderViewModel> ordersList = orders.Select(o => new OrderViewModel
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
            }).ToPagedList(1, 5);
            return View(ordersList);
        }

        [HttpGet]
        public ActionResult SearchOrderId(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Order order = _orderService.GetOrderById(custom.CustomId, orderId);
            OrderViewModel orderDTO = new OrderViewModel
            {
                OrderId = order.OrderId,
                GoodsId = order.GoodsId,
                CustomId = order.CustomId,
                GoodsName = order.GoodsName,
                Price = order.Price,
                Freight = order.Freight,
                Count = order.Count,
                Totla = order.Totla,
                Consignee = order.Consignee,
                PhoneNumber = order.PhoneNumber,
                DeliveryAddress = order.DeliveryAddress,
                State = order.State,
                CreateTime = order.CreateTime,
                PaymentTime = (DateTime)order.PaymentTime,
                DeliveryTime = (DateTime)order.DeliveryTime,
                IsDelete = order.IsDelete,
                ClientRemark = order.ClientRemark,
                OrderRemark = order.OrderRemark,
            };
            return PartialView(orderDTO);
        }

        [HttpGet]
        public ActionResult SearchOrderByState(int orderState)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            List<Order> orders = _orderService.GetOrdersByState(custom.CustomId, orderState);
            return PartialView(orders);
        }
    }
}