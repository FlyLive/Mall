using Mall.Service.DataBase;
using Mall.Service.Services;
using Mall.Service.Services.Enterprise;
using Mall.Web.Back.Filter;
using Mall.Web.Back.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class AdminController : ChatController
    {
        private EnterpriseService _enterpriseService = new EnterpriseService();
        private MenuViewService _menuViewService = new MenuViewService();
        private UserService _userService = new UserService();

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
                EmployeeViewModel employeeDTO = new EmployeeViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    UserId = employee.UserId,
                    ManagePassword = employee.ManagePassword
                };
                UserViewModel userDTO = DataUserToDTO(employee.User);
                List<PermissionsViewModel> permissions = _menuViewService
                    .GetAllPermissionsByEmployeeId(employee.EmployeeId).Select(p => new PermissionsViewModel
                    {
                        Id = p.PermissionId,
                        Name = p.Name,
                        Code = p.Code
                    }).ToList();
                Session.Add("Employee", employeeDTO);
                Session.Add("User", userDTO);
                Session.Add("Permissions", permissions);
                TempData["log"] = "success";
                TempData["Admin"] = employee.User.Account;
                return RedirectToAction("ManageCenter","Admin");
            }
            TempData["log"] = "password";
            return RedirectToAction("Index","Admin");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("Employee");
            Session.Remove("User");
            Session.Remove("Permissions");

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult ManageCenter()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EmployeeInfor()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            User user = _enterpriseService.GetUserByEmployeeId(employee.EmployeeId);
            UserViewModel userDTO = DataUserToDTO(user);
            return PartialView(userDTO);
        }        

        #region 用户中心

        #region 个人资料
        [HttpGet]
        [PermissionAuthorize("PersonalInfo")]
        public ActionResult PersonalInfoSet()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            User user = _enterpriseService.GetUserByEmployeeId(employee.UserId);
            UserViewModel userDTO = DataUserToDTO(user);
            return View(userDTO);
        }

        [HttpPost]
        [PermissionAuthorize("PersonalInfo")]
        public ActionResult ModifyInfo(string realName,
            string phone, string email, DateTime? birthday,
            string nick = null, int gender = 1)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            var result = _userService.ModifyInfo(employee.UserId, realName, phone, email, birthday, nick, gender);
            if (result)
            {
                TempData["ModifyInfo"] = "success";
            }
            return RedirectToAction("PersonalInfoSet");
        }

        [HttpPost]
        [PermissionAuthorize("PersonalInfo")]
        public bool ModifyPhoto(string imgBase)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            var result = _userService.ModifyPhoto(employee.UserId, imgBase);
            if (result != null)
            {
                ((UserViewModel)Session["User"]).Photo = result;
                return true;
            }
            return false;
        }
        #endregion

        #region 安全设置

        [HttpGet]
        [PermissionAuthorize("Security")]
        public ActionResult SecuritySet()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            User user = _enterpriseService.GetUserByEmployeeId(employee.EmployeeId);
            UserViewModel userDTO = DataUserToDTO(user);
            return View(userDTO);
        }

        [HttpPost]
        [PermissionAuthorize("Security")]
        public ActionResult ModifyLP(string log_password)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            var reuslt = _userService.ModifyPasswordByUserId(employee.UserId, log_password);
            return RedirectToAction("Logout","Admin");
        }

        [HttpPost]
        [PermissionAuthorize("Security")]
        public bool ModifyMP(string manage_password)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            _enterpriseService.ModifyManagePassword(employee.EmployeeId, manage_password);
            employee.ManagePassword = manage_password;
            Session["Employee"] = employee;

            return true;
        }

        /// <summary>
        /// 确认登录密码
        /// </summary>
        /// <param name="log_password"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ConfirmLP(string log_password)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            User user = _enterpriseService.GetUserByEmployeeId(employee.EmployeeId);
            var result = user.Password == log_password;
            return result;
        }

        /// <summary>
        /// 确认管理密码
        /// </summary>
        /// <param name="manage_password"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ConfirmMP(string manage_password)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            var result = employee.ManagePassword == manage_password;
            return result;
        }

        #endregion

        public static UserViewModel DataUserToDTO(User user)
        {
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = user.Gender == null ? true : (bool)user.Gender,
                Birthday = user.Birthday == null ? "0000-00-00" : user.Birthday.Value.ToString("yyyy-MM-dd"),
                CreateTime = user.CreateTime == null ? "0000-00-00 00-00-00" : user.CreateTime.ToString(),
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return userDTO;
        }
        #endregion
    }
}