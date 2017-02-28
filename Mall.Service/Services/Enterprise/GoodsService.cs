using Mall.Interface.Enterprise;
using Mall.Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Services.Enterprise
{
    public class GoodsService : IDisposable, IGoodsApplicationData
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
                                     .Take(5).ToList();
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
                                     .Take(5).ToList();
            return goods;
        }

        /// <summary>
        /// 随机推荐
        /// </summary>
        /// <returns></returns>
        public List<GoodsInfo> GetRandomGoodsTop5()
        {
            var dbGoods = _db.GoodsInfo;
            Random random = new Random();
            List<GoodsInfo> newList = new List<GoodsInfo>();
            foreach (GoodsInfo item in dbGoods)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            List<GoodsInfo> goods = newList.Take(5).ToList();
            return goods;
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <param name="price"></param>
        /// <param name="detail"></param>
        /// <param name="onshelves"></param>
        /// <param name="freight"></param>
        /// <param name="publicationDate"></param>
        /// <param name="author"></param>
        /// <param name="press"></param>
        /// <returns></returns>
        public bool CreateGoods(string name, int count,
            double price, string detail, int onshelves,
            double? freight, DateTime? publicationDate,
            string author = null, string press = null)
        {
            try
            {
                GoodsInfo good = new GoodsInfo
                {
                    GoodsName = name,
                    Stock = count,
                    Price = price,
                    Details = detail,
                    State = onshelves,
                    IsDelete = false,
                    CommentNumber = 0,
                    CreateTime = DateTime.Now,

                    freight = freight == null ? 0.00 : (double)freight,
                    PublicationDate = publicationDate,
                    Author = author,
                    Press = press,
                };
                _db.GoodsInfo.Add(good);
                _db.SaveChanges();

            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据商品Id删除商品
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns>删除成功/失败</returns>
        public bool DeleteByGoodsId(int goodsId)
        {
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            if (good.State == 2)
            {
                _db.GoodsInfo.Remove(good);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="good"></param>
        public void ModifyGoodsInfo(GoodsInfo good)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据商品Id修改库存
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <param name="stock">库存</param>
        public void ModifyGoodsStockByGoodsId(int goodsId, int stock = 0)
        {
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            good.Stock = stock;

            _db.SaveChanges();
        }

        /// <summary>
        /// 根据商品Id将商品下架
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public bool OffShelvesByGoodsId(int goodsId)
        {
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            good.State = 2;

            _db.SaveChanges();

            return true;
        }

        /// <summary>
        /// 根据商品Id将商品上架
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool OnShelvesByGoodsId(int goodsId)
        {
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            if (good.Stock != 0)
            {
                good.State = 1;

                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
