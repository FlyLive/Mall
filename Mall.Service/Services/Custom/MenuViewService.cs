using Mall.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Services.Custom
{
    public class MenuViewService
    {
        public static List<CustomMenu> _menuView { get; } = new List<CustomMenu>
        {
            new CustomMenu { MenuId = 1,Name = "个人中心",Icon="fa fa-male",Url = "#",ParentId = 0 },
            new CustomMenu { MenuId = 2,Name = "订单管理",Icon="fa fa-reorder",Url = "#",ParentId = 0 },
            new CustomMenu { MenuId = 3,Name = "修改资料",Icon="fa fa-edit",Url = "/Custom/PersonalInfo",ParentId = 1 },
            new CustomMenu { MenuId = 4,Name = "地址管理",Icon="fa fa-map-marker",Url = "/Custom/AddressSet",ParentId = 1 },
            new CustomMenu { MenuId = 5,Name = "安全设置",Icon="fa fa-lock",Url = "/Custom/SecuritySet",ParentId = 1 },
            new CustomMenu { MenuId = 7,Name = "所有订单",Icon="fa fa-reorder",Url = "/Order/AllOrders",ParentId = 2 },
            new CustomMenu { MenuId = 8,Name = "待付款",Icon="fa fa-cny",Url = "/Order",ParentId = 2 },
            new CustomMenu { MenuId = 9,Name = "待发货",Icon="fa fa-paper-plane-o",Url = "/Order",ParentId = 2 },
            new CustomMenu { MenuId = 10,Name = "待收货",Icon="fa fa-pencil-square-o",Url = "/Order",ParentId = 2 },
            new CustomMenu { MenuId = 11,Name = "待评价",Icon="fa fa-comments-o",Url = "/Order",ParentId = 2 },
            new CustomMenu { MenuId = 12,Name = "退款",Icon="fa fa-recycle",Url = "/Order",ParentId = 2 }
        };
        public MenuViewService(){}
    }
}
