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
    
    public partial class Comment
    {
        public int CommentId { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> GoodsId { get; set; }
        public string CommentDetail { get; set; }
        public System.DateTime CommentTime { get; set; }
        public int Rank { get; set; }
        public int ParentId { get; set; }
    
        public virtual GoodsInfo GoodsInfo { get; set; }
        public virtual Client Client { get; set; }
    }
}
