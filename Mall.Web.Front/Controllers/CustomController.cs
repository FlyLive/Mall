using Mall.Service.DataBase;
using Mall.Service.Services;
using Mall.Service.Services.Custom;
using Mall.Web.Front.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class CustomController : Controller
    {
        private CustomService _customService = new CustomService();
        private OrderService _orderService = new OrderService();
        private UserService _userService = new UserService();

        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            UserViewModel user = (UserViewModel)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(user);
        }

        [HttpGet]
        public double Wallet()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return new int();
            }
            var wallet = _customService.GetWallet(custom.CustomId);
            return wallet;
        }

        /// <summary>
        /// 客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            UserViewModel user = (UserViewModel)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return PartialView(user);
        }

        #region 个人资料
        /// <summary>
        /// 个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInfo()
        {
            UserViewModel user = (UserViewModel)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult ModifyPersonalInfo(string email, DateTime? birthday, string nick, string name, string phone, int gender = 1)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            var result = _userService.ModifyInfo(custom.UserId, name, phone, email, birthday, nick, gender);
            if (result)
            {
                TempData["ModifyInfo"] = "success";
                User user = _customService.GetUserByCustomId(custom.CustomId);
                UserViewModel userVM = DataUserToDTO(user);
                Session["User"] = userVM;
            }
            return RedirectToAction("PersonalInfo");
        }

        [HttpPost]
        public bool ModifyPhoto(string imgBase)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            var path = _userService.ModifyPhoto(custom.UserId, imgBase);
            if (path != null)
            {
                ((UserViewModel)Session["User"]).Photo = path;
                return true;
            }
            return false;
        }
        #endregion

        #region 购物车
        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult ShoppingCart()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom != null)
            {
                List<ShoppingCart> carts = _customService.GetCartByCustomId(custom.CustomId);
                List<ShoppingCartViewModel> cartsDTO = DataCartToDTO(carts);
                return View(cartsDTO);
            }
            return RedirectToAction("Index", "Users");
        }

        [HttpGet]
        public int GetCartNumber()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            int cartNumber = _customService.GetCartNumberByCustomId(custom.CustomId);
            return cartNumber;
        }

        [HttpPost]
        public double GetSelectedMoney(int[] goodIds)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            double totlaMoney = 0;
            if (goodIds.Length != 0 && custom != null)
            {
                totlaMoney = _customService.GetTotlaMoneyById(custom.CustomId, goodIds);
            }
            return totlaMoney;
        }

        [HttpPost]
        public bool CreateShoppingCart(int goodsId, int count)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return false;
            }
            _customService.AddGoodsToShoppingCart(custom.CustomId, goodsId, count);
            return true;
        }

        [HttpGet]
        public bool ModifyShoppingCart(int goodsId, int count)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            _customService.ModifyGoodsCountFromShoppingCart(custom.CustomId, goodsId, count);
            return true;
        }

        [HttpGet]
        public ActionResult SearchGoodFromCart(string searchName = "")
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            List<ShoppingCart> carts = _customService.GetCartByCustomId(customDTO.CustomId);
            List<ShoppingCartViewModel> cartsDTO = DataCartToDTO(carts).Where(c => c.Goods.GoodsName.Contains(searchName)).ToList();
            return PartialView(cartsDTO);
        }

        [HttpPost]
        public bool DeletGoodsFromCart(int goodsId)
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            _customService.DeleteGoodsFromShoppingCart(customDTO.CustomId, goodsId);
            return true;
        }

        #endregion

        #region 安全设置
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SecuritySet()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        /// <summary>
        /// 确认登录密码
        /// </summary>
        /// <param name="log_password"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ConfirmLP(string log_password)
        {
            UserViewModel user = (UserViewModel)Session["User"];
            var result = user.Password == log_password;
            return result;
        }

        /// <summary>
        /// 确认支付密码
        /// </summary>
        /// <param name="pay_password"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ConfirmPP(string pay_password)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            var result = custom.PayPassword == pay_password;
            return result;
        }

        [HttpPost]
        public ActionResult ModifyLP(string log_password)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            var result = _userService.ModifyPasswordByUserId(custom.UserId, log_password);

            if (result)
            {
                Session["Custom"] = null;
                Session["User"] = null;
                TempData["ChangeLP"] = "success";
                return RedirectToAction("Index", "Users");
            }
            return new EmptyResult();
        }

        [HttpPost]
        public bool ModifyPP(string pay_password)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return false;
            }
            _customService.ModifyPayPasswordByCustomId(custom.CustomId, pay_password);
            return true;
        }
        #endregion

        #region 收货地址管理
        /// <summary>
        /// 收货地址管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AddressSet()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddressAlready()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            List<DeliveryInfoViewModel> delivertInfosDTO = _customService
                .GetAllDeliveryInfoByCustomId(custom.CustomId)
                .Select(d => new DeliveryInfoViewModel
                {
                    CustomId = d.CustomId,
                    Id = d.Id,
                    IsDefault = d.IsDefault,
                    Consignee = d.Consignee,
                    DetailedAddress = d.DetailedAddress,
                    PhoneNumber = d.PhoneNumber,
                    Zip = d.Zip == null ? "未设置" : d.Zip,
                }).ToList();
            return PartialView(delivertInfosDTO);
        }

        [HttpGet]
        public ActionResult GetAddress()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            List<DeliveryInfoViewModel> delivertInfosDTO = _customService
                .GetAllDeliveryInfoByCustomId(custom.CustomId)
                .Select(d => new DeliveryInfoViewModel
                {
                    CustomId = d.CustomId,
                    Id = d.Id,
                    IsDefault = d.IsDefault,
                    Consignee = d.Consignee,
                    DetailedAddress = d.DetailedAddress,
                    PhoneNumber = d.PhoneNumber,
                    Zip = d.Zip == null ? "未设置" : d.Zip,
                }).ToList();
            return PartialView(delivertInfosDTO);
        }

        [HttpGet]
        public ActionResult GetSelectedAddress(int? addressId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            DeliveryInfo delivery = _customService.GetDeliveryInfoById(custom.CustomId, addressId);
            DeliveryInfoViewModel delivertInfosDTO = new DeliveryInfoViewModel
            {
                CustomId = delivery.CustomId,
                Id = delivery.Id,
                IsDefault = delivery.IsDefault,
                Consignee = delivery.Consignee,
                DetailedAddress = delivery.DetailedAddress,
                PhoneNumber = delivery.PhoneNumber,
                Zip = delivery.Zip == null ? "未设置" : delivery.Zip,
            };
            return PartialView(delivertInfosDTO);
        }

        [HttpPost]
        public ActionResult CreateAddress(string address, string phone, string name, string zip = " ")
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            var result = _customService.CreatDeliverInfo(custom.CustomId, address, name, phone, zip);
            if (result)
            {
                TempData["Create"] = "success";
            }
            return RedirectToAction("AddressSet");
        }

        [HttpGet]
        public bool IsAddressFull()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            var result = _customService.IsDeliveryFull(custom.CustomId);
            return result;
        }

        [HttpPost]
        public ActionResult ModifyAddress(int modify_id, string modify_name = "", string modify_address = "", string modify_phone = "", string modify_zip = "")
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            _customService.ModifyDeliverInfo(custom.CustomId, modify_id, modify_address, modify_name, modify_phone, modify_zip);
            return RedirectToAction("AddressSet");
        }

        [HttpPost]
        public bool DeletAddress(int addressId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            _customService.DeletDeliveryByDeliveryId(custom.CustomId, addressId);
            return true;
        }

        [HttpPost]
        public bool SetDefaultAddress(int addressId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            _customService.SetDefaultAddressOfCustomByDeliveryId(custom.CustomId, addressId);
            return true;
        }

        #endregion

        #region 购物
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfirmReceipt(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Order order = _orderService.GetOrderById(custom.CustomId, orderId);
            OrderViewModel orderDTO = OrderController.DataOrdersToDTO(new List<Order> { order }).ElementAt(0);
            return View(orderDTO);
        }

        [HttpPost]
        public bool ReceiptOrder(Guid orderId)
        {
            var result = _orderService.ConfirmReceipt(orderId);
            return result;
        }

        /// <summary>
        /// 申请退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ApplyRefund(Guid orderId)
        {
            var result = _orderService.ApplyRefundByOrderId(orderId);
            return result;
        }

        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ApplReturn(Guid orderId)
        {
            var result = _orderService.ApplyReturnByOrderId(orderId);
            return result;
        }

        /// <summary>
        /// 评价(View)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Evaluate(Guid orderId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            Order order = _orderService.GetOrderById(custom.CustomId, orderId);
            OrderViewModel orderDTO = OrderController.DataOrdersToDTO(new List<Order> { order }).ElementAt(0);
            return View(orderDTO);
        }

        /// <summary>
        /// 评价(Action)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="evaluateContent"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Evaluate(Guid orderId, string evaluateContent = null)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            var result = _orderService.EvaluateOrder(custom.CustomId, orderId, evaluateContent);
            if (result)
            {
                return RedirectToAction("AllOrders", "Order");
            }
            else
            {
                return new EmptyResult();
            }
        }
        #endregion

        public static List<ShoppingCartViewModel> DataCartToDTO(List<ShoppingCart> carts)
        {
            List<ShoppingCartViewModel> cartsDTO = carts.Select(c => new ShoppingCartViewModel
            {
                ShoppingCartId = c.ShoppingCartId,
                CustomId = c.CustomId,
                CreateTime = c.CreateTime,
                Number = c.Number,
                GoodsId = c.GoodsId,
                Goods = GoodsController.DataGoodToDTO(c.GoodsInfo),
            }).ToList();
            return cartsDTO;
        }

        public static UserViewModel DataUserToDTO(User user)
        {
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Password = user.Password,
                Gender = user.Gender == null ? true : (bool)user.Gender,
                Birthday = user.Birthday == null ? "0000-00-00" : user.Birthday.Value.ToString("yyyy-MM-dd"),
                CreateTime = user.CreateTime == null ? "0000-00-00" : user.CreateTime.ToString(),
                PhoneNumber = user.PhoneNumber == null ? "未设置" : user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return userDTO;
        }
    }
}