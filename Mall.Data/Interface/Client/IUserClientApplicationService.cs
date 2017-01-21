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
        void ModifyUserInfo(User user,int clientId);
        /// <summary>
        /// 新建收货信息
        /// </summary>
        /// <param name="deliverInfo"></param>
        void CreatDeliverInfo(DeliveryInfo deliverInfo);
        /// <summary>
        /// 修改收货信息
        /// </summary>
        /// <param name="deliverInfo"></param>
        void ModifyDeliverInfo(DeliveryInfo newDeliverInfo,int deliveryInfoId);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="clientId">用户Id</param>
        /// <param name="newPassword">新密码</param>
        void ModifyPasswordByUserId(int clientId, string newPassword);
    }
}
