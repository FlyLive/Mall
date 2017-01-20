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
            Client client = _userService.Login(name, password);
            if(client != null)
            {
                Session.Add("Client", client);
                return RedirectToAction("../Home/Home");
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
        [HttpPost]
        public ActionResult RetrievePW(int id,string newPassword)
        {
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
        public ActionResult Registe(string name,string fPWord,string sPWord,string email)
        {
            if(fPWord == sPWord)
            {
                Client client = _userService.Registe(name,fPWord,email);
            }
            return RedirectToAction("../Home/Home");
        }
        #endregion
        public ActionResult BuyNow()
        {
            return View();
        }
        public ActionResult ShoppingCart()
        {
            List<GoodsInfo> cartGoods = new List<GoodsInfo>();
            return View(cartGoods);
        }
        public ActionResult OrderDetails()
        {
            return View();
        }

        public ActionResult ConfirmReceipt()
        {
            return View();
        }

        public ActionResult Evaluate()
        {
            return View();
        }
    }
}