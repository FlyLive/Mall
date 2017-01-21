using Mall.Data.DataBase;
using Mall.Data.IManager.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Services.Enterprise
{
    public class GoodsService : IDisposable, IGoodsApplicationService
    {
        private MallDBContext _db;

        public GoodsService()
        {
            _db = new MallDBContext();
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
        /// 创建商品
        /// </summary>
        /// <param name="good"></param>
        /// <returns></returns>
        public bool CreateGoods(GoodsInfo good)
        {
            _db.GoodsInfo.Add(good);
            _db.SaveChanges();

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
