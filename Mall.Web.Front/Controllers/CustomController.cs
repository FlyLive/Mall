using Mall.Service.DataBase;
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
        CustomService _customService = new CustomService();

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
            LoginTest();
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
            _customService.ModifyUserInfo(custom.CustomId, email, birthday, nick, name, phone, gender);
            TempData["ModifyInfo"] = "success";

            return RedirectToAction("PersonalInfo");
        }

        [HttpPost]
        public string ModifyPhoto(string imgBase)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            var path = _customService.ModifyPhoto(custom.CustomId, imgBase);
            return path;
        }
        #endregion

        #region 购物车
        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult ShoppingCart()
        {
            LoginTest();
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom != null)
            {
                List<GoodsInfo> cartGoods = new List<GoodsInfo>();
                List<ShoppingCartViewModel> cartsDTO = new List<ShoppingCartViewModel>();
                return View(cartsDTO);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public bool ModifyShoppingCart(int goodsId,int count)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            _customService.ModifyGoodsCountFromShoppingCart(custom.CustomId, goodsId, count);
            return true;
        }

        [HttpGet]
        public ActionResult SearchGoodFromCart(string searchName)
        {
            CustomViewModel customDTO = (CustomViewModel)Session["Custom"];
            List<ShoppingCartViewModel> cartsDTO = new List<ShoppingCartViewModel>();
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
        public bool ConfirmMP(string pay_password)
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
            _customService.ModifyPasswordByCustomId(custom.CustomId, log_password);

            Session["Custom"] = null;
            Session["User"] = null;
            TempData["ChangeLP"] = "success";

            return RedirectToAction("Index", "Users");
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
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            List<DeliveryInfoViewModel> delivertInfosDTO = _customService
                .GetAllDeliveryInfoByCustomId(custom.CustomId)
                .Select(d => new DeliveryInfoViewModel
                {
                    CustomId = d.CustomId,
                    Id = d.Id,
                    IsDefault = d.IsDefault,
                    Consignee = d.Consignee,
                    DetailedAddress = d.DetailedAddress,
                    PhoneNumber = d.PhoneNumber
                }).ToList();
            return PartialView();
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
                    PhoneNumber = d.PhoneNumber
                }).ToList();
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
            _customService.CreatDeliverInfo(custom.CustomId, address, name, phone, zip);
            TempData["Create"] = "success";
            return RedirectToAction("AddressSet");
        }

        [HttpPost]
        public ActionResult ModifyAddress(int modify_id, string modify_name, string modify_address, string modify_phone, string zip)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("AddressSet");
        }

        [HttpDelete]
        public ActionResult DeletAddress(int addressId)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return RedirectToAction("");
        }

        #endregion

        #region 购物
        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        public ActionResult BuyNow(Guid orderId)
        {
            Custom custom = (Custom)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfirmReceipt()
        {
            Custom custom = (Custom)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <returns></returns>
        public ActionResult Evaluate()
        {
            Custom custom = (Custom)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }
        #endregion

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

        public static UserViewModel DataUserToDTO(User user)
        {
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Password = user.Password,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return userDTO;
        }
    }
}