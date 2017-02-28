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
            GoodsInfoViewModel goodsDTO = new GoodsInfoViewModel
            {
                GoodsId = goods.GoodsId,
                GoodsName = goods.GoodsName,
                Price = goods.Price,
                Stock = goods.Stock,
                Details = goods.Details,
                Category = goods.Category,
                CommentNumber = goods.CommentNumber,
                State = goods.State,
                CreateTime = goods.CreateTime,
                ShelfTime = (DateTime)goods.ShelfTime,
                UnderShelfTime = (DateTime)goods.UnderShelfTime,
                IsDelete = goods.IsDelete,
                Author = goods.Author,
                Press = goods.Press,
                PublicationDate = (DateTime)goods.PublicationDate,
                freight = goods.freight,
            };
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
            GoodsInfoViewModel goodsDTO = new GoodsInfoViewModel
            {
                GoodsId = goods.GoodsId,
                GoodsName = goods.GoodsName,
                Price = goods.Price,
                Stock = goods.Stock,
                Details = goods.Details,
                Category = goods.Category,
                CommentNumber = goods.CommentNumber,
                State = goods.State,
                CreateTime = goods.CreateTime,
                ShelfTime = (DateTime)goods.ShelfTime,
                UnderShelfTime = (DateTime)goods.UnderShelfTime,
                IsDelete = goods.IsDelete,
                Author = goods.Author,
                Press = goods.Press,
                PublicationDate = (DateTime)goods.PublicationDate,
                freight = goods.freight,
            };
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
    }
}