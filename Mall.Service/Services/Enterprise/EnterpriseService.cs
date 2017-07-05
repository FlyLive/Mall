using Mall.Interface.Enterprise;
using Mall.Service.DataBase;
using Mall.Service.Services.Enterprise;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mall.Service.Services.Enterprise
{
    public class EnterpriseService : IDisposable, IUserEnterpriseApplicationService
    {
        private MallDBContext _db;
        public EnterpriseService()
        {
            _db = new MallDBContext();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Employee Login(string account, string password)
        {
            Employee employee = GetEmployeeByAccount(account);
            if (employee != null)
            {
                if (employee.User.Password == password)
                    return employee;
            }
            return null;
        }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="account">员工账户</param>
        public bool CreateEmployee(int employeeId, string account, string logPassword, string email,
                DateTime? birthday, bool gender = true, string nick = null,
                string managePassword = null, string phoneNumber = null
                , int[] menuIds = null)
        {
            try
            {
                Employee actionEmployee = GetEmployeeByEmployeeId(employeeId);
                var reName = _db.User.SingleOrDefault(u => u.Account == account) == null ? false : true;
                if (!reName)
                {
                    User user = new User
                    {
                        Account = account,
                        Password = logPassword,
                        Email = email,
                        Photo = "http://localhost:9826/Mall.Web.Back/Users/Avatar/avatar.png",
                        CreateTime = DateTime.Now,

                        NickName = nick == "" ? account : nick,
                        PhoneNumber = phoneNumber,
                        Gender = gender
                    };

                    _db.User.Add(user);

                    Employee employee = new Employee
                    {
                        UserId = user.UserId,
                        ManagePassword = managePassword == "" ? logPassword : managePassword,
                    };
                    _db.Employee.Add(employee);
                    _db.AdminLog.Add(new AdminLog
                    {
                        EmployeeId = actionEmployee.EmployeeId,
                        Permission = "员工管理(DistributionAuthority)",
                        OperationTime = DateTime.Now,
                        OperatDetail = "员工" + actionEmployee.User.RealName + "于" + DateTime.Now + "创建员工:" + actionEmployee + ",员工Id为:" + employee.EmployeeId + "！",
                        Operater = actionEmployee.User.RealName,
                        Object = "员工",
                        Style = "新增",
                    });
                    
                    SetPermissionsToEmployee(actionEmployee.EmployeeId, employee.EmployeeId, menuIds);

                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 返回所有员工
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetAllEmployee()
        {
            return _db.Employee.Include("User").ToList();
        }

        /// <summary>
        /// 根据用户Id返回员工
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Employee GetEmployeeByUserId(int userId)
        {
            return _db.Employee.Include("AdminLog")
                .SingleOrDefault(e => e.UserId == userId);
        }

        /// <summary>
        /// 通过员工账户返回员工
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        public Employee GetEmployeeByAccount(string account)
        {
            Employee employee = GetAllEmployee()
                .SingleOrDefault(u => u.User.Account == account);
            return employee;
        }

        /// <summary>
        /// 根据员工Id返回操作记录
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<AdminLog> GetAdminLogsByEmployeeId(int employeeId)
        {
            Employee employee = GetEmployeeByEmployeeId(employeeId);
            List<AdminLog> adminLogs = employee.AdminLog.ToList();
            return adminLogs;
        }
        
        /// <summary>
        /// 修改员工管理密码
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="newManagePassword"></param>
        public bool ModifyManagePassword(int employeeId, string newManagePassword)
        {
            try
            {
                Employee employee = GetEmployeeByEmployeeId(employeeId);
                employee.ManagePassword = newManagePassword;

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 通过员工Id返回用户
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns></returns>
        public User GetUserByEmployeeId(int employeeId)
        {
            Employee employee = _db.Employee.Include("User")
                .SingleOrDefault(u => u.EmployeeId == employeeId);
            return employee.User;
        }

        /// <summary>
        /// 通过员工Id返回员工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <returns></returns>
        public Employee GetEmployeeByEmployeeId(int employeeId)
        {
            Employee employee = _db.Employee
                .SingleOrDefault(u => u.EmployeeId == employeeId);
            return employee;
        }

        public List<Roles> GetRoles()
        {
            List<Roles> roles = _db.Roles.ToList();
            return roles;
        }

        public List<Roles> GetEmployeeRolesByEmployeeId(int employeeId)
        {
            List<Roles> roles = GetEmployeeByEmployeeId(employeeId).Roles.ToList();
            return roles;
        }

        public List<Permissions> GetRolePermissionsByRoleId(int roleId)
        {
            List<Permissions> permissions = _db.Roles.Include("Permissions")
                .SingleOrDefault(r => r.RoleId == roleId).Permissions.ToList();
            return permissions;
        }

        public int[] GetDefaultPermissionIds()
        {
            int[] defaultPermissionsIds = _db.Permissions.Where(p => p.IsDefault == true).Select(p => p.PermissionId).ToArray();
            return defaultPermissionsIds;
        }

        /// <summary>
        /// 给员工添加角色
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="roleIds">角色Ids</param>
        public bool ModifyRolesById(int actionEmployeeId, int userId, int[] roleIds)
        {
            try
            {
                Employee actionEmployee = GetEmployeeByEmployeeId(actionEmployeeId);
                Employee employee = GetEmployeeByUserId(userId);
                if (employee == null)
                {
                    return false;
                }
                var roles = _db.Roles.Where(r => roleIds
                                         .Contains(r.RoleId)).ToList();

                employee.Roles = new List<Roles>();

                roles.ForEach(role =>
                {
                    employee.Roles.Add(role);
                });
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = actionEmployee.EmployeeId,
                    Permission = "角色管理(RoleManage)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + actionEmployee.User.RealName + "于" + DateTime.Now + "修改员工:" + employee.User.RealName + "角色,员工Id为:" + employee.EmployeeId + "！",
                    Operater = actionEmployee.User.RealName,
                    Object = "员工",
                    Style = "修改",
                });

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        //菜单id转换为权限Id
        private int[] SwitchMenuIdsToPermissionIds(int[] menuIds)
        {
            List<Permissions> permissions = _db.Permissions.ToList();
            var permissionCodes = _db.Permissions.Select(p => p.Code).ToList();

            List<Menus> menus = _db.Menus.ToList();
            List<Menus> permissionsMenu = new List<Menus>();

            permissionsMenu.AddRange(
                menus.Where(m => permissionCodes.Any(p => p == m.MenuPath) && menuIds.Any(mi => mi == m.MenuId)));

            int[] permissionIds = new int[permissionsMenu.Count];

            for (int i = 0; i < permissionIds.Length; i++)
            {
                permissionIds[i] = permissions.SingleOrDefault(p => p.Code == permissionsMenu[i].MenuPath).PermissionId;
            }
            return permissionIds;
        }

        /// <summary>
        /// 根据用户Id设置权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public bool ModifyPermissions(int actionEmployeeId, int userId, int[] menuIds = null)
        {
            Employee actionEmployee = GetEmployeeByEmployeeId(actionEmployeeId);
            Employee employee = GetEmployeeByUserId(userId);
            if (employee == null)
            {
                return false;
            }
            else
            {
                var defaultPermissionsIds = _db.Permissions.Where(p => p.IsDefault == true).Select(p => p.PermissionId);
                var permissionIds = SwitchMenuIdsToPermissionIds(menuIds);
                permissionIds.ToList().AddRange(defaultPermissionsIds);
                var result = SetPermissionsToEmployee(actionEmployee.EmployeeId, employee.EmployeeId, permissionIds);
                return result;
            }
        }

        /// <summary>
        /// 修改员工权限
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="permissionIds">权限Ids</param>
        public bool SetPermissionsToEmployee(int actionEmployeeId, int employeeId, int[] permissionIds)
        {
            try
            {
                var permissions = _db.Permissions.Where(p => permissionIds.ToList()
                                         .Any(pi => pi == p.PermissionId)).ToList();

                Employee actionEmployee = GetEmployeeByEmployeeId(actionEmployeeId);
                Employee employee = GetEmployeeByEmployeeId(employeeId);
                employee.Permissions = new List<Permissions>();

                permissions.ForEach(p =>
                {
                    employee.Permissions.Add(p);
                });
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = actionEmployee.EmployeeId,
                    Permission = "权限管理(DistributionAuthority)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + actionEmployee.User.RealName + "于" + DateTime.Now + "修改员工:" + employee.User.RealName + "权限,员工Id为:" + employee.EmployeeId + "！",
                    Operater = actionEmployee.User.RealName,
                    Object = "员工",
                    Style = "修改",
                });
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 根据员工Id删除员工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        public bool DeleteEmployeeByEmployeeId(int actionEmployeeId, int employeeId)
        {
            try
            {
                Employee actionEmployee = GetEmployeeByEmployeeId(actionEmployeeId);
                Employee employee = _db.Employee.Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);

                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = actionEmployee.EmployeeId,
                    Permission = "权限管理(DistributionAuthority)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + actionEmployee.User.RealName + "于" + DateTime.Now + "删除员工:" + employee.User.RealName + ",员工Id为:" + employee.EmployeeId + "！",
                    Operater = actionEmployee.User.RealName,
                    Object = "员工",
                    Style = "删除",
                });
                _db.User.Remove(employee.User);
                _db.Employee.Remove(employee);

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">权限Ids</param>
        public bool CreateRole(int employeeId, string roleName, string roleDetails, int[] menuIds)
        {
            try
            {
                Employee employee = GetEmployeeByEmployeeId(employeeId);
                var permissionIds = SwitchMenuIdsToPermissionIds(menuIds);

                var permissions = _db.Permissions.Where(p => permissionIds
                                                .Contains(p.PermissionId)).ToList();
                Roles role = new Roles
                {
                    Name = roleName,
                    CreationTime = DateTime.Now,
                    Details = roleDetails
                };

                permissions.ForEach(permission =>
                {
                    role.Permissions.Add(permission);
                });

                _db.Roles.Add(role);
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "角色管理(RoleManage)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "新建角色:" + roleName + ",角色Id为:" + role.RoleId + "！",
                    Operater = employee.User.RealName,
                    Object = "角色",
                    Style = "新建",
                });

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 通过角色Id修改角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">角色权限Ids</param>
        public bool ModifyRoleByRoleId(int employeeId, int roleId, int[] menuIds, string roleName, string roleDetails)
        {
            try
            {
                Employee employee = GetEmployeeByEmployeeId(employeeId);
                int[] permissionIds = SwitchMenuIdsToPermissionIds(menuIds);
                var permissions = _db.Permissions.Where(p => permissionIds
                                         .Contains(p.PermissionId)).ToList();

                Roles role = _db.Roles.Include("Permissions").SingleOrDefault(r => r.RoleId == roleId);

                role.Name = roleName == null ? role.Name : roleName;
                role.Name = roleDetails == null ? role.Details : roleDetails;
                role.Permissions = new List<Permissions>();

                permissions.ForEach(permission =>
                {
                    role.Permissions.Add(permission);
                });

                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "角色管理(RoleManage)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "修改角色权限:" + roleName + ",角色Id为:" + role.RoleId + "！",
                    Operater = employee.User.RealName,
                    Object = "角色权限",
                    Style = "修改",
                });

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 根据角色Id删除角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        public bool DeleteRoleByRoleId(int employeeId, int roleId)
        {
            try
            {
                Employee employee = GetEmployeeByEmployeeId(employeeId);
                Roles role = _db.Roles.Include("Permissions").Include("Employee").SingleOrDefault(r => r.RoleId == roleId);
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "角色管理(RoleManage)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "删除角色:" + role.Name + ",角色Id为:" + role.RoleId + "！",
                    Operater = employee.User.RealName,
                    Object = "角色",
                    Style = "删除",
                });

                
                _db.Roles.Remove(role);
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
