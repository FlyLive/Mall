using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mall.Web.Back.ViewModel
{
    public partial class RolesViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Has { get; set; }
        public bool IsDefault { get; set; }
        public string Details { get; set; }
    }
}
