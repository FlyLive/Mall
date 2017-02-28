using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mall.Web.Back.ViewModel
{
    public partial class RolesViewModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Default { get; set; }
    }
}
