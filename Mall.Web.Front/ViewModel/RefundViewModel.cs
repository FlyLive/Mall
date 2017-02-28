using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
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
