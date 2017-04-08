using Mall.Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mall.Service.Services.Enterprise
{
    public class MenuViewService : IDisposable
    {
        private MallDBContext _db;
        public MenuViewService()
        {
            _db = new MallDBContext();
        }

        /// <summary>
        /// 通过权限Id和角色Id给角色添加权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="roleId"></param>
        public void AddPermissionToRole(int permissionId, int roleId)
        {
            Permissions permission = GetPermissionByPermissionId(permissionId);
            Roles role = GetRoleByRoleId(roleId);

            role.Permissions.Add(permission);

            _db.SaveChanges();
        }

        /// <summary>
        /// 通过员工Id和权限Id给员工添加权限
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="permissionId"></param>
        public void AddPermissionToEmployeeById(int EmployeeId, int permissionId)
        {
            Employee user = _db.Employee.Include("Permissions")
                .SingleOrDefault(u => u.EmployeeId == EmployeeId);
            Permissions permission = GetPermissionByPermissionId(permissionId);

            user.Permissions.Add(permission);

            _db.SaveChanges();
        }

        /// <summary>
        /// 通过员工Id和角色Id给员工添加角色
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="roleId"></param>
        public void AddRoleToEmployeeById(int EmployeeId, int roleId)
        {
            Employee employee = _db.Employee.Include("Roles")
                .SingleOrDefault(u => u.UserId == EmployeeId);
            Roles role = GetRoleByRoleId(roleId);

            employee.Roles.Add(role);

            _db.SaveChanges();
        }

        /// <summary>
        /// 通过权限列表
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<Menus> GetMenus()
        {
            List<Menus> menus = _db.Menus.ToList();
            return menus;
        }

        /// <summary>
        /// 通过员工权限返回相应权限列表
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<Menus> GetMenuByEmployeeId(int employeeId)
        {
            List<Permissions> employeePermissions = GetAllPermissionsByEmployeeId(employeeId);
            List<Menus> menus = _db.Menus.ToList();
            var employeeMenus = menus.Where(m => employeePermissions.Any(p => p.Code.StartsWith(m.MenuPath))).ToList();

            return employeeMenus;
        }

        /// <summary>
        /// 通过角色Id返回角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Roles GetRoleByRoleId(int roleId)
        {
            Roles role = _db.Roles
                .SingleOrDefault(r => r.RoleId == roleId);
            return role;
        }

        /// <summary>
        /// 通过权限Id返回权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public Permissions GetPermissionByPermissionId(int permissionId)
        {
            Permissions permission = _db.Permissions
                .SingleOrDefault(p => p.PermissionId == permissionId);
            return permission;
        }

        /// <summary>
        /// 返回全部已有角色
        /// </summary>
        /// <returns></returns>
        public List<Roles> GetAllRoles()
        {
            List<Roles> roles = _db.Roles.ToList();
            return roles;
        }

        /// <summary>
        /// 返回全部已有权限
        /// </summary>
        /// <returns></returns>
        public List<Permissions> GetAllPermissions()
        {
            List<Permissions> permissions = _db.Permissions.ToList();
            return permissions;
        }

        /// <summary>
        /// 返回用户的全部权限
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<Permissions> GetAllPermissionsByEmployeeId(int employeeId)
        {
            Employee employee = _db.Employee
                .Include("Permissions")
                .Include("Roles")
                .SingleOrDefault(u => u.EmployeeId == employeeId);

            var roles = employee.Roles.ToList();
            List<Permissions> permissions = employee.Permissions.ToList();

            var roleIds = roles.Select(r => r.RoleId);
            List<Permissions> rolePermissions = _db.Permissions
                .Where(p => p.Roles
                        .Any(r => roleIds
                        .Contains(r.RoleId))
                ).ToList();

            List<Permissions> employeePermissions = rolePermissions.Union(permissions).ToList();
            return employeePermissions;
        }

        /// <summary>
        /// 返回用户的全部角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Roles> GetAllRolesByEmployeeId(int employeeId)
        {
            List<Roles> roles = _db.Employee
                .SingleOrDefault(e => e.EmployeeId == employeeId)
                .Roles.ToList();
            return roles;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}