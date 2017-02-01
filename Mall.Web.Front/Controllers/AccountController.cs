﻿using Mall.Data.DataBase;
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

        public ActionResult SecuritySet()
        {
            return View();
        }

        public ActionResult AddressSet()
        {
            return View();
        }
    }
}