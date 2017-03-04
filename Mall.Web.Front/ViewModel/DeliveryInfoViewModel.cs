using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    public class DeliveryInfoViewModel
    {
        public int CustomId { get; set; }
        public int Id { get; set; }
        public string Consignee { get; set; }
        public string DetailedAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDefault { get; set; }
    }
}
