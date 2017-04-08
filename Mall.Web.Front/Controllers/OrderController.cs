using Mall.Service.DataBase;
using Mall.Service.Services.Custom;
using Mall.Service.Services;
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
        private OrderService _orderService = new OrderService();
        private CustomService _customService = new CustomService();
        private GoodsService _goodsService = new GoodsService();

        public ActionResult AllOrders()
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            if (customDTO == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        [HttpGet]
        public int GetUnfinishedNumber()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            int number = _orderService.GetUnfinishedOrderByCustomId(custom.CustomId);
            return number;
        }

        [HttpGet]
        public ActionResult SearchOrderId(Guid orderId)
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            Order order = _orderService.GetOrderById(customDTO.CustomId, orderId);
            OrderViewModel orderDTO = order == null ? null : DataOrdersToDTO(new List<Order> { order }).ElementAt(0);
            return PartialView(orderDTO);
        }

        /// <summary>
        /// 返回该订单状态下的所有订单
        /// </summary>
        /// <param name="orderState"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchOrderByState(int orderState, int page = 1, int pageSize = 5)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            List<Order> orders = _orderService.GetOrdersByState(custom.CustomId, orderState);
            IPagedList<OrderViewModel> ordersDTO = DataOrdersToDTO(orders).ToPagedList(page, pageSize);
            return PartialView(ordersDTO);
        }

        [HttpPost]
        public ActionResult CreateOrder(int deliveryAddressId, int goodsId, int count, string remark = null)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            Guid orderId = _orderService.CreateOrder(goodsId, custom.CustomId, deliveryAddressId, count, remark);

            JsonResult jsonOrderId = new JsonResult();
            jsonOrderId.Data = orderId;

            return jsonOrderId;
        }

        [HttpGet]
        public ActionResult ConfirmOrder(int goodsId, int count)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            if (_customService.GetAllDeliveryInfoByCustomId(custom.CustomId).Count == 0)
            {
                TempData["NoAddress"] = "NoAddress";
                return RedirectToAction("AddressSet", "Custom");
            }
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
            ShoppingCartViewModel cartDTO = new ShoppingCartViewModel
            {
                Goods = GoodsController.DataGoodToDTO(goods),
                GoodsId = goodsId,
                Number = count,
            };
            return View(cartDTO);
        }

        /// <summary>
        /// 从购物车创建订单
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="count"></param>
        /// <param name="deliveryAddressId"></param>
        /// <param name="clientRemark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrderFromCart(int deliveryAddressId, int[] goodsId, string[] customRemark = null)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            Guid[] orderId = new Guid[goodsId.Length];
            for (int i = 0; i < goodsId.Length; i++)
            {
                customRemark[i] = customRemark[i] == null ? " " : customRemark[i];
                orderId[i] = _orderService.CreateOrderFromCart(goodsId[i], custom.CustomId, deliveryAddressId, customRemark[i]);
            }

            JsonResult jsonOrderId = new JsonResult();
            jsonOrderId.Data = orderId;

            return jsonOrderId;
        }

        /// <summary>
        /// 确认购物车商品订单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmOrderFromCart(int[] goodsId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            if (_customService.GetAllDeliveryInfoByCustomId(custom.CustomId).Count == 0)
            {
                TempData["NoAddress"] = "NoAddress";
                return RedirectToAction("AddressSet", "Custom");
            }
            List<ShoppingCart> cart = _customService.GetCartByCustomIdAndGoodsId(custom.CustomId, goodsId);
            List<ShoppingCartViewModel> cartDTO = CustomController.DataCartToDTO(cart);
            return View(cartDTO);
        }

        [HttpPost]
        public bool CancelOrder(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return false;
            }
            var result = _orderService.CancleOrderByOrderId(orderId);
            return result;
        }

        /// <summary>
        /// 支付订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool PayOrder(Guid[] orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return false;
            }
            bool result = _orderService.PayByOrderId(custom.CustomId, orderId);
            return result;
        }

        /// <summary>
        /// 退款中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RefundCenter()
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            if (customDTO == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        /// <summary>
        /// 退货中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReturnCenter()
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            if (customDTO == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderDetails(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Order order = _orderService.GetOrderById(custom.CustomId, orderId);
            OrderViewModel orderDTO = DataOrdersToDTO(new List<Order> { order }).ElementAt(0);
            return View(orderDTO);
        }

        public static List<OrderViewModel> DataOrdersToDTO(List<Order> orders)
        {
            if (orders == null)
            {
                return new List<OrderViewModel>();
            }
            List<OrderViewModel> orderDTO = orders.Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                GoodsId = order.GoodsId,
                CustomId = order.CustomId,
                GoodsName = order.GoodsName,
                PhotoUrl = order.GoodsInfo.Image.ElementAt(0).ImageSrc,
                Price = order.Price,
                Freight = order.Freight,
                Count = order.Count,
                Totla = order.Totla,
                Consignee = order.Consignee,
                PhoneNumber = order.PhoneNumber,
                DeliveryAddress = order.DeliveryAddress,
                State = order.State,
                CreateTime = order.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                PaymentTime = order.PaymentTime == null ? "0000-00-00 00:00:00" : order.PaymentTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                DeliveryTime = order.DeliveryTime == null ? "0000-00-00 00:00:00" : order.DeliveryTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                ReceiptTime = order.ReceiptTime == null ? "0000/00/00 00:00:00" : order.ReceiptTime.Value.ToString("yyyy/MM/dd HH:mm:ss"),
                IsDelete = order.IsDelete,
                ClientRemark = order.CustomRemark == null ? "" : order.CustomRemark,
                OrderRemark = order.OrderRemark == null ? "" : order.OrderRemark,
            }).ToList();
            return orderDTO;
        }
    }
}