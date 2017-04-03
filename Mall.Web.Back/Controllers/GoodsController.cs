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

        #region 添加商品
        [HttpPost]
        public int CreateGoods(string name, string count, string price,
            string detail, DateTime? publicationDate, string freight = "+0",
            string author = null, string press = null)
        {
            int countNumber = Convert.ToInt16(count.Substring(1));
            double newPrice = Convert.ToDouble(price.Substring(1));
            double newFreight = Convert.ToDouble(freight.Substring(1));

            int result = _goodsService.CreateGoods(name, countNumber, newPrice, detail,
                newFreight, publicationDate, author, press);
            return result;
        }

        [HttpPost]
        public ActionResult CreateGoodsImg(int goodsId)
        {
            HttpFileCollection filess = System.Web.HttpContext.Current.Request.Files;

            foreach (var key in filess.AllKeys)
            {
                HttpPostedFile file = filess[key];
                if (string.IsNullOrEmpty(file.FileName) == false)
                {
                    if(SaveImage(file, goodsId))
                    {
                        return new EmptyResult();
                    }
                    return HttpNotFound();
                }
                return HttpNotFound();
            }
            return new EmptyResult();
        }

        private bool SaveImage(HttpPostedFile file,int goodsId)
        {
            if (!_goodsService.IsImageFull(goodsId))
            {
                string now = DateTime.Now.ToString("yyyy-MM-ddHHmmss");
                string path = "Mall.Web.Goods/Goods/" + now + file.FileName;
                file.SaveAs("D:/网站部署/MallImg/" + path);
                _goodsService.AddGoodsImage(goodsId, path);
                return true;
            }
            return false;
        }
        #endregion

        [HttpGet]
        public ActionResult GetGoodsSimpleInfo(int goodsId)
        {
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(new List<GoodsInfo> { goods }).ElementAt(0);
            return PartialView(goodsDTO);
        }

        #region 修改商品
        [HttpGet]
        public ActionResult Search(string search)
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods();
            List<GoodsInfoViewModel> goodsDTO = DataGoodToDTO(goods).Where(g => g.GoodsName.Contains(search)).ToList();
            return PartialView(goodsDTO);
        }

        [HttpGet]
        public ActionResult GoodsInfoDetails(int goodsId)
        {
            GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);
            GoodsInfoViewModel goodsDTO = DataGoodToDTO(new List<GoodsInfo> { goods }).ElementAt(0);
            return PartialView(goodsDTO);
        }

        [HttpGet]
        public ActionResult GoodsImgList(int goodsId)
        {
            List<Image> imgs = _goodsService.GetImgsByGoodsId(goodsId);
            List<ImageViewModel> imgsDTO = imgs.Select(i => new ImageViewModel
            {
                GoodsId = goodsId,
                ImageId = i.ImageId,
                ImageSrc = i.ImageSrc
            }).ToList();
            return PartialView(imgsDTO);
        }

        [HttpGet]
        public ActionResult GoodsEdit()
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods();
            List<GoodsInfoViewModel> goodsDTO = DataGoodToDTO(goods);
            return View(goodsDTO);
        }

        [HttpPost]
        public bool GoodsEdit(int goodsId, string name, string price,
            string detail, DateTime? publicationDate, string freight = "+0",
            string author = null, string press = null)
        {
            double newPrice = Convert.ToDouble(price.Substring(1));
            double newFreight = Convert.ToDouble(freight.Substring(1));

            _goodsService.ModifyGoodsInfo(goodsId, name, newPrice, detail,
                publicationDate, newFreight, author, press);
            return true;
        }

        [HttpPost]
        public bool DeletGoodsImgs(int[] imageIds)
        {
            _goodsService.DeletGoodsImage(imageIds);
            return true;
        }
        #endregion

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
            if (goods == null)
            {
                return new List<GoodsInfoViewModel>();
            }
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
                ShelfTime = g.ShelfTime == null ? "0000-00-00 00-00-00" : g.ShelfTime.Value.ToString("yyyy-MM-dd HH-mm-ss"),
                UnderShelfTime = g.UnderShelfTime == null ? "0000-00-00 00-00-00" : g.UnderShelfTime.Value.ToString("yyyy-MM-dd HH-mm-ss"),
                IsDelete = g.IsDelete,
                Author = g.Author,
                Press = g.Press,
                PublicationDate = g.PublicationDate == null ? "0000-00-00 00-00-00" : g.PublicationDate.Value.ToString("yyyy-MM-dd"),
                freight = g.freight,
                ImageUrl = g.Image == null ? "" : g.Image.ElementAt(0).ImageSrc,
            }).ToList();
            return goodDTO;
        }
    }
}