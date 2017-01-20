using Mall.Data.DataBase;
using Mall.Data.Services;
using Mall.Data.Services.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private EnterpriseService _enterpriseService = new EnterpriseService();
        private MenuViewService _ACService = new MenuViewService();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login(string account, string password)
        {
            Employee employee = _enterpriseService.Login(account, password);
            if (employee != null)
            {
                Session.Add("Employee", employee);
                return RedirectToAction("ManageCenter");
            }
            return RedirectToAction("Index");
        }
        public ActionResult ManageCenter()
        {
            return View();
        }
        public ActionResult EmployeeInfor()
        {
            Employee employee = (Employee)Session["Employee"];
            return PartialView(employee.User);
        }
    }
}