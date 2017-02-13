using Mall.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Services.Client
{
    public class MenuViewService
    {
        public static List<ClientMenuViewModel> _menuView { get; } = new List<ClientMenuViewModel>
        {
            new ClientMenuViewModel { MenuId = 1,Name = "个人中心",Url = "#",ParentId = 0 },
            new ClientMenuViewModel { MenuId = 2,Name = "订单管理",Url = "#",ParentId = 0 },
            new ClientMenuViewModel { MenuId = 3,Name = "修改资料",Url = "/Account/PersonalInfo",ParentId = 1 },
            new ClientMenuViewModel { MenuId = 4,Name = "地址管理",Url = "/Account/AddressSet",ParentId = 1 },
            new ClientMenuViewModel { MenuId = 5,Name = "安全设置",Url = "/Account/SecuritySet",ParentId = 1 },
            new ClientMenuViewModel { MenuId = 7,Name = "全部订单",Url = "/Order",ParentId = 2 },
            new ClientMenuViewModel { MenuId = 8,Name = "待付款",Url = "/Order",ParentId = 2 },
            new ClientMenuViewModel { MenuId = 9,Name = "待发货",Url = "/Order",ParentId = 2 },
            new ClientMenuViewModel { MenuId = 10,Name = "待收货",Url = "/Order",ParentId = 2 },
            new ClientMenuViewModel { MenuId = 11,Name = "待评价",Url = "/Order",ParentId = 2 },
            new ClientMenuViewModel { MenuId = 12,Name = "退款",Url = "/Order",ParentId = 2 }
        };
        public MenuViewService(){}
    }
}
