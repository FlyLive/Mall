using Mall.Service.DataBase;
using Mall.Web.Front.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mall.Service.Services;

namespace Mall.Web.Front.Controllers
{
    public class GoodsController : Controller
    {
        private GoodsService _goodsService = new GoodsService();

        /// <summary>
        /// 商品详情页
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Goods(int goodsId)
        {
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
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
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
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
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(goods);
            return PartialView(goodsDTO);
        }

        public ActionResult GoodsImgs(int goodsId)
        {
            List<Image> images = _goodsService.GetImgsByGoodsId(goodsId);
            List<ImageViewModel> imagesDTO = images.Select(i => new ImageViewModel
            {
                ImageId = i.ImageId,
                ImageSrc = i.ImageSrc,
                GoodsId = i.GoodsId,
            }).ToList();
            return PartialView(imagesDTO);
        }

        public ActionResult GoodsCarousel(int goodsId)
        {
            List<Image> images = _goodsService.GetImgsByGoodsId(goodsId);
            List<ImageViewModel> imagesDTO = images.Select(i => new ImageViewModel
            {
                ImageId = i.ImageId,
                ImageSrc = i.ImageSrc,
                GoodsId = i.GoodsId,
            }).ToList();
            return PartialView(imagesDTO);
        }

        /// <summary>
        /// 商品评价
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public ActionResult GoodsEvaluates(int goodsId)
        {
            List<Comment> comments = _goodsService.GetGoodsCommentsByGoodsId(goodsId);
            List<CommentViewModel> commentsDTO = comments.Select(c => new CommentViewModel
            {
                CommentId = c.CommentId,
                CustomId = (int)c.CustomId,
                GoodsId = (int)c.GoodsId,
                CommentDetail = c.CommentDetail,
                Reply = c.Reply,
                CommentTime = c.CommentTime,
                PhotoUrl = c.Custom.User.Photo,
                CustomNick = c.Custom.User.NickName
            }).ToList();
            return PartialView(commentsDTO);
        }

        [HttpGet]
        public ActionResult SimpleGoodsInfo(int goodsId)
        {
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(goods);
            return PartialView(goodsDTO);
        }

        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search(string searchName = "")
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods()
                .Where(g => g.GoodsName.Contains(searchName)).ToList();
            List<GoodsInfoViewModel> goodsDTO = new List<GoodsInfoViewModel>();
            goods.ForEach(g => goodsDTO.Add(DataGoodToDTO(g)));;

            return View(goodsDTO);
        }

        public static GoodsInfoViewModel DataGoodToDTO(GoodsInfo g)
        {
            GoodsInfoViewModel goodDTO = new GoodsInfoViewModel
            {
                GoodsId = g.GoodsId,
                GoodsName = g.GoodsName,
                GoodsPhotoUrl = g.Image.ElementAt(0).ImageSrc,
                Price = g.Price,
                Stock = g.Stock,
                Details = g.Details,
                SalesNumber = g.SalesNumber,
                CommentNumber = g.CommentNumber,
                State = g.State,
                CreateTime = g.CreateTime.ToString("yyyy-MM-dd HH-mm-ss"),
                ShelfTime = g.ShelfTime == null ? "0000-00-00 00-00-00" : g.ShelfTime.ToString(),
                UnderShelfTime = g.UnderShelfTime == null ? "0000-00-00 00-00-00" : g.UnderShelfTime.ToString(),
                IsDelete = g.IsDelete,
                Author = g.Author,
                Press = g.Press,
                PublicationDate = g.PublicationDate == null ? "0000-00-00" : g.PublicationDate.Value.ToString("yyyy-MM-dd"),
                Freight = g.Freight,
            };
            return goodDTO;
        }
    }
}