using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    public class GoodsInfoViewModel
    {
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Details { get; set; }
        public string Category { get; set; }
        public int CommentNumber { get; set; }
        public int State { get; set; }
        public string CreateTime { get; set; }
        public string ShelfTime { get; set; }
        public string UnderShelfTime { get; set; }
        public bool IsDelete { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public string PublicationDate { get; set; }
        public double freight { get; set; }
    }
}
