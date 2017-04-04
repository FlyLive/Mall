using Mall.Interface.Enterprise;
using Mall.Service.DataBase;
using Mall.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public int CreateGoods(int employeeId,string name, int count,
            double price, string detail,
            double? freight, DateTime? publicationDate,
            string author = null, string press = null)
        {
            try
            {
                Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
                GoodsInfo good = new GoodsInfo
                {
                    GoodsName = name,
                    Stock = count,
                    Price = price,
                    Details = detail,
                    State = (int)StateOfGoods.State.OffShelves,
                    IsDelete = false,
                    CommentNumber = 0,
                    CreateTime = DateTime.Now,

                    freight = freight == null ? 0.00 : (double)freight,
                    PublicationDate = publicationDate,
                    Author = author,
                    Press = press,
                };
                _db.GoodsInfo.Add(good);
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "添加商品(AddGoods)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "添加商品:" + good.GoodsName + "商品Id为:" + good.GoodsId,
                    Operater = employee.User.RealName,
                    Object = "商品",
                    Style = "添加",
                });

                _db.SaveChanges();
                return good.GoodsId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public bool IsImageFull(int goodsId)
        {
            var imgs = GetImgsByGoodsId(goodsId);
            if(imgs.Count >= 5)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加商品图片
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="path"></param>
        public void AddGoodsImage(int goodsId,string path)
        {
            _db.Image.Add(new Image
            {
                GoodsId = goodsId,
                ImageSrc = "http://localhost:9826/" + path,
            });
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除商品图片
        /// </summary>
        /// <param name="imageId"></param>
        public void DeletGoodsImage(int[] imageId)
        {
            List<Image> imgs = _db.Image.Where(i => imageId.Any(ii => ii == i.ImageId)).ToList();
            _db.Image.RemoveRange(imgs);
            _db.SaveChanges();
        }

        /// <summary>
        /// 根据商品Id删除商品
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns>删除成功/失败</returns>
        public bool DeleteByGoodsId(int employeeId,int goodsId)
        {
            Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            if (good.State == 2)
            {
                good.State = (int)StateOfGoods.State.Delet;
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "商品进货(NewStorage)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "删除商品:" + good.GoodsName + "商品Id为:" + good.GoodsId,
                    Operater = employee.User.RealName,
                    Object = "商品状态",
                    Style = "修改",
                });

                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="good"></param>
        public void ModifyGoodsInfo(int employeeId,int goodsId, string name, double price,
            string detail, DateTime? publicationDate, double ? freight,
            string author = null, string press = null)
        {
            Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            good.GoodsName = name;
            good.Price = price;
            good.Details = detail;
            good.PublicationDate = publicationDate;
            good.freight = freight == null ? 0.00 : (double)freight;
            good.Author = author;
            good.Press = press;
            _db.AdminLog.Add(new AdminLog
            {
                EmployeeId = employee.EmployeeId,
                Permission = "商品编辑(ModifyGoods)",
                OperationTime = DateTime.Now,
                OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "对商品:" + good.GoodsName + "修改信息" + "商品Id为:" + good.GoodsId,
                Operater = employee.User.RealName,
                Object = "商品信息",
                Style = "修改",
            });

            _db.SaveChanges();
        }

        /// <summary>
        /// 根据商品Id修改库存
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <param name="stock">库存</param>
        public void ModifyGoodsStockByGoodsId(int employeeId,int goodsId, int stock)
        {
            Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            good.Stock += stock;
            _db.AdminLog.Add(new AdminLog
            {
                EmployeeId = employee.EmployeeId,
                Permission = "商品上架(NewStorage)",
                OperationTime = DateTime.Now,
                OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "对商品:" + good.GoodsName + "进货" + "商品Id为:" + good.GoodsId + "数量为:" + stock,
                Operater = employee.User.RealName,
                Object = "商品库存",
                Style = "进货",
            });

            _db.SaveChanges();
        }

        /// <summary>
        /// 根据商品Id将商品下架
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public bool OffShelvesByGoodsId(int employeeId,int goodsId)
        {
            Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            good.State = 2;
            _db.AdminLog.Add(new AdminLog
            {
                EmployeeId = employee.EmployeeId,
                Permission = "商品下架(OffShelves)",
                OperationTime = DateTime.Now,
                OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "将商品:" + good.GoodsName + "下架停售" + "商品Id为:" + good.GoodsId,
                Operater = employee.User.RealName,
                Object = "商品状态",
                Style = "修改",
            });

            _db.SaveChanges();

            return true;
        }

        /// <summary>
        /// 根据商品Id将商品上架
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool OnShelvesByGoodsId(int employeeId,int goodsId)
        {
            Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
            GoodsInfo good = GetGoodsByGoodsId(goodsId);
            if (good.Stock != 0)
            {
                good.State = 1;
                _db.AdminLog.Add(new AdminLog
                {
                    EmployeeId = employee.EmployeeId,
                    Permission = "商品上架(OnShelves)",
                    OperationTime = DateTime.Now,
                    OperatDetail = "员工" + employee.User.RealName + "于" + DateTime.Now + "将商品:" + good.GoodsName + "上架销售" + "商品Id为:" + good.GoodsId,
                    Operater = employee.User.RealName,
                    Object = "商品状态",
                    Style = "修改",
                });

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
