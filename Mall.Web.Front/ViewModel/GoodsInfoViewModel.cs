using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoodsInfoViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GoodsInfoViewModel()
        {
            this.Comment = new HashSet<CommentViewModel>();
            this.Image = new HashSet<ImageViewModel>();
            this.Order = new HashSet<OrderViewModel>();
            this.ShoppingCart = new HashSet<ShoppingCartViewModel>();
        }
    
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public int CommentNumber { get; set; }
        public int State { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> ShelfTime { get; set; }
        public Nullable<System.DateTime> UnderShelfTime { get; set; }
        public bool IsDelete { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public Nullable<System.DateTime> PublicationDate { get; set; }
        public double freight { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentViewModel> Comment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImageViewModel> Image { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderViewModel> Order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ShoppingCartViewModel> ShoppingCart { get; set; }
    }
}
