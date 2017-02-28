using Mall.Service.DataBase;
using Mall.Service.Services.Client;
using Mall.Web.Front.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class AccountController : Controller
    {
        CustomService _customService = new CustomService();

        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index","Users");
            }
            User user = _customService.GetUserByCustomId(custom.CustomId);
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return View(userDTO);
        }

        /// <summary>
        /// 客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            CustomViewModel client = (CustomViewModel)Session["Client"];
            if (client == null)
            {
                return RedirectToAction("Index", "Users");
            }
            User user = _customService.GetUserByCustomId(client.CustomId);
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return PartialView(user);
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInfo()
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
            if (customDTO == null)
            {
                return RedirectToAction("Index", "Users");
            }
            User user = _customService.GetUserByCustomId(custom.CustomId);
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return View(userDTO);
        }

        [HttpPost]
        public ActionResult ModifyPersonalInfo(string email, DateTime? birthday, string nick, string name, string phone, int gender = 1)
        {
            Custom custom = (Custom)Session["Custom"];
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
            Custom custom = (Custom)Session["Custom"];
            var path = _customService.ModifyPhoto(custom.CustomId, imgBase);
            return path;
        }

        #region 安全设置
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SecuritySet()
        {
            Custom custom = (Custom)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
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
            TempData["ChangeLP"] = "success";

            return RedirectToAction("Index", "Users");
        }

        [HttpPost]
        public ActionResult ModifyPP(string pay_password)
        {
            CustomViewModel custom = (CustomViewModel)Session["Custom"];
            if (custom == null)
            {
                return RedirectToAction("Index", "Users");
            }
            _customService.ModifyPayPasswordByCustomId(custom.CustomId, pay_password);
            TempData["ChangePP"] = "success";
            return RedirectToAction("SecuritySet");
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
                    Consignee = d.Consignee,
                    DetailedAddress = d.DetailedAddress,
                    PhoneNumber = d.PhoneNumber
                }).ToList();
            return PartialView();
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
    }
}