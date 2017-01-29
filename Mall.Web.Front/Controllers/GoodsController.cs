using Mall.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class GoodsController : Controller
    {
        /// <summary>
        /// 商品详情页
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult Goods()
        {
            return View();
        }

        /// <summary>
        /// 商品销售信息
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public ActionResult GoodsSaleInfo(int goodsId)
        {
            return PartialView();
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult GoodsDetails(int goodsId)
        {
            return PartialView();
        }

        /// <summary>
        /// 商品评价
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult GoodsEvaluates(int goodsId)
        {
            return PartialView();
        }

        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult SearchResult(string search)
        {
            List<GoodsInfo> goods = new List<GoodsInfo>();
            return View(goods);
        }
    }
}