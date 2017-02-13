//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mall.Data.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoodsInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoodsInfo()
        {
            this.Comment = new HashSet<Comment>();
            this.Image = new HashSet<Image>();
            this.ShoppingCart = new HashSet<ShoppingCart>();
        }
    
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public int CommentNumber { get; set; }
        public int State { get; set; }
        public System.DateTime AddTime { get; set; }
        public Nullable<System.DateTime> ShelfTime { get; set; }
        public Nullable<System.DateTime> UnderShelfTime { get; set; }
        public bool IsDelete { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Image { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}
