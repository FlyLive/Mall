using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Back.ViewModel
{
    public partial class CommentViewModel
    {
        public int CommentId { get; set; }
        public int CustomId { get; set; }
        public int GoodsId { get; set; }
        public string CommentDetail { get; set; }
        public DateTime CommentTime { get; set; }
        public string Reply { get; set; }
        public Guid OrderId { get; set; }
    }
}
