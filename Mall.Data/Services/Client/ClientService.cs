﻿using Mall.Data.DataBase;
using Mall.Data.Interface.Client;
using Mall.Data.Interface.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Services.Client
{
    public class ClientService : IDisposable , IUserClientApplicationService
    {
        private MallDBContext _db;

        public ClientService()
        {
            _db = new MallDBContext();
        }
       
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public DataBase.Client Login(string name, string password)
        {
            DataBase.Client client = GetClientByAccount(name);
            if(client != null)
            {
                return client.User.Password == password ? client : null;
            }
            return null;
        }
        
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="password">密码</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public DataBase.Client Registe(string name, string password,string email)
        {
            if (!ReName(name))
            {
                User user = new User
                {
                    Account = name,
                    NickName = "小白",
                    Password = password,
                    Email = email,
                    CreateTime = DateTime.Now,
                };

                DataBase.Client client = new DataBase.Client
                {
                    UserId = user.UserId,
                    Wallet = 10000,
                    PayPassword = password,
                };

                _db.User.Add(user);
                _db.Client.Add(client);
                _db.SaveChanges();

                return client;
            }
            return null;
        }
        
        /// <summary>
        /// 是否重名
        /// </summary>
        /// <param name="name">账户</param>
        /// <returns></returns>
        public bool ReName(string name)
        {
            var user = _db.User.SingleOrDefault(u => u.Account == name);
            return user == null ? false : true;
        }

        /// <summary>
        /// 根据账户Id获取用户
        /// </summary>
        /// <param name="cilentId">账户Id</param>
        /// <returns></returns>
        public DataBase.Client GetClientById(int cilentId)
        {
            var clients = GetAllClient();
            DataBase.Client client = clients.SingleOrDefault(c => c.ClientId == cilentId);
            return client;
        }

        /// <summary>
        /// 根据账户获取用户
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        public DataBase.Client GetClientByAccount(string account)
        {
            var clients = GetAllClient();
            DataBase.Client client = clients.SingleOrDefault(c => c.User.Account == account);
            return client;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<DataBase.Client> GetAllClient()
        {
            return _db.Client.Include("User").ToList();
        }

        /// <summary>
        /// 根据地址Id获取地址
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
        public DeliveryInfo GetDeliveryInfoById(int id)
        {
            DeliveryInfo deliveryInfo= _db.DeliveryInfo.SingleOrDefault(d => d.Id == id);
            return deliveryInfo;
        }

        /// <summary>
        /// 根据顾客Id获取所有地址
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <returns></returns>
        public List<DeliveryInfo> GetAllDeliveryInfoByClientId(int clientId)
        {
            var deliveryInfos = _db.DeliveryInfo.Where(d => d.ClientId == clientId).ToList();
            return deliveryInfos;
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="user"></param>
        public void ModifyUserInfo(User user,int clientId)
        {
            DataBase.Client client = GetClientById(clientId);
            user.UserId = client.UserId;

            _db.User.Remove(client.User);
            _db.User.Add(user);

            _db.SaveChanges();
        }

        /// <summary>
        /// 新建收货信息
        /// </summary>
        /// <param name="deliverInfo"></param>
        public void CreatDeliverInfo(int clientId,string address,string contact,string phone,string zip = " ")
        {
            DataBase.Client client = GetClientById(clientId);
            DeliveryInfo deliveryInfo = new DeliveryInfo
            {
                ClientId = client.ClientId,
                DetailedAddress = address,
                PhoneNumber = phone,
                Consignee = contact,
            };
            _db.DeliveryInfo.Add(deliveryInfo);
            _db.SaveChanges();
        }

        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="newDeliverInfo"></param>
        public void ModifyDeliverInfo(int deliveryInfoId, string address, string contact, string phone, string zip = " ")
        {
            DeliveryInfo deliveryInfo = GetDeliveryInfoById(deliveryInfoId);

            deliveryInfo.DetailedAddress = address;
            deliveryInfo.Consignee = contact;
            deliveryInfo.PhoneNumber = phone;

            _db.SaveChanges();
        }

        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="clientId">用户Id</param>
        /// <param name="newPassword">新登录密码</param>
        public void ModifyPasswordByClientId(int clientId, string newPassword)
        {
            DataBase.Client client = GetClientById(clientId);
            client.User.Password = newPassword;

            _db.SaveChanges();
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="clientId">客户Id</param>
        /// <param name="newPayPassword">新支付密码</param>
        public void ModifyPayPasswordByClientId(int clientId,string newPayPassword)
        {
            DataBase.Client client = GetClientById(clientId);
            client.PayPassword = newPayPassword;

            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
