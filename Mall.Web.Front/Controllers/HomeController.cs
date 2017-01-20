using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }
    }
}