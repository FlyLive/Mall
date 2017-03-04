using Mall.Service.DataBase;
using Mall.Service.Services.Custom;
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

        public ActionResult AllOrders()
        {
            LoginTest();//
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            if (customDTO == null)
            {
                return RedirectToAction("Index", "Users");
            }
            List<Order> orders = _orderService.GetAllOrderByClientId(customDTO.CustomId);
            IPagedList<OrderViewModel> ordersList = DataOrdersToDTO(orders).ToPagedList(1, 5);
            return View(ordersList);
        }

        [HttpGet]
        public ActionResult SearchOrderId(string orderId)
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            Order order = _orderService.GetOrderById(customDTO.CustomId, new Guid(orderId));
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
                CreateTime = order.CreateTime.ToString("yyyy-MM-dd HH-mm-ss"),
                PaymentTime = order.PaymentTime == null ? order.PaymentTime.ToString() : "0000-00-00 00-00-00",
                DeliveryTime = order.DeliveryTime == null ? order.DeliveryTime.ToString() : "0000-00-00 00-00-00",
                IsDelete = order.IsDelete,
                ClientRemark = order.ClientRemark,
                OrderRemark = order.OrderRemark,
            };
            return PartialView(orderDTO);
        }

        /// <summary>
        /// 返回该订单状态下的所有订单
        /// </summary>
        /// <param name="orderState"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchOrderByState(int orderState)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            List<Order> orders = _orderService.GetOrdersByState(custom.CustomId, orderState);
            List<OrderViewModel> ordersDTO = DataOrdersToDTO(orders);
            return PartialView(ordersDTO);
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="count"></param>
        /// <param name="deliveryAddressId"></param>
        /// <param name="clientRemark"></param>
        /// <returns></returns>
        public ActionResult CreateOrder(int deliveryAddressId, int[] goodsId, int[] count,string[] clientRemark)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            else
            {
                var orderId = _orderService.CreateOrder(goodsId, custom.CustomId, deliveryAddressId, count, clientRemark);
                return RedirectToAction("BuyNow", orderId);
            }
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmOrder(int goodsId)
        {
            LoginTest();
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            ShoppingCart cart = _customService.GetCartByCustomIdAndGoodsId(custom.CustomId, goodsId);
            List<ShoppingCartViewModel> cartDTO = new List<ShoppingCartViewModel>();
            return View(cartDTO);
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

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetails()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        public void LoginTest()
        {
            Custom custom = _customService.Login("Blank", "654321");
            CustomViewModel customDTO = new CustomViewModel
            {
                CustomId = custom.CustomId,
                UserId = custom.UserId,
                Wallet = custom.Wallet,
                PayPassword = custom.PayPassword,
                MaxAddressNumber = custom.MaxAddressNumber,
            };
            UserViewModel userDTO = CustomController.DataUserToDTO(custom.User);
            Session.Add("Custom", customDTO);
            Session.Add("User", userDTO);
        }

        public static List<OrderViewModel> DataOrdersToDTO(List<Order> orders)
        {
            List<OrderViewModel> orderDTO = orders.Select(order => new OrderViewModel
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
                CreateTime = order.CreateTime.ToString("yyyy-MM-dd HH-mm-ss"),
                PaymentTime = order.PaymentTime == null ? order.PaymentTime.ToString() : "0000-00-00 00-00-00",
                DeliveryTime = order.DeliveryTime == null ? order.DeliveryTime.ToString() : "0000-00-00 00-00-00",
                IsDelete = order.IsDelete,
                ClientRemark = order.ClientRemark,
                OrderRemark = order.OrderRemark,
            }).ToList();
            return orderDTO;
        }
    }
}