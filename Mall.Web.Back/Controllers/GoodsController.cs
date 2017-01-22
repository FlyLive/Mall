using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class GoodsController : Controller
    {
        #region 商品管理
        public ActionResult CreateGoods()
        {
            return View();
        }

        public ActionResult GoodsEdit()
        {
            return View();
        }
        #endregion
    }
}