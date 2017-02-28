using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mall.Web.Back.ViewModel
{
    public class RefundViewModel
    {
        public int RefundId { get; set; }
        public int ClientId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime DealTime { get; set; }
        public string Remark { get; set; }
    }
}
