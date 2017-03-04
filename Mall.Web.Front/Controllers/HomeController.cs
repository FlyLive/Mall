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
        /// <returns></returns>
        public ActionResult Suggest()
        {
            List<GoodsInfo> suggests = _goodService.GetRandomGoodsTop5();
            List < GoodsInfoViewModel > suggestsDTO = DataGoodsToDTO(suggests);
            return PartialView(suggestsDTO);
        }

        public ActionResult NewGoods()
        {
            List<GoodsInfo> goods = _goodService.GetNewGoodsTop5();
            List < GoodsInfoViewModel > goodsDTO = DataGoodsToDTO(goods);
            return PartialView(goodsDTO);
        }

        public ActionResult HotSale()
        {
            List<GoodsInfo> goods = _goodService.GetHotSaleGoodsTop5();
            List<GoodsInfoViewModel> goodsDTO = DataGoodsToDTO(goods);
            return PartialView(goodsDTO);
        }

        public static List<GoodsInfoViewModel> DataGoodsToDTO(List<GoodsInfo> goods)
        {
            List<GoodsInfoViewModel> goodsDTO = goods
                .Select(g => new GoodsInfoViewModel
                {
                    GoodsId = g.GoodsId,
                    GoodsName = g.GoodsName,
                    Price = g.Price,
                    Stock = g.Stock,
                    Details = g.Details,
                    Category = g.Category,
                    CommentNumber = g.CommentNumber,
                    State = g.State,
                    CreateTime = g.CreateTime.ToString("yyyy-MM-dd HH-mm-ss"),
                    ShelfTime = g.ShelfTime == null ? g.ShelfTime.ToString() : "0000-00-00 00-00-00",
                    UnderShelfTime = g.UnderShelfTime == null ? g.UnderShelfTime.ToString() : "0000-00-00 00-00-00",
                    IsDelete = g.IsDelete,
                    Author = g.Author,
                    Press = g.Press,
                    PublicationDate = g.PublicationDate == null ? g.PublicationDate.ToString() : "0000-00-00 00-00-00",
                    freight = g.freight,
                }).ToList();
            return goodsDTO;
        }
    }
}