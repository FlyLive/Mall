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
    public class EnterpriseService : IDisposable, IUserEnterpriseApplicationData
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
            var ul = _db.User.SingleOrDefault(u => u.Account == account);
            return ul == null ? false : true;
        }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="account">员工账户</param>
        public bool CreateEmployee(string account, string logPassword, string email,
                DateTime? birthday, bool gender = true, string nick = null,
                string managePassword = null, string phoneNumber = null
                , int[] permissionIds = null)
        {
            if (!ReName(account))
            {
                User user = new User
                {
                    Account = account,
                    Password = logPassword,
                    Email = email,
                    Photo = "../Pictures/Users/Avatar/avatar.png",
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

                _db.SaveChanges();

                if(permissionIds != null)
                {
                    SetPermissionsToEmployee(employee.EmployeeId, permissionIds);
                
                    _db.SaveChanges();
                }

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
        /// 修改个人信息
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="realName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="nick"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        public void ModifyInfo(int employeeId, string realName, string phone, string email, DateTime? birthday, string nick, int gender)
        {
            Employee employee = GetEmployeeByEmployeeId(employeeId);
            User user = employee.User;

            user.RealName = realName;
            user.PhoneNumber = phone;
            user.Email = email;
            user.NickName = nick;
            user.Gender = gender == 1 ? true : false;
            user.Birthday = birthday;

            _db.SaveChanges();
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="imgBase"></param>
        public string ModifyPhoto(int employeeId, string imgBase)
        {
            Employee employee = GetEmployeeByEmployeeId(employeeId);

            try
            {
                var img = imgBase.Split(',');
                byte[] bt = Convert.FromBase64String(img[1]);
                string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                string path = "C:/Users/LL/Documents/Visual Studio 2015/Projects/Mall/Mall.Web.Back/Pictures/Users/Avatar/avatar" + now + ".png";
                string DataPath = "../Pictures/Users/Avatar/avatar" + now + ".png";
                File.WriteAllBytes(path, bt);
                employee.User.Photo = DataPath;
                _db.SaveChanges();
                return DataPath;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// 修改员工登录密码
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="newLogPassword"></param>
        public void ModifyLogPassword(int employeeId,string newLogPassword)
        {
            Employee employee = GetEmployeeByEmployeeId(employeeId);
            employee.User.Password = newLogPassword;

            _db.SaveChanges();
        }
        /// <summary>
        /// 修改员工登录密码
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="newManagePassword"></param>
        public void ModifyManagePassword(int employeeId,string newManagePassword)
        {
            Employee employee = GetEmployeeByEmployeeId(employeeId);
            employee.ManagePassword = newManagePassword;

            _db.SaveChanges();
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
