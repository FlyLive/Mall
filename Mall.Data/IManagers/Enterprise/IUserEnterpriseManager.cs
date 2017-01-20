using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.IManagers.Enterprise
{
    public interface IUserEnterpriseManager
    {
        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="employeeId">用户Id</param>
        /// <param name="roleIds">角色Ids</param>
        void SetRolesToEmployee(int employeeId, int[] roleIds);
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissionIds">权限Ids</param>
        void SetPermissionsToEmployee(int userId, int[] permissionIds);
        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="account">员工账户</param>
        bool CreateEmployee(string account);
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        void DeleteEmployeeByEmployeeId(int employeeId);
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="permissions">权限Ids</param>
        void CreateRole(string roleName, int[] permissionIds);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">权限Ids</param>
        void ModifyRoleByRoleId(int roleId, string roleName, int[] permissionIds);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        void DeleteRoleByRoleId(int roleId);
    }
}
