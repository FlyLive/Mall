using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mall.Web.Back.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreateTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string RealName { get; set; }
    }
}