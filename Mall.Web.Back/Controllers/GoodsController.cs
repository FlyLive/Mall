using Mall.Service.DataBase;
using Mall.Service.Services.Enterprise;
using Mall.Web.Back.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mall.Web.Back.Controllers
{
    public class GoodsController : Controller
    {
        public GoodsService _goodsService = new GoodsService();
        #region 商品管理
        [HttpGet]
        public ActionResult CreateGoods()
        {
            return View();
        }

        [HttpPost]
        public int CreateGoods(string name, string count, string price,
            string detail, DateTime? publicationDate, string freight = "+0",
            string author = null, string press = null)
        {
            int countNumber = Convert.ToInt16(count.Substring(1));
            double newPrice = Convert.ToDouble(price.Substring(1));
            double newFreight = Convert.ToDouble(freight.Substring(1));

            int result = _goodsService.CreateGoods(name, countNumber, newPrice, detail,
                0, newFreight, publicationDate, author, press);
            return result;
        }

        [HttpPost]
        public bool CreateGoodsImg(int goodsId)
        {
            HttpFileCollection filess = System.Web.HttpContext.Current.Request.Files;

            foreach (var key in filess.AllKeys)
            {
                HttpPostedFile file = filess[key];
                if (string.IsNullOrEmpty(file.FileName) == false)
                {
                    string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                    string path = "localhost:9826/Mall.Web.Goods/Goods/" + now + file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath(path));
                    _goodsService.AddGoodsImage(goodsId,path);
                }
            }
            
            return true;
        }

        [HttpGet]
        public ActionResult GetGoodsSimpleInfo(int goodsId)
        {
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(new List<GoodsInfo> { goods }).ElementAt(0);
            return PartialView(goodsDTO);
        }

        [HttpGet]
        public ActionResult GoodsDetails(int goodsId)
        {
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
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
                CreateTime = goods.CreateTime.ToString("yyyy-MM-dd HH-mm-ss"),
                ShelfTime = goods.ShelfTime == null ? "0000-00-00 00-00-00" : goods.ShelfTime.ToString(),
                UnderShelfTime = goods.UnderShelfTime == null ? "0000-00-00 00-00-00" : goods.UnderShelfTime.ToString(),
                IsDelete = goods.IsDelete,
                Author = goods.Author,
                Press = goods.Press,
                PublicationDate = goods.PublicationDate == null ? "0000-00-00 00-00-00" : goods.PublicationDate.ToString(),
                freight = goods.freight,
            };
            return PartialView(goodsDTO);
        }

        [HttpGet]
        public ActionResult GoodsEdit()
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods();
            List<GoodsInfoViewModel> goodsDTO = DataGoodToDTO(goods);
            return View(goodsDTO);
        }

        [HttpPost]
        public ActionResult GoodsEdit(string name)
        {
            return RedirectToAction("");
        }

        [HttpGet]
        public ActionResult GoodsStock()
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods();
            List<GoodsInfoViewModel> goodsDTO = DataGoodToDTO(goods);
            return View(goodsDTO);
        }
        #endregion

        public static List<GoodsInfoViewModel> DataGoodToDTO(List<GoodsInfo> goods)
        {
            List<GoodsInfoViewModel> goodDTO = goods.Select(g => new GoodsInfoViewModel
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
            return goodDTO;
        }
    }
}