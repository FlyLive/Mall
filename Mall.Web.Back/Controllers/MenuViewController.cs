﻿using Mall.Data.DataBase;
using Mall.Data.Services;
using Mall.Data.Services.Enterprise;
using Mall.Web.Back.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class MenuViewController : Controller
    {
        private MenuViewService _ACService = new MenuViewService();

        public ActionResult SideBarMenuByPermissions()
        {
            Employee employee = (Employee)Session["Employee"];
            List<MenuViewModel> trees = GetTreeByEmployeeId(employee.UserId);
            return PartialView(trees);
        }
        /// <summary>
        /// 根据EmployeeId创建树
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private List<MenuViewModel> GetTreeByEmployeeId(int employeeId)
        {
            List<Menus> menus = _ACService.GetMenuByEmployeeId(employeeId);
            List<MenuViewModel> trees = new List<MenuViewModel>();
            foreach (var m in menus)
            {
                if (m.ParentId == 0)
                {
                    trees.Add(
                        new MenuViewModel
                        {
                            Id = m.MenuId,
                            Name = m.MenuName,
                            Url = m.URL,
                            Path = m.MenuPath,
                            Childs = GetChildsByParentId(employeeId, m.MenuId),
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
        private List<MenuViewModel> GetChildsByParentId(int employeeId, int parentId)
        {
            List<Menus> menus = _ACService.GetMenuByEmployeeId(employeeId);
            List<MenuViewModel> trees = new List<MenuViewModel>();
            foreach (var m in menus)
            {
                if (m.ParentId == parentId)
                {
                    trees.Add(
                        new MenuViewModel
                        {
                            Id = m.MenuId,
                            Name = m.MenuName,
                            Url = m.URL,
                            Path = m.MenuPath,
                            Childs = GetChildsByParentId(employeeId, m.MenuId),
                        }
                    );
                }
            }
            return trees;
        }
    }
}