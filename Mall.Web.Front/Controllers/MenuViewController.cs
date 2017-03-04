using Mall.Service.Models;
using Mall.Service.Services.Custom;
using Mall.Web.Front.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class MenuViewController : Controller
    {
        MenuViewService _menuViewService = new MenuViewService();
        // GET: MenuView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SideBarMenu()
        {
            List<MenuViewModel> trees = GetTree();
            return PartialView(trees);
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private List<MenuViewModel> GetTree()
        {
            List<CustomMenu> menus = MenuViewService._menuView;
            List<MenuViewModel> trees = new List<MenuViewModel>();
            foreach (var m in menus)
            {
                if (m.ParentId == 0)
                {
                    trees.Add(
                        new MenuViewModel
                        {
                            Id = m.MenuId,
                            Name = m.Name,
                            Url = m.Url,
                            Icon = m.Icon,
                            Childs = GetChildsByParentId(m.MenuId),
                        }
                    );
                }
            }
            return trees;
        }

        /// <summary>
        /// 递归创建树
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<MenuViewModel> GetChildsByParentId(int parentId)
        {
            List<CustomMenu> menus = MenuViewService._menuView;
            List<MenuViewModel> trees = new List<MenuViewModel>();
            foreach (var m in menus)
            {
                if (m.ParentId == parentId)
                {
                    trees.Add(
                        new MenuViewModel
                        {
                            Id = m.MenuId,
                            Name = m.Name,
                            Url = m.Url,
                            Icon = m.Icon,
                            Childs = GetChildsByParentId(m.MenuId),
                        }
                    );
                }
            }
            return trees;
        }
    }
}