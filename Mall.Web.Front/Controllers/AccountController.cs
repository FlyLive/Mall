using Mall.Data.DataBase;
using Mall.Data.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class AccountController : Controller
    {
        ClientService _clientService = new ClientService();
        
        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Client client = _clientService.Login("Blank","123456");
            Session.Add("Client",client);
            return View(client.User);
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInfo()
        {
            Client client = _clientService.Login("Blank","123456");
            Session.Add("Client",client);
            client = (Client)Session["Client"];
            if (client != null)
            {
                return View(client.User);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 安全设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SecuritySet()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeLP(string log_password)
        {
            Client client = (Client)Session["Client"];
            _clientService.ModifyPasswordByClientId(client.ClientId, log_password);

            Session["Client"] = null;
            TempData["ChangeLP"] = "success";

            return RedirectToAction("../Users/Index");
        }
        /// <summary>
        /// 收货地址管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AddressSet()
        {
            return View();
        }
    }
}