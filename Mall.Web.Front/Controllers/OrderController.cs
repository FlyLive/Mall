using Mall.Data.Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class OrderController : Controller
    {
        public OrderService _orderService = new OrderService();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
    }
}