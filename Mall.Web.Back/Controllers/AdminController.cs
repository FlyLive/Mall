using Mall.Service.DataBase;
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
                };
                List<PermissionsViewModel> permissions = _menuViewService
                    .GetAllPermissionsByEmployeeId(employee.EmployeeId).Select(p => new PermissionsViewModel
                    {

                    }).ToList();
                Session.Add("Employee", employeeDTO);
                Session.Add("Permissions", permissions);
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
            //User user = _enterpriseService.GetUserByEmployeeId(employee.UserId);
            //UserViewModel userDTO = new UserViewModel
            //{
            //    UserId = user.UserId,
            //    Account = user.Account,
            //    NickName = user.NickName,
            //    Gender = user.Gender,
            //    Birthday = user.Birthday,
            //    CreateTime = user.CreateTime,
            //    PhoneNumber = user.Phone,
            //    Email = user.Email,
            //    Photo = user.Photo,
            //    RealName = user.RealName
            //};
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

        #region 用户中心
        [HttpGet]
        public ActionResult EmployeeDetails()
        {
            int employeeId = 1;
            User user = _enterpriseService.GetUserByEmployeeId(employeeId);
            UserViewModel employeeDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return View(employeeDTO);
        }

        [HttpGet]
        public ActionResult EmployeeActions(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            List<AdminLog> adminLogs = _enterpriseService.GetAdminLogsByEmployeeId(employee.EmployeeId);
            List<AdminLogViewModel> adminLogsDTO = new List<AdminLogViewModel>();
            foreach (var adminlog in adminLogs)
            {
                adminLogsDTO.Add(new AdminLogViewModel
                {
                    RecordId = adminlog.RecordId,
                    EmployeeId = adminlog.EmployeeId,
                    OperationTime = adminlog.OperationTime,
                    OperatDetail = adminlog.OperatDetail,
                    Operater = adminlog.Operater,
                    Object = adminlog.Object,
                    Permission = adminlog.Permission,
                    Style = adminlog.Style
                });
            };
            return PartialView(adminLogs);
        }

        [HttpGet]
        public ActionResult PersonalInfoSet()
        {
            //Employee employee = (Employee)Session["Employee"];
            //User user = _enterpriseService.GetUserByEmployeeId(employee.UserId);
            //UserViewModel userDTO = new UserViewModel
            //{
            //    UserId = user.UserId,
            //    Account = user.Account,
            //    NickName = user.NickName,
            //    Gender = (bool)user.Gender,
            //    Birthday = (DateTime)user.Birthday,
            //    CreateTime = user.CreateTime,
            //    PhoneNumber = user.PhoneNumber,
            //    Email = user.Email,
            //    Photo = user.Photo,
            //    RealName = user.RealName
            //};
            Employee employee = _enterpriseService.Login("001", "123456");
            Session.Add("Employee", employee);
            return View(employee.User);
        }

        [HttpPost]
        public ActionResult ModifyInfo(string realName,
            string phone, string email, DateTime? birthday,
            string nick = null, int gender = 1)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            _enterpriseService.ModifyInfo(employee.EmployeeId, realName, phone, email, birthday, nick, gender);
            TempData["ModifyInfo"] = "success";

            return RedirectToAction("PersonalInfoSet");
        }

        [HttpPost]
        public bool ModifyPhoto(string imgBase)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            _enterpriseService.ModifyPhoto(employee.EmployeeId, imgBase);
            return true;
        }

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
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return View(userDTO);
        }

        [HttpPost]
        public ActionResult ModifyLP(string log_password)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            _enterpriseService.ModifyLogPassword(employee.EmployeeId, log_password);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public bool ModifyMP(string manage_password)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            _enterpriseService.ModifyManagePassword(employee.EmployeeId, manage_password);
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
            //待完善
            var result = employee.ManagePassword == manage_password;
            return result;
        }

        #endregion

        /// <summary>
        /// 重名验证
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet]
        public bool ReAccount(string account)
        {
            var result = _enterpriseService.ReName(account);
            return result;
        }

        /// <summary>
        /// 创建员工(视图)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            List<Permissions> permissions = _menuViewService.GetAllPermissions();
            List<UserViewModel> permissionsDTO = new List<UserViewModel>();
            foreach (var permission in permissions)
            {
                permissionsDTO.Add(new UserViewModel
                {
                    //待完善
                });
            }
            return View(permissions);
        }

        /// <summary>
        /// 创建员工(Action)
        /// </summary>
        /// <param name="account"></param>
        /// <param name="logPassword"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthday"></param>
        /// <param name="gender"></param>
        /// <param name="nick"></param>
        /// <param name="managePassword"></param>
        /// <param name="email"></param>
        /// <param name="permissionIds"></param>
        public bool CreateEmployee(string account, string logPassword, string phoneNumber,
                DateTime? birthday, bool gender = true, string nick = null,
                string managePassword = null, string email = null
                , int[] permissionIds = null)
        {
            var result = _enterpriseService.CreateEmployee(account, logPassword, email,
                birthday, gender, nick, managePassword, phoneNumber, permissionIds);
            return result;
        }

        #region 权限/角色 管理
        [HttpGet]
        public ActionResult RoleManage()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult PermissionSet(int page = 1, int pageSize = 5)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                IPagedList<UserViewModel> users = _enterpriseService.GetAllEmployee()
                    .Where(e => e.EmployeeId != employee.EmployeeId)
                    .Select(e => new UserViewModel
                    {
                        UserId = e.User.UserId,
                        Account = e.User.Account,
                        NickName = e.User.NickName,
                        Gender = (bool)e.User.Gender,
                        Birthday = (DateTime)e.User.Birthday,
                        CreateTime = e.User.CreateTime,
                        PhoneNumber = e.User.PhoneNumber,
                        Email = e.User.Email,
                        Photo = e.User.Photo,
                        RealName = e.User.RealName
                    }).ToPagedList(page, pageSize);
                return View(users);
            }
        }

        [HttpGet]
        public List<UserViewModel> GetPermissions()
        {
            List<Permissions> permissions = _menuViewService.GetAllPermissions();
            List<UserViewModel> permissionsDTO = new List<UserViewModel>();
            //待完善
            return permissionsDTO;
        }

        [HttpPost]
        public ActionResult PermissionSet(int employeeId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetEmployeeInfo(int employeeId)
        {
            User user = _enterpriseService.GetUserByEmployeeId(employeeId);
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = (bool)user.Gender,
                Birthday = (DateTime)user.Birthday,
                CreateTime = user.CreateTime,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return PartialView(user);
        }

        [HttpGet]
        public ActionResult GetEmployeePermissions(int employeeId)
        {
            List<Permissions> permissions = _menuViewService.GetAllPermissionsByEmployeeId(employeeId);
            List<PermissionsViewModel> users = _menuViewService.GetAllPermissionsByEmployeeId(employeeId)
                .Select(e => new PermissionsViewModel
                {
                        PermissionId = e.PermissionId,
                        Name = e.Name,
                        Code = e.Code,
                }).ToList();
            //待完善
            return PartialView(permissions);
        }
        #endregion
        #endregion
    }
}