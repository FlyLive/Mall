using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Front.ViewModel
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public int ClientId { get; set; }
        public int GoodsId { get; set; }
        public string CommentDetail { get; set; }
        public DateTime CommentTime { get; set; }
        public int Rank { get; set; }
        public int ParentId { get; set; }
    }
}
