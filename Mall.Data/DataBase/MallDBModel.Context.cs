﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MallDBContext : DbContext
    {
        public MallDBContext()
            : base("name=MallDBContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminLog> AdminLog { get; set; }
        public virtual DbSet<Advertisement> Advertisement { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<DeliveryInfo> DeliveryInfo { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<GoodsInfo> GoodsInfo { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<Refund> Refund { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
