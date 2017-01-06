using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Services
{
    public interface IUserManager
    {
        #region 前台
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="user"></param>
        void ModifyUserInfo(User user);
        /// <summary>
        /// 修改收货信息
        /// </summary>
        /// <param name="deliverInfo"></param>
        void ModifyDeliverInfo(DeliveryInfo deliverInfo);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="password">新密码</param>
        void ModifyPasswordByUserId(int userId,string password);
        #endregion
        #region 后台
        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleIds">角色Ids</param>
        void SetRolesToUser(int userId, int[] roleIds);
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissionIds">权限Ids</param>
        void SetPermissionsToUser(int userId, int[] permissionIds);
        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="employeeName">员工名</param>
        void CreateEmployee(string employeeName);
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
        void CreateRole(string roleName,int[] permissionIds);
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="roleName">角色名</param>
        /// <param name="permissionIds">权限Ids</param>
        void ModifyRoleByRoleId(int roleId,string roleName, int[] permissionIds);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        void DeleteRoleByRoleId(int roleId);
        #endregion
    }
}
