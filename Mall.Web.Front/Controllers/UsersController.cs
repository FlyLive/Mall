using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mall.Service.DataBase;
using Mall.Service.Services;
using Mall.Service.Services.Custom;
using Mall.Web.Front.ViewModel;

namespace Mall.Web.Front.Controllers
{
    public class UsersController : Controller
    {
        public CustomService _customService = new CustomService();
        private UserService _userService = new UserService();

        /// <summary>
        /// 用户登录首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string name,string password)
        {
            if(name == null || password == null)
            {
                TempData["loginError"] = "input";
            }
            else
            {
                Custom custom = _customService.Login(name, password);
                if (custom != null)
                {
                    User user = custom.User;
                    CustomViewModel customDTO = new CustomViewModel
                    {
                        CustomId = custom.CustomId,
                        Wallet = custom.Wallet,
                        UserId = custom.UserId,
                        PayPassword = custom.PayPassword,
                    };
                    UserViewModel userDTO = CustomController.DataUserToDTO(user);

                    Session.Add("Custom", customDTO);
                    Session.Add("User", userDTO);
                    return RedirectToAction("Home","Home");
                }
                TempData["loginError"] = "password";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("Custom");
            Session.Remove("User");

            return RedirectToAction("Index", "Users");
        }

        #region 找回密码
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RetrievePW()
        {
            return View();
        }

        [HttpGet]
        public bool CustomConfirm(string account,string email)
        {
            var result = _customService.CustomConfirm(account, email);
            if (result)
            {
                TempData["Account"] = account;
                string verifyCode = _customService.SendEmailOfVerifyCode(email);
                if (!verifyCode.Equals(string.Empty))
                {
                    Session.Add("VerifyCode", verifyCode);
                    return true;
                }
            }
            return false;
        }

        [HttpGet]
        public bool VerifyCodeConfirm(string verifyCode)
        {
            string code = (string)Session["VerifyCode"];
            var result = code == verifyCode;
            return result;
        }

        [HttpPost]
        public ActionResult RetrievePW(string newPassword)
        {
            var account = (string)TempData["Account"];
            Custom custom = _customService.GetCustomByAccount(account);
            _userService.ModifyPasswordByUserId(custom.UserId, newPassword);

            TempData["RetrievePW"] = "success";
            Session.Remove("VerifyCode");

            return RedirectToAction("Index");
        }
        #endregion

        #region 注册
        /// <summary>
        /// 注册(View)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registe()
        {
            return View();
        }
        
        [HttpGet]
        public bool ReName(string account)
        {
            var result = _userService.ReName(account);
            return result;
        }

        /// <summary>
        /// 注册(Action)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fPWord"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Registe(string name,string fPWord,string email)
        {
            Custom custom = _customService.Registe(name,fPWord,email);
            if(custom == null)
            {
                TempData["Registe"] = "false";
                return RedirectToAction("Registe");
            }
            else
            {
                TempData["Registe"] = "true";
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}