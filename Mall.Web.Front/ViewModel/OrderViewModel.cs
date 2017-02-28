using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    public class OrderViewModel
    {
        public System.Guid OrderId { get; set; }
        public int GoodsId { get; set; }
        public int CustomId { get; set; }
        public string GoodsName { get; set; }
        public double Price { get; set; }
        public double Freight { get; set; }
        public int Count { get; set; }
        public double Totla { get; set; }
        public string Consignee { get; set; }
        public string PhoneNumber { get; set; }
        public string DeliveryAddress { get; set; }
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public bool IsDelete { get; set; }
        public string ClientRemark { get; set; }
        public string OrderRemark { get; set; }
    }
}
