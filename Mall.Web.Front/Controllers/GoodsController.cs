using Mall.Service.DataBase;
using Mall.Web.Front.ViewModel;
using PagedList;
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
        public ActionResult Goods(/*int goodsId*/)
        {
            GoodsInfo goods = new GoodsInfo();
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(goods);
            return View(goodsDTO);
        }

        /// <summary>
        /// 商品销售信息
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public ActionResult GoodsSaleInfo(int goodsId)
        {
            GoodsInfo goods = new GoodsInfo();
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(goods);
            return PartialView(goodsDTO);
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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult SearchResult(int page = 1,int pageSize = 10)
        {
            List<GoodsInfo> goods = new List<GoodsInfo>();
            List<int> nums = new List<int>();
            for(int i = 0;i < 1000; i++)
            {
                nums.Add(i);
            }
            IPagedList<int> num = nums.ToPagedList(page,pageSize);
            return View(num);
        }

        public static GoodsInfoViewModel DataGoodToDTO(GoodsInfo g)
        {
            GoodsInfoViewModel goodDTO = new GoodsInfoViewModel
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
            };
            return goodDTO;
        }
    }
}