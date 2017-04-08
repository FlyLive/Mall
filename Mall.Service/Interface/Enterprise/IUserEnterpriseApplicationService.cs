using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Interface.Enterprise
{
    public interface IUserEnterpriseApplicationService
    {
        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="employeeId">用户Id</param>
        /// <param name="roleIds">角色Ids</param>
        bool ModifyRolesById(int actionEmployeeId,int employeeId, int[] roleIds);
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="employeeId">用户Id</param>
        /// <param name="permissionIds">权限Ids</param>
        bool SetPermissionsToEmployee(int actionEmployeeId,int employeeId, int[] permissionIds);
        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="account"></param>
        /// <param name="logPassword"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="birthday"></param>
        /// <param name="gender"></param>
        /// <param name="nick"></param>
        /// <param name="managePassword"></param>
        /// <param name="email"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        bool CreateEmployee(int employeeId,string account, string logPassword, string email,
                DateTime? birthday, bool gender = true, string nick = null,
                string managePassword = null, string phoneNumber = null
                , int[] menuIds = null);
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        bool DeleteEmployeeByEmployeeId(int actionEmployeeId,int employeeId);
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="permissions">权限Ids</param>
        bool CreateRole(int employeeId,string roleName,string roleDetails, int[] permissionIds);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">权限Ids</param>
        bool ModifyRoleByRoleId(int employeeId,int roleId,  int[] permissionIds,string roleName,string roleDetails);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        bool DeleteRoleByRoleId(int employeeId,int roleId);
    }
}
