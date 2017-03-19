//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mall.Service.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Comment = new HashSet<Comment>();
            this.Refund = new HashSet<Refund>();
        }
    
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
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> PaymentTime { get; set; }
        public Nullable<System.DateTime> DeliveryTime { get; set; }
        public Nullable<System.DateTime> ReceiptTime { get; set; }
        public bool IsDelete { get; set; }
        public string CustomRemark { get; set; }
        public string OrderRemark { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual Custom Custom { get; set; }
        public virtual GoodsInfo GoodsInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Refund> Refund { get; set; }
    }
}
