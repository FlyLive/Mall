using System;
using Mall.Service.DataBase;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Services
{
    public class UserService
    {
        private MallDBContext _db;
        
        public UserService()
        {
            _db = new MallDBContext();
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="realName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="nick"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        public bool ModifyInfo(int userId, string realName, string phone, string email, DateTime? birthday, string nick, int gender)
        {
            try
            {
                User user = _db.User.SingleOrDefault(u => u.UserId == userId);

                user.RealName = realName;
                user.PhoneNumber = phone;
                user.Email = email;
                user.NickName = nick;
                user.Gender = gender == 1 ? true : false;
                user.Birthday = birthday;

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
        /// <param name="userId"></param>
        /// <param name="imgBase"></param>
        public string ModifyPhoto(int userId, string imgBase)
        {
            try
            {
                User user = _db.User.SingleOrDefault(u => u.UserId == userId);

                var img = imgBase.Split(',');
                byte[] bt = Convert.FromBase64String(img[1]);
                string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                string path = "D:/网站部署/MallImg/Mall.Web.Back/Users/Avatar/avatar" + now + ".png";
                string DataPath = "http://localhost:9826/Mall.Web.Back/Users/Avatar/avatar" + now + ".png";
                File.WriteAllBytes(path, bt);
                user.Photo = DataPath;
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
        /// 修改登录密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newPassword">新登录密码</param>
        public bool ModifyPasswordByUserId(int userId, string newPassword)
        {
            try
            {
                User user = _db.User.SingleOrDefault(u => u.UserId == userId);
                user.Password = newPassword;

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
        /// 是否重名
        /// </summary>
        /// <param name="name">账户</param>
        /// <returns></returns>
        public bool ReName(string name)
        {
            var user = _db.User.SingleOrDefault(u => u.Account == name);
            return user == null ? false : true;
        }
    }
}
