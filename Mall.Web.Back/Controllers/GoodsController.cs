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
        public bool CreateGoods(string name, string count, string price,
            string detail, DateTime? publicationDate, string freight = "+0",
            string author = null, string press = null)
        {
            int countNumber = Convert.ToInt16(count.Substring(1));
            double newPrice = Convert.ToDouble(price.Substring(1));
            double newFreight = Convert.ToDouble(freight.Substring(1));
            //string name = System.Web.HttpContext.Current.Request.Form["name"];
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

            foreach(var key in files.AllKeys)
            {
                HttpPostedFile file = files[key];
                if(string.IsNullOrEmpty(file.FileName) == false)
                {
                    string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                    file.SaveAs(HttpContext.Server.MapPath("~/Pictures/Goods/") + now + file.FileName);
                }
            }

            var result = _goodsService.CreateGoods(name, countNumber, newPrice, detail,
                0, newFreight, publicationDate, author, press);
            return true;
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

        [HttpGet]
        public ActionResult GoodsEdit()
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods();
            List<GoodsInfoViewModel> goodsDTO = goods
                .Select(good => new GoodsInfoViewModel {
                    GoodsId = good.GoodsId,
                    GoodsName = good.GoodsName,
                    Price = good.Price,
                    Stock = good.Stock,
                    Details = good.Details,
                    Category = good.Category,
                    CommentNumber = good.CommentNumber,
                    State = good.State,
                    CreateTime = good.CreateTime,
                    ShelfTime = (DateTime)good.ShelfTime,
                    UnderShelfTime = (DateTime)good.UnderShelfTime,
                    IsDelete = good.IsDelete,
                    Author = good.Author,
                    Press = good.Press,
                    PublicationDate = (DateTime)good.PublicationDate,
                    freight = good.freight,
                }).ToList();
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
            List<GoodsInfoViewModel> goodsDTO = goods
                .Select(good => new GoodsInfoViewModel{
                    GoodsId = good.GoodsId,
                    GoodsName = good.GoodsName,
                    Price = good.Price,
                    Stock = good.Stock,
                    Details = good.Details,
                    Category = good.Category,
                    CommentNumber = good.CommentNumber,
                    State = good.State,
                    CreateTime = good.CreateTime,
                    ShelfTime = (DateTime)good.ShelfTime,
                    UnderShelfTime = (DateTime)good.UnderShelfTime,
                    IsDelete = good.IsDelete,
                    Author = good.Author,
                    Press = good.Press,
                    PublicationDate = (DateTime)good.PublicationDate,
                    freight = good.freight,
                }).ToList();
            return View(goodsDTO);
        }
        
        #endregion
    }
}