using Mall.Interface.Enterprise;
using Mall.Service.DataBase;
using Mall.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Services
{
    public class GoodsService
    {
        private MallDBContext _db;

        public GoodsService()
        {
            _db = new MallDBContext();
        }

        public List<GoodsInfo> GetAllGoods()
        {
            return _db.GoodsInfo.ToList();
        }

        public List<GoodsInfo> GetCarousels()
        {
            List<GoodsInfo> goods = _db.GoodsInfo.Take(5).ToList();
            return goods;
        }

        public List<GoodsInfo> GetGoodsByGoodsState(int state)
        {
            List<GoodsInfo> goods = _db.GoodsInfo.Where(g => g.State == state).ToList();
            return goods;
        }

        /// <summary>
        /// 根据商品Id获取商品
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public GoodsInfo GetGoodsByGoodsId(int goodsId)
        {
            GoodsInfo goods = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);
            return goods;
        }

        /// <summary>
        /// 根据商品Id获取商品图片
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public List<Image> GetImgsByGoodsId(int goodsId)
        {
            List<Image> imgs = _db.Image.Where(i => i.GoodsId == goodsId).ToList();
            return imgs;
        }

        /// <summary>
        /// 根据商品Id获取商品评价
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public List<Comment> GetGoodsCommentsByGoodsId(int goodsId)
        {
            List<Comment> comments = _db.Comment.Where(c => c.GoodsId == goodsId).ToList();
            return comments;
        }

        /// <summary>
        /// 热销商品
        /// </summary>
        /// <returns></returns>
        public List<GoodsInfo> GetHotSaleGoodsTop5()
        {
            var dbGoods = _db.GoodsInfo;
            List<GoodsInfo> goods = (from g in dbGoods
                                     orderby g.CommentNumber
                                     descending
                                     select g)
                                     .Take(5).Where(g => g.State == (int)StateOfGoods.State.OnShelves).ToList();
            return goods;
        }

        /// <summary>
        /// 新品
        /// </summary>
        /// <returns></returns>
        public List<GoodsInfo> GetNewGoodsTop5()
        {
            var dbGoods = _db.GoodsInfo;
            List<GoodsInfo> goods = (from g in dbGoods
                                     orderby g.CreateTime
                                     descending
                                     select g)
                                     .Take(5).Where(g => g.State == (int)StateOfGoods.State.OnShelves).ToList();
            return goods;
        }

        /// <summary>
        /// 随机推荐
        /// </summary>
        /// <returns></returns>
        public List<GoodsInfo> GetRandomGoodsTop5()
        {
            var dbGoods = _db.GoodsInfo.Where(g => g.State == (int)StateOfGoods.State.OnShelves);
            Random random = new Random();
            List<GoodsInfo> newList = new List<GoodsInfo>();
            foreach (GoodsInfo item in dbGoods)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            List<GoodsInfo> goods = newList.Take(5).ToList();
            return goods;
        }
    }
}
