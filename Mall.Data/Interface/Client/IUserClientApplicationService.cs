using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Interface.Client
{
    public interface IUserClientApplicationService
    {
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="client"></param>
        void ModifyUserInfo(int clientId, string email, DateTime? birthday, string nick, string name, string phone, int gender = 1);
        /// <summary>
        /// 新建收货信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="address"></param>
        /// <param name="contact"></param>
        /// <param name="phone"></param>
        /// <param name="zip"></param>
        void CreatDeliverInfo(int clientId, string address, string contact, string phone, string zip = " ");
        /// <summary>
        /// 修改收货信息
        /// </summary>
        /// <param name="deliveryInfoId"></param>
        /// <param name="address"></param>
        /// <param name="contact"></param>
        /// <param name="phone"></param>
        /// <param name="zip"></param>
        void ModifyDeliverInfo(int deliveryInfoId, string address, string contact, string phone, string zip = " ");
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="clientId">用户Id</param>
        /// <param name="newPassword">新密码</param>
        void ModifyPasswordByClientId(int clientId, string newPassword);
    }
}
