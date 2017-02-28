using Mall.Service.DataBase;
using Mall.Service.Services.Enterprise;
using Mall.Web.Front.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Front.Controllers
{
    public class HomeController : Controller
    {
        private GoodsService _goodService = new GoodsService();
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
            List<GoodsInfoViewModel> carousels = new List<GoodsInfoViewModel>();
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
        public ActionResult Suggest()
        {
            List<GoodsInfoViewModel> suggests = _goodService
                .GetRandomGoodsTop5().Select(g => new GoodsInfoViewModel
                {
                    GoodsId = g.GoodsId,
                    GoodsName = g.GoodsName,
                    Price = g.Price,
                    Stock = g.Stock,
                    Details = g.Details,
                    Category = g.Category,
                    CommentNumber = g.CommentNumber,
                    State = g.State,
                    CreateTime = g.CreateTime,
                    ShelfTime = (DateTime)g.ShelfTime,
                    UnderShelfTime = (DateTime)g.UnderShelfTime,
                    IsDelete = g.IsDelete,
                    Author = g.Author,
                    Press = g.Press,
                    PublicationDate = (DateTime)g.PublicationDate,
                    freight = g.freight,
                }).ToList();
            return PartialView(suggests);
        }

        public ActionResult NewGoods()
        {
            List<GoodsInfoViewModel> newGoods = _goodService
                .GetNewGoodsTop5().Select(g => new GoodsInfoViewModel
                {
                    GoodsId = g.GoodsId,
                    GoodsName = g.GoodsName,
                    Price = g.Price,
                    Stock = g.Stock,
                    Details = g.Details,
                    Category = g.Category,
                    CommentNumber = g.CommentNumber,
                    State = g.State,
                    CreateTime = g.CreateTime,
                    ShelfTime = (DateTime)g.ShelfTime,
                    UnderShelfTime = (DateTime)g.UnderShelfTime,
                    IsDelete = g.IsDelete,
                    Author = g.Author,
                    Press = g.Press,
                    PublicationDate = (DateTime)g.PublicationDate,
                    freight = g.freight,
                }).ToList();
            return PartialView(newGoods);
        }

        public ActionResult HotSale()
        {
            List<GoodsInfoViewModel> newGoods = _goodService
                .GetHotSaleGoodsTop5().Select(g => new GoodsInfoViewModel
                {
                    GoodsId = g.GoodsId,
                    GoodsName = g.GoodsName,
                    Price = g.Price,
                    Stock = g.Stock,
                    Details = g.Details,
                    Category = g.Category,
                    CommentNumber = g.CommentNumber,
                    State = g.State,
                    CreateTime = g.CreateTime,
                    ShelfTime = (DateTime)g.ShelfTime,
                    UnderShelfTime = (DateTime)g.UnderShelfTime,
                    IsDelete = g.IsDelete,
                    Author = g.Author,
                    Press = g.Press,
                    PublicationDate = (DateTime)g.PublicationDate,
                    freight = g.freight,
                }).ToList();
            return PartialView(newGoods);
        }
    }
}