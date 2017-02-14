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
                TempData["log"] = "success";
                TempData["Admin"] = employee.User.Account;
                return RedirectToAction("ManageCenter");
            }
            TempData["log"] = "password";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ManageCenter()
        {
            return View();
        }

        [HttpGet]
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
        public ActionResult ModifyInfo(string realName,
            string phone, string email, DateTime ? birthday,
            string nick = null,int gender = 1)
        {
            Employee employee = (Employee)Session["Employee"];
            _enterpriseService.ModifyInfo(employee.EmployeeId,realName,phone,email,birthday,nick,gender);
            TempData["ModifyInfo"] = "success";

            return RedirectToAction("PersonalInfoSet");
        }

        [HttpPost]
        public string ModifyPhoto(string imgBase)
        {
            Employee employee = (Employee)Session["Employee"];
            var path = _enterpriseService.ModifyPhoto(employee.EmployeeId, imgBase);
            return path;
        }

        #region 安全设置

        [HttpGet]
        public ActionResult SecuritySet()
        {
            Employee employee = (Employee)Session["Employee"];
            return View(employee.User);
        }
        
        [HttpPost]
        public ActionResult ModifyLP(string log_password)
        {
            Employee employee = (Employee)Session["Employee"];
            //_enterpriseService.ModifyLP(employee.EmployeeId,log_password);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ModifyMP(string manage_password)
        {
            Employee employee = (Employee)Session["Employee"];
            //_enterpriseService.ModifyMP(employee.EmployeeId,log_password);
        }

        [HttpGet]
        public bool ConfirmLP(string log_password)
        {
            Employee employee = (Employee)Session["Employee"];
            var result = employee.User.Password == log_password;
            return result;
        }

        [HttpGet]
        public bool ConfirmMP(string manage_password)
        {
            Employee employee = (Employee)Session["Employee"];
            var result = employee.ManagePassword == manage_password;
            return result;
        }

        #endregion

        [HttpGet]
        public bool ReAccount(string account)
        {
            var result = _enterpriseService.ReName(account);
            return result;
        }

        [HttpGet]
        public ActionResult CreateEmployee()
        {
            List<Permissions> permissions = _menuViewService.GetAllPermissions();
            return View(permissions);
        }

        public void CreateEmployee(string account, string logPassword, string phoneNumber,
                DateTime ? birthday, bool gender = true, string nick = null,
                string managePassword = null, string email = null
                ,int[] permissionIds = null)
        {
            _enterpriseService.CreateEmployee(account,logPassword,email,
                birthday,gender,nick,managePassword,phoneNumber,permissionIds);
        }
        #endregion
    }
}