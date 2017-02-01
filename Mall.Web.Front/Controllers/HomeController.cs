using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 商城头部
        /// </summary>
        /// <returns></returns>
        public ActionResult MallHead()
        {
            return PartialView();
        }

        /// <summary>
        /// 轮播
        /// </summary>
        /// <returns></returns>
        public ActionResult Carousel()
        {
            List<GoodsInfo> carousels = new List<GoodsInfo>();
            return PartialView(carousels);
        }

        /// <summary>
        /// 广告位
        /// </summary>
        /// <returns></returns>
        public ActionResult Advertisement()
        {
            return PartialView();
        }

        /// <summary>
        /// 商城推荐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SuggestById(int id)
        {
            List<GoodsInfo> suggests = new List<GoodsInfo>();
            return PartialView(suggests);
        }
    }
}