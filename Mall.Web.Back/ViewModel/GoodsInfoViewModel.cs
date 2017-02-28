using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Back.ViewModel
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
        public DateTime CreateTime { get; set; }
        public DateTime ShelfTime { get; set; }
        public DateTime UnderShelfTime { get; set; }
        public bool IsDelete { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public DateTime PublicationDate { get; set; }
        public double freight { get; set; }
    }
}
