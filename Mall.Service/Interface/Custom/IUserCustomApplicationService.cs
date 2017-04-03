using Mall.Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Interface.Custom
{
    public interface IUserCustomApplicationService
    {
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="custom"></param>
        void ModifyUserInfo(int customId, string email, DateTime? birthday, string nick, string name, string phone, int gender = 1);
        /// <summary>
        /// 新建收货信息
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="address"></param>
        /// <param name="contact"></param>
        /// <param name="phone"></param>
        /// <param name="zip"></param>
        bool CreatDeliverInfo(int customId, string address, string contact, string phone, string zip = " ");
        /// <summary>
        /// 修改收货信息
        /// </summary>
        /// <param name="deliveryInfoId"></param>
        /// <param name="address"></param>
        /// <param name="contact"></param>
        /// <param name="phone"></param>
        /// <param name="zip"></param>
        void ModifyDeliverInfo(int customId,int deliveryInfoId, string address, string contact, string phone, string zip = " ");
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="customId">用户Id</param>
        /// <param name="newPassword">新密码</param>
        void ModifyPasswordByCustomId(int customId, string newPassword);
        /// <summary>
        /// 根据顾客Id和商品Id和数量向购物车添加商品
        /// </summary>
        /// <param name="customId"></param>
        void AddGoodsToShoppingCart(int customId, int goodsId, int count = 1);
        /// <summary>
        /// 根据顾客Id和商品Id和数量向购物车添加商品
        /// </summary>
        /// <param name="customId"></param>
        void DeleteGoodsFromShoppingCart(int customId, int goodsId);
        /// <summary>
        /// 根据顾客Id和商品Id和数量向购物车修改商品数量
        /// </summary>
        /// <param name="customId"></param>
        void ModifyGoodsCountFromShoppingCart(int customId, int goodsId, int count);
    }
}
