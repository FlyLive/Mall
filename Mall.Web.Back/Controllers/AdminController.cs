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
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            User user = _enterpriseService.GetUserByEmployeeId(employee.EmployeeId);
            UserViewModel userDTO = DataUserToDTO(user);
            return PartialView(userDTO);
        }        

        #region 用户中心
        [HttpGet]
        public ActionResult EmployeeDetails(int userId)
        {
            User user = _enterpriseService.GetEmployeeByUserId(userId).User;
            UserViewModel employeeDTO = DataUserToDTO(user);
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
            return PartialView(adminLogsDTO);
        }

        [HttpGet]
        public ActionResult PersonalInfoSet()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            User user = _enterpriseService.GetUserByEmployeeId(employee.UserId);
            UserViewModel userDTO = DataUserToDTO(user);
            return View(userDTO);
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
            UserViewModel userDTO = DataUserToDTO(user);
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

        #region 员工管理
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
        /// 创建员工(View)
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
            return View();
        }

        /// <summary>
        /// 创建员工(Permissions)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllMenus()
        {
            List<PermissionsViewModel> menus = GetAllPermissionsMenu();
            JsonResult json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.Data = menus;
            return json;
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
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            var result = _enterpriseService.CreateEmployee(employee.EmployeeId,account, logPassword, email,
                birthday, gender, nick, managePassword, phoneNumber, permissionIds);
            return result;
        }
        #endregion

        #region 权限/角色 管理

        [HttpGet]
        public ActionResult SearchEmployee(string search)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            List<UserViewModel> users = _enterpriseService.GetAllEmployee()
                .Where(e => e.User.RealName.Contains(search) && e.EmployeeId != employee.EmployeeId)
                .Select(e => new UserViewModel
                {
                    UserId = e.User.UserId,
                    Account = e.User.Account,
                    NickName = e.User.NickName,
                    Gender = (bool)e.User.Gender,
                    Birthday = e.User.Birthday == null ? "0000-00-00 00-00-00" : e.User.Birthday.ToString(),
                    CreateTime = e.User.CreateTime == null ? "0000-00-00 00-00-00" : e.User.CreateTime.ToString(),
                    PhoneNumber = e.User.PhoneNumber,
                    Email = e.User.Email,
                    Photo = e.User.Photo,
                    RealName = e.User.RealName
                }).ToList();
            return View(users);
        }

        #region 权限
        /// <summary>
        /// 权限设置(View)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize("DistributionAuthority")]
        public ActionResult PermissionSet()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ActionResult GetEmployeePermissionsInfo(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            UserViewModel userDTO = DataUserToDTO(employee.User);
            return PartialView(userDTO);
        }

        [HttpGet]
        [PermissionAuthorize("DistributionAuthority")]
        public JsonResult GetPermissions(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            List<PermissionsViewModel> permissionsDTO = GetPermissionsMenuByEmployeeId(employee.EmployeeId);
            
            JsonResult json = new JsonResult();
            json.Data = permissionsDTO;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetEmployeePermissions(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            List<Permissions> permissions = _menuViewService.GetAllPermissionsByEmployeeId(employee.EmployeeId);
            List<PermissionsViewModel> permissionsDTO = permissions
                .Select(e => new PermissionsViewModel
                {
                    Id = e.PermissionId,
                    Name = e.Name,
                    Code = e.Code,
                }).ToList();
            return PartialView(permissionsDTO);
        }

        [HttpPost]
        [PermissionAuthorize("DistributionAuthority")]
        public bool ModifyEmployeePermissions(int userId, int[] menuIds)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            bool result = _enterpriseService.ModifyPermissions(employee.EmployeeId,userId, menuIds);
            return result;
        }

        #endregion

        #region 角色
        [HttpGet]
        [PermissionAuthorize("RoleManage")]
        public ActionResult RoleManage()
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            List<RolesViewModel> rolesDTO = _enterpriseService.GetRoles()
                .Select(r => new RolesViewModel
                {
                    Id = r.RoleId,
                    Code = r.Code,
                    CreationTime = r.CreationTime,
                    Name = r.Name,
                }).ToList();
            return View(rolesDTO);
        }

        [HttpPost]
        [PermissionAuthorize("RoleManage")]
        public bool CreateRole(int[] menuIds,string roleName)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            _enterpriseService.CreateRole(employee.EmployeeId,roleName, menuIds);
            return true;
        }

        [HttpGet]
        [PermissionAuthorize("RoleManage")]
        public JsonResult GetAllPermissions()
        {
            List<Menus> menus = _menuViewService.GetMenus().Where(m => m.IsDefault == false).ToList();
            List<PermissionsViewModel> permissionsDTO = menus.Select(m => new PermissionsViewModel
            {
                Id = m.MenuId,
                ParentId = m.ParentId,
                Name = m.MenuName,
                Code = m.MenuPath,
                Has = false,
                IsDefault = false,
            }).ToList();

            JsonResult json = new JsonResult();
            json.Data = permissionsDTO;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        [PermissionAuthorize("RoleManage")]
        public JsonResult GetRolePermissionsMenu(int roleId)
        {
            List<Permissions> rolePermissions = _enterpriseService.GetRolePermissionsByRoleId(roleId);
            List<Menus> menus = _menuViewService.GetMenus().Where(m => m.IsDefault == false).ToList();
            List<PermissionsViewModel> permissionsDTO = menus.Select(m => new PermissionsViewModel
            {
                Id = m.MenuId,
                ParentId = m.ParentId,
                Name = m.MenuName,
                Code = m.MenuPath,
                Has = rolePermissions.SingleOrDefault(rp => rp.Code == m.MenuPath) == null ? false : true,
                IsDefault = false,
            }).ToList();

            JsonResult json = new JsonResult();
            json.Data = permissionsDTO;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpPost]
        [PermissionAuthorize("RoleManage")]
        public bool ModifyRolePermissions(int roleId,int[] menuIds,string name = null)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            _enterpriseService.ModifyRoleByRoleId(employee.EmployeeId,roleId, name, menuIds);
            return true;
        }

        [HttpGet]
        [PermissionAuthorize("RoleManage")]
        public JsonResult GetRoles(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            List<RolesViewModel> rolesDTO = GetRolesMenuByEmployeeId(employee.EmployeeId);
            JsonResult json = new JsonResult();
            json.Data = rolesDTO;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        [HttpGet]
        public ActionResult GetEmployeeRoles(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            List<RolesViewModel> rolesDTO = _enterpriseService
                .GetEmployeeRolesByEmployeeId(employee.EmployeeId)
                .Select(r => new RolesViewModel {
                    Id = r.RoleId,
                    Code = r.Code,
                    CreationTime = r.CreationTime,
                    Name = r.Name,
                }).ToList();
            return PartialView(rolesDTO);
        }

        [HttpPost]
        [PermissionAuthorize("RoleManage")]
        public bool ModifyEmployeeRoles(int userId, int[] roleIds)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            bool result = _enterpriseService.ModifyRolesById(employee.EmployeeId,userId, roleIds);
            return result;
        }

        [HttpGet]
        public ActionResult GetEmployeeRolesInfo(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            UserViewModel userDTO = DataUserToDTO(employee.User);
            return PartialView(userDTO);
        }
        #endregion
        #endregion

        private List<PermissionsViewModel> GetAllPermissionsMenu()
        {
            List<Menus> menus = _menuViewService.GetMenus();
            List<PermissionsViewModel> employeeMenusDTO = menus
                .Select(p => new PermissionsViewModel
                {
                    Id = p.MenuId,
                    ParentId = p.ParentId,
                    Name = p.MenuName,
                    Code = p.MenuPath,
                    Has = false,
                    IsDefault = p.IsDefault == true ? true : false,
                }
            ).ToList();

            return employeeMenusDTO;
        }

        private List<PermissionsViewModel> GetPermissionsMenuByEmployeeId(int employeeId)
        {
            List<Menus> menus = _menuViewService.GetMenus();
            List<Menus> employeeMenus = _menuViewService.GetMenuByEmployeeId(employeeId);

            List<PermissionsViewModel> employeeMenusDTO = menus
                .Select(m => new PermissionsViewModel
                {
                    Id = m.MenuId,
                    ParentId = m.ParentId,
                    Name = m.MenuName,
                    Code = m.MenuPath,
                    Has = employeeMenus.SingleOrDefault(em => em.MenuId == m.MenuId) == null ? false : true,
                    IsDefault = m.IsDefault == null ? false : (bool)m.IsDefault,
                }
            ).ToList();

            return employeeMenusDTO;
        }

        private List<RolesViewModel> GetRolesMenuByEmployeeId(int employeeId)
        {
            List<Roles> roles = _enterpriseService.GetRoles();
            List<Roles> employeeRoles = _enterpriseService.GetEmployeeRolesByEmployeeId(employeeId);

            List<RolesViewModel> employeeRoleMenusDTO = roles
                .Select(r => new RolesViewModel
                {
                    Id = r.RoleId,
                    IsDefault = r.Default == null ? false : (bool)r.Default,
                    ParentId = 0,
                    Name = r.Name,
                    Code = r.Code,
                    Has = employeeRoles.SingleOrDefault(er => er.RoleId == r.RoleId) == null ? false : true,
                }
            ).ToList();

            roles.ForEach(r => employeeRoleMenusDTO
                .AddRange(_enterpriseService.GetRolePermissionsByRoleId(r.RoleId)
                    .Select(rp => new RolesViewModel
                    {
                        Id = rp.PermissionId,
                        IsDefault = true,
                        ParentId = r.RoleId,
                        Name = rp.Name,
                        Code = rp.Code,
                        Has = false,
                    }).ToList()));

            employeeRoles.ForEach(er => employeeRoleMenusDTO
                .AddRange(_enterpriseService.GetRolePermissionsByRoleId(er.RoleId)
                    .Select(rp => new RolesViewModel
                    {
                        Id = rp.PermissionId,
                        IsDefault = true,
                        ParentId = er.RoleId,
                        Name = rp.Name,
                        Code = rp.Code,
                        Has = false,
                    }).ToList()));

            return employeeRoleMenusDTO;
        }

        public static UserViewModel DataUserToDTO(User user)
        {
            UserViewModel userDTO = new UserViewModel
            {
                UserId = user.UserId,
                Account = user.Account,
                NickName = user.NickName,
                Gender = user.Gender == null ? true : (bool)user.Gender,
                Birthday = user.Birthday == null ? "0000-00-00 00-00-00" : user.Birthday.ToString(),
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