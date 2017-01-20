using Mall.Data.DataBase;
using Mall.Data.IManagers.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mall.Data.Services.Enterprise
{
    public class EnterpriseService : IDisposable, IUserEnterpriseManager
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
        /// 是否重名
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        public bool ReName(string account)
        {
            var ul = GetAllEmployee()
                .SingleOrDefault(employee => employee.User.Account == account);
            return ul == null ? false : true;
        }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="account">员工账户</param>
        public bool CreateEmployee(string account)
        {
            if (!ReName(account))
            {
                User user = new User
                {
                    Account = account,
                    NickName = "新员工",
                    Password = "654321",
                    CreateTime = DateTime.Now
                };

                _db.User.Add(user);
                _db.Employee.Add(
                    new Employee
                    {
                        UserId = user.UserId
                    }
                );
                _db.SaveChanges();

                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回所有员工
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetAllEmployee()
        {
            return _db.Employee.ToList();
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

        /// <summary>
        /// 给员工添加角色
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="roleIds">角色Ids</param>
        public void SetRolesToEmployee(int employeeId, int[] roleIds)
        {
            var roles = _db.Roles.Where(r => roleIds
                                     .Contains(r.RoleId)).ToList();

            Employee employee = GetEmployeeByEmployeeId(employeeId);

            employee.Roles = new List<Roles>();

            roles.ForEach(role =>
            {
                employee.Roles.Add(role);
            });
            _db.SaveChanges();
        }

        /// <summary>
        /// 给员工添加权限
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="permissionIds">权限Ids</param>
        public void SetPermissionsToEmployee(int employeeId, int[] permissionIds)
        {
            var permissions = _db.Permissions.Where(p => permissionIds
                                     .Contains(p.PermissionId)).ToList();

            Employee employee = GetEmployeeByEmployeeId(employeeId);

            permissions.ForEach(permission =>
            {
                employee.Permissions.Add(permission);
            });

            _db.SaveChanges();
        }

        /// <summary>
        /// 根据员工Id删除员工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        public void DeleteEmployeeByEmployeeId(int employeeId)
        {
            Employee employee = _db.Employee.Include("User").SingleOrDefault(e => e.EmployeeId == employeeId);
            _db.User.Remove(employee.User);
            _db.Employee.Remove(employee);

            _db.SaveChanges();
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">权限Ids</param>
        public void CreateRole(string roleName, int[] permissionIds)
        {
            var permissions = _db.Permissions.Where(p => permissionIds
                                            .Contains(p.PermissionId)).ToList();
            Roles role = new Roles
            {
                Name = roleName,
                CreationTime = DateTime.Now,
            };

            permissions.ForEach(permission =>
            {
                role.Permissions.Add(permission);
            });

            _db.Roles.Add(role);
            _db.SaveChanges();
        }

        /// <summary>
        /// 通过角色Id修改角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">角色权限Ids</param>
        public void ModifyRoleByRoleId(int roleId, string roleName, int[] permissionIds)
        {
            var permissions = _db.Permissions.Where(p => permissionIds
                                     .Contains(p.PermissionId)).ToList();

            Roles role = _db.Roles.Include("Permissions").SingleOrDefault(r => r.RoleId == roleId);

            role.Name = roleName;
            role.Permissions = new List<Permissions>();

            permissions.ForEach(permission =>
            {
                role.Permissions.Add(permission);
            });

            _db.SaveChanges();
        }

        /// <summary>
        /// 根据角色Id删除角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        public void DeleteRoleByRoleId(int roleId)
        {
            Roles role = _db.Roles.SingleOrDefault(r => r.RoleId == roleId);

            _db.Roles.Remove(role);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
