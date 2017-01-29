using Mall.Data.DataBase;
using Mall.Data.Services.Enterprise;
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
        public ActionResult CreateGoods(string name)
        {
            return RedirectToAction("");
        }

        [HttpGet]
        public ActionResult GoodsInfo(int goodsId)
        {
            Data.DataBase.GoodsInfo goods = _goodsService.GetGoodsByGoodsId(goodsId);

            return PartialView(goods);
        }

        [HttpGet]
        public ActionResult GoodsEdit()
        {
            List<GoodsInfo> goods = _goodsService.GetAllGoods();
            return View(goods);
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
            return View(goods);
        }
        #endregion
    }
}