using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    public class ShoppingCartViewModel
    {
        public int ShoppingCartId { get; set; }
        public int ClientId { get; set; }
        public DateTime CreateTime { get; set; }
        public int Number { get; set; }
        public int GoodsId { get; set; }
    }
}
