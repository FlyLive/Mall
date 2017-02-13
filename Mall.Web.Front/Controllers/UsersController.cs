using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mall.Data.DataBase;
using Mall.Data.Services.Client;

namespace Mall.Web.Front.Controllers
{
    public class UsersController : Controller
    {
        public ClientService _userService = new ClientService();
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
                Client client = _userService.Login(name, password);
                if(client != null)
                {
                    Session.Add("Client", client);
                    return RedirectToAction("../Home/Home");
                }
                TempData["loginError"] = "password";
            }
            return RedirectToAction("Index");
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
        public bool ClientConfirm(string account,string email)
        {
            var result = _userService.ClientConfirm(account, email);
            if (result)
            {
                TempData["Account"] = account;
            }
            return result;
        }
        [HttpPost]
        public ActionResult RetrievePW(string newPassword)
        {
            var account = (string)TempData["Account"];
            Client client = _userService.GetClientByAccount(account);
            _userService.ModifyPasswordByClientId(client.ClientId, newPassword);
            TempData["RetrievePW"] = "successs";
            return RedirectToAction("Index");
        }
        #endregion

        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registe(string name,string fPWord,string email)
        {
            Client client = _userService.Registe(name,fPWord,email);
            if(client == null)
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

        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        public ActionResult BuyNow()
        {
            return View();
        }

        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult ShoppingCart()
        {
            Client client = (Client)Session["Client"];
            if(client != null)
            {
                List<GoodsInfo> cartGoods = new List<GoodsInfo>();
                return View(cartGoods);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 订单
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetails()
        {
            return View();
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfirmReceipt()
        {
            return View();
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <returns></returns>
        public ActionResult Evaluate()
        {
            return View();
        }
    }
}