using Mall.Service.DataBase;
using Mall.Service.Services.Enterprise;
using Mall.Web.Back.Filter;
using Mall.Web.Back.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class StaffManageController : Controller
    {
        private EnterpriseService _enterpriseService = new EnterpriseService();
        private MenuViewService _menuViewService = new MenuViewService();

        /// <summary>
        /// 管理员详情(View)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize("DistributionAuthority")]
        public ActionResult EmployeeDetails(int userId)
        {
            User user = _enterpriseService.GetEmployeeByUserId(userId).User;
            UserViewModel employeeDTO = DataUserToDTO(user);
            return View(employeeDTO);
        }

        /// <summary>
        /// 管理员操作日志(View)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize("DistributionAuthority")]
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
        [PermissionAuthorize("AddAdmin")]
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
        /// 权限Menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionAuthorize("AddAdmin")]
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
        [HttpPost]
        [PermissionAuthorize("AddAdmin")]
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
            var result = _enterpriseService.CreateEmployee(employee.EmployeeId, account, logPassword, email,
                birthday, gender, nick, managePassword, phoneNumber, permissionIds);
            return result;
        }
        #endregion

        #region 权限/角色 管理
        [HttpGet]
        public ActionResult SearchEmployee(string search = "")
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            List<UserViewModel> users = _enterpriseService.GetAllEmployee()
                .Where(e => e.User.RealName.Contains(search) && e.EmployeeId != employee.EmployeeId)
                .Select(e => DataUserToDTO(e.User)).ToList();
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
        [PermissionAuthorize("DistributionAuthority")]
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
        [PermissionAuthorize("DistributionAuthority")]
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
                    Details = e.Details,
                    CreationTime = e.CreationTime
                }).ToList();
            return PartialView(permissionsDTO);
        }

        [HttpPost]
        [PermissionAuthorize("DistributionAuthority")]
        public bool ModifyEmployeePermissions(int userId, int[] menuIds = null)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            bool result = _enterpriseService.ModifyPermissions(employee.EmployeeId, userId, menuIds);
            return result;
        }
        #endregion

        #region 角色
        /// <summary>
        /// 角色管理(View)
        /// </summary>
        /// <returns></returns>
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
                    Details = r.Details
                }).ToList();
            return View(rolesDTO);
        }

        /// <summary>
        /// 创建角色(Action)
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="roleName"></param>
        /// <param name="roleDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize("RoleManage")]
        public bool CreateRole(int[] menuIds, string roleName, string roleDetails)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            var result = _enterpriseService.CreateRole(employee.EmployeeId, roleName, roleDetails, menuIds);
            return result;
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
        public bool ModifyRolePermissions(int roleId, int[] menuIds, string modifyName = null, string modifyDetails = null)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            var result = _enterpriseService.ModifyRoleByRoleId(employee.EmployeeId, roleId, menuIds, modifyName, modifyDetails);
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize("RoleManage")]
        public bool DeletRoleByRoleId(int roleId)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            var result = _enterpriseService.DeleteRoleByRoleId(employee.EmployeeId, roleId);
            return result;
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
        [PermissionAuthorize("RoleManage")]
        public ActionResult GetEmployeeRoles(int userId)
        {
            Employee employee = _enterpriseService.GetEmployeeByUserId(userId);
            List<RolesViewModel> rolesDTO = _enterpriseService
                .GetEmployeeRolesByEmployeeId(employee.EmployeeId)
                .Select(r => new RolesViewModel
                {
                    Id = r.RoleId,
                    Code = r.Code,
                    CreationTime = r.CreationTime,
                    Name = r.Name,
                    Details = r.Details
                }).ToList();
            return PartialView(rolesDTO);
        }

        /// <summary>
        /// 修改权限(Action)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionAuthorize("RoleManage")]
        public bool ModifyEmployeeRoles(int userId, int[] roleIds)
        {
            EmployeeViewModel employee = (EmployeeViewModel)Session["Employee"];
            if (employee == null)
            {
                return false;
            }
            var result = _enterpriseService.ModifyRolesById(employee.EmployeeId, userId, roleIds);
            return result;
        }

        [HttpGet]
        [PermissionAuthorize("RoleManage")]
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

            var allRoles = roles.Union(employeeRoles).ToList();

            List<RolesViewModel> employeeRoleMenusDTO = allRoles
                .Select(r => new RolesViewModel
                {
                    Id = r.RoleId,
                    IsDefault = r.IsDefault == null ? false : (bool)r.IsDefault,
                    ParentId = 0,
                    Name = r.Name,
                    Code = r.Code,
                    Has = employeeRoles.SingleOrDefault(er => er.RoleId == r.RoleId) == null ? false : true,
                    Details = r.Details,
                }
            ).ToList();

            allRoles.ForEach(r => employeeRoleMenusDTO
                .AddRange(_enterpriseService.GetRolePermissionsByRoleId(r.RoleId)
                    .Select(rp => new RolesViewModel
                    {
                        Id = -1,
                        IsDefault = true,
                        ParentId = r.RoleId,
                        Name = rp.Name,
                        Code = rp.Code,
                        Has = false,
                        Details = r.Details,
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
                Birthday = user.Birthday == null ? "0000-00-00" : user.Birthday.Value.ToString("yyyy-MM-dd"),
                CreateTime = user.CreateTime == null ? "0000-00-00 00-00-00" : user.CreateTime.ToString(),
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Photo = user.Photo,
                RealName = user.RealName
            };
            return userDTO;
        }
    }
}