using Mall.Interface.Custom;
using Mall.Service.DataBase;
using Mall.Service.Services.Custom;
using Mall.Service.Services.Enterprise;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Services.Custom
{
    public class CustomService : IDisposable, IUserCustomApplicationService
    {
        private MallDBContext _db;

        public CustomService()
        {
            _db = new MallDBContext();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public DataBase.Custom Login(string name, string password)
        {
            DataBase.Custom custom = GetCustomByAccount(name);
            if (custom != null)
            {
                return custom.User.Password == password ? custom : null;
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
        public DataBase.Custom Registe(string name, string password, string email)
        {
            if (!ReName(name))
            {
                User user = new User
                {
                    Account = name,
                    NickName = "小白",
                    Password = password,
                    Email = email,
                    Photo = "../Pictures/Users/Avatar/avatar.png",
                    CreateTime = DateTime.Now,
                };

                DataBase.Custom custom = new DataBase.Custom
                {
                    UserId = user.UserId,
                    Wallet = 10000,
                    PayPassword = password,
                };

                _db.User.Add(user);
                _db.Custom.Add(custom);
                _db.SaveChanges();

                return custom;
            }
            return null;
        }

        public string SendEmailOfVerifyCode(string email)
        {
            int customerID = 1;
            string verifyCode = Guid.NewGuid().ToString();
            try
            {
                System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("1585213801@qq.com", "云翳商城"); //填写电子邮件地址，和显示名称
                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(email, "客户"); //填写邮件的收件人地址和名称
                //设置好发送地址，和接收地址，接收地址可以是多个
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = from;
                mail.To.Add(to);
                mail.Subject = "验证码";

                StringBuilder strBody = new StringBuilder();
                strBody.Append("请将下面的验证码输入到重置密码的验证处，仅限本次访问有效，验证码只能使用一次，请尽快重置密码！</br>");
                strBody.Append("<h3>验证码:&emsp;" + verifyCode + "</h3>");
                strBody.Append("<a href='http://localhost:31061/Order/ActivePage?customerID=" + customerID + "&valiDataCode =" + verifyCode + "'>点击这里</a></br>");

                mail.Body = strBody.ToString();
                mail.IsBodyHtml = true;//设置显示htmls

                //设置好发送邮件服务地址
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = "smtp.qq.com";
                
                //填写服务器地址相关的用户名和密码信息
                client.Credentials = new System.Net.NetworkCredential("1585213801@qq.com", "rsxtdoqaeknhfiij");
                client.EnableSsl = true;
                
                //发送邮件
                client.Send(mail);
                return verifyCode;
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return string.Empty;
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
        /// 验证用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool CustomConfirm(string account, string email)
        {
            var custom = GetCustomByAccount(account);
            return custom.User.Email.Equals(email);
        }

        /// <summary>
        /// 根据账户Id获取用户
        /// </summary>
        /// <param name="customId">账户Id</param>
        /// <returns></returns>
        public User GetUserByCustomId(int customId)
        {
            var customs = GetAllCustom();
            DataBase.Custom custom = customs.SingleOrDefault(c => c.CustomId == customId);
            return custom.User;
        }

        /// <summary>
        /// 根据账户Id获取客户
        /// </summary>
        /// <param name="customId">账户Id</param>
        /// <returns></returns>
        public DataBase.Custom GetCustomByCustomId(int customId)
        {
            var customs = GetAllCustom();
            DataBase.Custom custom = customs.SingleOrDefault(c => c.CustomId == customId);
            return custom;
        }

        /// <summary>
        /// 根据账户获取客户
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        public DataBase.Custom GetCustomByAccount(string account)
        {
            var customs = GetAllCustom();
            DataBase.Custom custom = customs.SingleOrDefault(c => c.User.Account == account);
            return custom;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public List<DataBase.Custom> GetAllCustom()
        {
            return _db.Custom.Include("User").ToList();
        }

        /// <summary>
        /// 根据地址Id获取地址
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
        public DeliveryInfo GetDeliveryInfoById(int id)
        {
            DeliveryInfo deliveryInfo = _db.DeliveryInfo.SingleOrDefault(d => d.Id == id);
            return deliveryInfo;
        }

        /// <summary>
        /// 根据顾客Id获取所有地址
        /// </summary>
        /// <param name="customId">客户Id</param>
        /// <returns></returns>
        public List<DeliveryInfo> GetAllDeliveryInfoByCustomId(int customId)
        {
            var deliveryInfos = _db.DeliveryInfo.Where(d => d.CustomId == customId).ToList();
            return deliveryInfos;
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="user"></param>
        public void ModifyUserInfo(int customId, string email, DateTime? birthday, string nick, string name, string phone, int gender = 1)
        {
            User user = GetCustomByCustomId(customId).User;

            user.Email = email;
            user.Birthday = birthday == null ? user.Birthday : birthday;
            user.NickName = nick;
            user.RealName = name;
            user.PhoneNumber = phone;
            user.Gender = gender == 1 ? true : false;

            _db.SaveChanges();
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="imgBase"></param>
        public string ModifyPhoto(int customId, string imgBase)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);

            try
            {
                var img = imgBase.Split(',');
                byte[] bt = Convert.FromBase64String(img[1]);
                string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                string path = "C:/Users/LL/Documents/Visual Studio 2015/Projects/Mall/Mall.Web.Front/Pictures/Users/Avatar/avatar" + now + ".png";
                string DataPath = "../Pictures/Users/Avatar/avatar" + now + ".png";
                File.WriteAllBytes(path, bt);
                custom.User.Photo = DataPath;
                _db.SaveChanges();
                return DataPath;
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

        /// <summary>
        /// 新建收货信息
        /// </summary>
        /// <param name="deliverInfo"></param>
        public void CreatDeliverInfo(int customId, string address, string contact, string phone, string zip = " ")
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            DeliveryInfo deliveryInfo = new DeliveryInfo
            {
                CustomId = custom.CustomId,
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
        /// <param name="customId">用户Id</param>
        /// <param name="newPassword">新登录密码</param>
        public void ModifyPasswordByCustomId(int customId, string newPassword)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            custom.User.Password = newPassword;

            _db.SaveChanges();
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="customId">客户Id</param>
        /// <param name="newPayPassword">新支付密码</param>
        public void ModifyPayPasswordByCustomId(int customId, string newPayPassword)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            custom.PayPassword = newPayPassword;

            _db.SaveChanges();
        }
        
        /// <summary>
        /// 向购物车添加商品
        /// </summary>
        /// <param name="customId">客户ID</param>
        /// <param name="goodsId">商品ID</param>
        /// <param name="count">数量</param>
        public void AddGoodsToShoppingCart(int customId, int goodsId, int count = 1)
        {
            DataBase.Custom custom = _db.Custom.Include("ShoppingCart").SingleOrDefault(c => c.CustomId == customId);
            custom.ShoppingCart.Add(new ShoppingCart
            {
                GoodsId = goodsId,
                CreateTime = DateTime.Now,
                Number = count,
            });
            _db.SaveChanges();
        }

        /// <summary>
        /// 修改购物车内数量
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="goodsId"></param>
        /// <param name="count"></param>
        public void ModifyGoodsCountFromShoppingCart(int customId, int goodsId, int count)
        {
            DataBase.Custom custom = _db.Custom.Include("ShoppingCart").SingleOrDefault(c => c.CustomId == customId);
            custom.ShoppingCart.SingleOrDefault(s => s.GoodsId == goodsId).Number = count;
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除购物车中商品
        /// </summary>
        /// <param name="cilentId"></param>
        /// <param name="goodsId"></param>
        public void DeleteGoodsFromShoppingCart(int customId, int goodsId)
        {
            DataBase.Custom custom = _db.Custom.Include("ShoppingCart").SingleOrDefault(c => c.CustomId == customId);
            custom.ShoppingCart.Remove(
                custom.ShoppingCart.
                    SingleOrDefault(s => s.GoodsId == goodsId)
            );
            _db.SaveChanges();
        }

        /// <summary>
        /// 返回用户指定商品的购物车
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ShoppingCart GetCartByCustomIdAndGoodsId(int customId,int goodsId)
        {
            ShoppingCart cart = _db.ShoppingCart.Include("GoodsInfo").SingleOrDefault(c => c.CustomId == customId && c.GoodsId == goodsId);
            return cart;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
