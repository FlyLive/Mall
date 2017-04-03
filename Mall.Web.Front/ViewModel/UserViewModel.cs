using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public string Birthday { get; set; }
        public string CreateTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string RealName { get; set; }
    }
}
