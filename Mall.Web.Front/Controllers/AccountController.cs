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
        /// 客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            User user = ((Client)Session["Client"]).User;
            return PartialView(user);
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

        [HttpPost]
        public ActionResult ModifyPersonalInfo(string email,DateTime ? birthday,string nick,string name,string phone, bool gender = true)
        {
            Client client = (Client)Session["Client"];
            _clientService.ModifyUserInfo(client.ClientId, email,birthday, nick, name, phone, gender);
            TempData["ModifyInfo"] = "success";

            return RedirectToAction("PersonalInfo");
        }

        [HttpPost]
        public string ModifyPhoto(string imgBase)
        {
            Client client = (Client)Session["Client"];
            var path = _clientService.ModifyPhoto(client.ClientId, imgBase);
            return path;
        }

        #region 安全设置
        /// <summary>
        /// 安全设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SecuritySet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyLP(string log_password)
        {
            Client client = (Client)Session["Client"];
            _clientService.ModifyPasswordByClientId(client.ClientId, log_password);

            Session["Client"] = null;
            TempData["ChangeLP"] = "success";

            return RedirectToAction("../Users/Index");
        }

        [HttpPost]
        public ActionResult ModifyPP(string pay_password)
        {
            Client client = (Client)Session["Client"];
            _clientService.ModifyPayPasswordByClientId(client.ClientId, pay_password);
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
            return View();
        }

        public ActionResult AddressAlready()
        {
            Client client = (Client)Session["Client"];
            List<DeliveryInfo> delivertInfos = _clientService.GetAllDeliveryInfoByClientId(client.ClientId);
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateAddress(string address,string phone,string name,string zip = " ")
        {
            Client client = (Client)Session["Client"];
            _clientService.CreatDeliverInfo(client.ClientId,address,name,phone,zip);
            TempData["Create"] = "success";
            return RedirectToAction("AddressSet");
        }

        [HttpPost]
        public ActionResult ModifyAddress(int modify_id,string modify_name,string modify_address,string modify_phone,string zip)
        {
            return RedirectToAction("AddressSet");
        }

        [HttpDelete]
        public ActionResult DeletAddress(int addressId)
        {
            return RedirectToAction("");
        }

        #endregion
    }
}