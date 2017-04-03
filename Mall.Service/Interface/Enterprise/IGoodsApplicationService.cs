using Mall.Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Interface.Enterprise
{
    public interface IGoodsApplicationData
    {
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        int CreateGoods(string name, int count,
            double price, string detail,
            double? freight, DateTime? publicationDate,
            string author = null, string press = null);
        /// <summary>
        /// 通过商品Id删除商品
        /// </summary>
        /// <returns></returns>
        bool DeleteByGoodsId(int goodsId);
        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="good"></param>
        /// <returns></returns>
        void ModifyGoodsInfo(int goodsId, string name, double price,
            string detail, DateTime? publicationDate, double ? freight,
            string author = null, string press = null);
        /// <summary>
        /// 根据商品Id和修改后的库存修改
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="stock"></param>
        void ModifyGoodsStockByGoodsId(int goodsId,int stock = 0);
        /// <summary>
        /// 根据商品Id上架商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        bool OnShelvesByGoodsId(int goodsId);
        /// <summary>
        /// 根据商品Id下架商品
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        bool OffShelvesByGoodsId(int goodsId);
    }
}
