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
        private EnterpriseService _enterpriseService = new EnterpriseService();
        private MenuViewService _menuViewService = new MenuViewService();

        [HttpGet]
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

        [HttpGet]
        public ActionResult ManageCenter()
        {
            return View();
        }

        public ActionResult EmployeeInfor()
        {
            //Employee employee = (Employee)Session["Employee"];
            Employee employee = _enterpriseService.Login("001", "123456");
            Session.Add("Employee", employee);
            return PartialView(employee.User);
        }
        #region 商城管理
        public ActionResult HomeSet()
        {
            return View();
        }

        public ActionResult AdvertisementSet()
        {
            return View();
        }

        public ActionResult RecommendSet()
        {
            return View();
        }

        public ActionResult ClassSet()
        {
            return View();
        }
        #endregion
        #region 账户管理
        [HttpGet]
        public ActionResult PersonalInfoSet()
        {
            Employee employee = _enterpriseService.Login("001", "123456");
            Session.Add("Employee", employee);
            return View(employee.User);
        }

        [HttpPost]
        public ActionResult PersonalInfoSet(string nickName, int gender, DateTime birthday, string phoneNumber, string email)
        {

            return RedirectToAction("PersonalInfoSet");
        }

        [HttpGet]
        public ActionResult SecuritySet()
        {
            Employee employee = (Employee)Session["Employee"];
            return View(employee.User);
        }
        [HttpPost]
        public ActionResult SecuritySet(int employeeId, string password)
        {
            return RedirectToAction("");
        }
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            List<Permissions> permissions = _menuViewService.GetAllPermissions();
            return View(permissions);
        }
        [HttpPost]
        public ActionResult CreateEmployee(string account, string nick,
                string birthday, string gender, string logPassword,
                string managePassword, string phoneNumber, string email
                ,int[] permissionIds)
        {
            //DateTime birthdayDate = DateTime.Parse(birthday);
            return RedirectToAction("");
        }
        #endregion
    }
}