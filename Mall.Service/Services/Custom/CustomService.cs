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
                    Photo = "http://localhost:9826/Mall.Web.Front/Users/Avatar/avatar.png",
                    CreateTime = DateTime.Now,
                    RealName = name,
                };

                DataBase.Custom custom = new DataBase.Custom
                {
                    UserId = user.UserId,
                    Wallet = 200,
                    PayPassword = password,
                };

                _db.User.Add(user);
                _db.Custom.Add(custom);
                _db.SaveChanges();

                return custom;
            }
            return null;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
            return _db.Custom.Include("User").Include("ShoppingCart").ToList();
        }

        /// <summary>
        /// 根据地址Id获取地址
        /// </summary>
        /// <param name="id">地址Id</param>
        /// <returns></returns>
        public DeliveryInfo GetDeliveryInfoById(int customId, int? id)
        {
            DeliveryInfo deliveryInfo = null;
            if (id == null)
            {
                var deliveryInfos = GetAllDeliveryInfoByCustomId(customId);
                deliveryInfo = deliveryInfos.SingleOrDefault(d => d.IsDefault == true);
                if (deliveryInfo == null)
                {
                    deliveryInfo = deliveryInfos[0];
                }
            }
            else
            {
                deliveryInfo = _db.DeliveryInfo.SingleOrDefault(d => d.Id == id);
            }
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
        public bool ModifyUserInfo(int customId, string email, DateTime? birthday, string nick, string name, string phone, int gender = 1)
        {
            try
            {
                User user = GetCustomByCustomId(customId).User;

                user.Email = email;
                user.Birthday = birthday == null ? user.Birthday : birthday;
                user.NickName = nick;
                user.RealName = name;
                user.PhoneNumber = phone;
                user.Gender = gender == 1 ? true : false;

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="imgBase"></param>
        public string ModifyPhoto(int customId, string imgBase)
        {
            try
            {
                DataBase.Custom custom = GetCustomByCustomId(customId);
                var img = imgBase.Split(',');
                byte[] bt = Convert.FromBase64String(img[1]);
                string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                string path = "D:/网站部署/MallImg/Mall.Web.Front/Users/Avatar/avatar" + now + ".png";
                string DataPath = "http://localhost:9826/Mall.Web.Front/Users/Avatar/avatar" + now + ".png";
                File.WriteAllBytes(path, bt);
                custom.User.Photo = DataPath;
                _db.SaveChanges();
                return DataPath;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return null;
            }
        }

        /// <summary>
        /// 新建收货信息
        /// </summary>
        /// <param name="deliverInfo"></param>
        public bool CreatDeliverInfo(int customId, string address, string contact, string phone, string zip = " ")
        {
            try
            {
                DataBase.Custom custom = GetCustomByCustomId(customId);
                if (custom.DeliveryInfo.Count < custom.MaxAddressNumber)
                {
                    DeliveryInfo deliveryInfo = new DeliveryInfo
                    {
                        CustomId = custom.CustomId,
                        DetailedAddress = address,
                        PhoneNumber = phone,
                        Consignee = contact,
                        Zip = zip,
                        IsDefault = false,
                    };
                    _db.DeliveryInfo.Add(deliveryInfo);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 收货地址是否达到上限
        /// </summary>
        /// <param name="customId"></param>
        /// <returns></returns>
        public bool IsDeliveryFull(int customId)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            var result = custom.DeliveryInfo.Count == custom.MaxAddressNumber;
            return result;
        }

        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="newDeliverInfo"></param>
        public bool ModifyDeliverInfo(int customId, int deliveryInfoId, string address, string contact, string phone, string zip)
        {
            try
            {
                DeliveryInfo deliveryInfo = GetDeliveryInfoById(customId, deliveryInfoId);

                deliveryInfo.DetailedAddress = address == "" ? deliveryInfo.DetailedAddress : address;
                deliveryInfo.Consignee = contact == "" ? deliveryInfo.Consignee : contact;
                deliveryInfo.PhoneNumber = phone == "" ? deliveryInfo.PhoneNumber : phone;
                deliveryInfo.Zip = zip == "" ? deliveryInfo.Zip : zip;

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 删除收货地址
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="deliveryId"></param>
        public bool DeletDeliveryByDeliveryId(int customId, int deliveryId)
        {
            try
            {
                DeliveryInfo delivery = GetAllDeliveryInfoByCustomId(customId).SingleOrDefault(d => d.CustomId == customId && d.Id == deliveryId);
                _db.DeliveryInfo.Remove(delivery);

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 修改默认地址
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="deliveryId"></param>
        public bool SetDefaultAddressOfCustomByDeliveryId(int customId, int deliveryId)
        {
            try
            {
                var defaultAddress = GetCustomByCustomId(customId).DeliveryInfo.SingleOrDefault(d => d.IsDefault == true);
                if (defaultAddress != null)
                {
                    defaultAddress.IsDefault = false;
                }
                _db.DeliveryInfo.SingleOrDefault(d => d.Id == deliveryId).IsDefault = true;

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="customId">用户Id</param>
        /// <param name="newPassword">新登录密码</param>
        public bool ModifyPasswordByCustomId(int customId, string newPassword)
        {
            try
            {
                DataBase.Custom custom = GetCustomByCustomId(customId);
                custom.User.Password = newPassword;

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="customId">客户Id</param>
        /// <param name="newPayPassword">新支付密码</param>
        public bool ModifyPayPasswordByCustomId(int customId, string newPayPassword)
        {
            try
            {
                DataBase.Custom custom = GetCustomByCustomId(customId);
                custom.PayPassword = newPayPassword;

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 返回客户的购物车内商品总价
        /// </summary>
        /// <param name="customId"></param>
        /// <returns></returns>
        public double GetTotlaMoneyById(int customId, int[] goodsIds)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            double totleMoney = 0;
            List<ShoppingCart> cart = custom.ShoppingCart
                .Where(c => goodsIds.Any(gi => gi == c.GoodsId)).ToList();

            cart.ForEach(c => totleMoney += c.Number * c.GoodsInfo.Price + c.GoodsInfo.Freight);

            return totleMoney;
        }

        /// <summary>
        /// 返回客户的购物车内选中商品的总数量
        /// </summary>
        /// <param name="customId"></param>
        /// <returns></returns>
        public int GetCartNumberByCustomId(int customId)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            List<ShoppingCart> cart = custom.ShoppingCart.ToList();
            return cart.Count;
        }

        /// <summary>
        /// 返回客户的购物车
        /// </summary>
        /// <param name="customId"></param>
        /// <returns></returns>
        public List<ShoppingCart> GetCartByCustomId(int customId)
        {
            DataBase.Custom custom = GetCustomByCustomId(customId);
            List<ShoppingCart> cart = custom.ShoppingCart.ToList();
            return cart;
        }

        /// <summary>
        /// 向购物车添加商品
        /// </summary>
        /// <param name="customId">客户ID</param>
        /// <param name="goodsId">商品ID</param>
        /// <param name="count">数量</param>
        public bool AddGoodsToShoppingCart(int customId, int goodsId, int count = 1)
        {
            try
            {
                DataBase.Custom custom = _db.Custom.Include("ShoppingCart").SingleOrDefault(c => c.CustomId == customId);
                List<ShoppingCart> carts = custom.ShoppingCart.ToList();
                ShoppingCart newCart = carts.SingleOrDefault(c => c.GoodsId == goodsId);

                if (newCart != null)
                {
                    ModifyGoodsCountFromShoppingCart(customId, goodsId, count + newCart.Number);
                }
                else
                {
                    custom.ShoppingCart.Add(new ShoppingCart
                    {
                        GoodsId = goodsId,
                        CreateTime = DateTime.Now,
                        Number = count,
                    });
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 修改购物车内数量
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="goodsId"></param>
        /// <param name="count"></param>
        public bool ModifyGoodsCountFromShoppingCart(int customId, int goodsId, int count)
        {
            try
            {
            DataBase.Custom custom = _db.Custom.Include("ShoppingCart").SingleOrDefault(c => c.CustomId == customId);
            custom.ShoppingCart.SingleOrDefault(s => s.GoodsId == goodsId).Number = count;
            _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 删除购物车中商品
        /// </summary>
        /// <param name="cilentId"></param>
        /// <param name="goodsId"></param>
        public bool DeleteGoodsFromShoppingCart(int customId, int goodsId)
        {
            try
            {
            DataBase.Custom custom = _db.Custom.Include("ShoppingCart").SingleOrDefault(c => c.CustomId == customId);
            _db.ShoppingCart.Remove(
                custom.ShoppingCart.
                    SingleOrDefault(s => s.GoodsId == goodsId)
            );
            _db.SaveChanges();
            return true;
        }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
}

        /// <summary>
        /// 返回用户指定商品的购物车
        /// </summary>
        /// <param name="customId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<ShoppingCart> GetCartByCustomIdAndGoodsId(int customId, int[] goodsId)
        {
            List<ShoppingCart> carts = _db.ShoppingCart
                .Include("GoodsInfo")
                .Where(s => s.CustomId == customId)
                .Where(s => goodsId.Any(g => g == s.GoodsId))
                .ToList();
            return carts;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
