using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registe()
        {
            return View();
        }
        public ActionResult BuyNow()
        {
            return View();
        }
    }
}