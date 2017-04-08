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
    public class GoodsManageService : IDisposable, IGoodsApplicationService
    {
        private MallDBContext _db;

        public GoodsManageService()
        {
            _db = new MallDBContext();
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
        public int CreateGoods(int employeeId, string name, int count,
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
                    SalesNumber = 0,
                    CreateTime = DateTime.Now,

                    Freight = freight == null ? 0.00 : (double)freight,
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
                Console.Out.Write(e);
                return -1;
            }
        }

        public bool IsImageFull(int goodsId)
        {
            var imgs = _db.Image.Where(g => g.GoodsId == goodsId).ToList();
            if (imgs.Count >= 5)
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
        public bool AddGoodsImage(int goodsId, string path)
        {
            try
            {
                _db.Image.Add(new Image
                {
                    GoodsId = goodsId,
                    ImageSrc = "http://localhost:9826/" + path,
                });
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 删除商品图片
        /// </summary>
        /// <param name="imageId"></param>
        public bool DeletGoodsImage(int[] imageId)
        {
            try
            {
                List<Image> imgs = _db.Image.Where(i => imageId.Any(ii => ii == i.ImageId)).ToList();
                _db.Image.RemoveRange(imgs);
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 根据商品Id删除商品
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns>删除成功/失败</returns>
        public bool DeleteByGoodsId(int employeeId, int goodsId)
        {
            try
            {
                Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
                GoodsInfo good = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);
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
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 修改商品信息
        /// </summary>
        /// <param name="good"></param>
        public bool ModifyGoodsInfo(int employeeId, int goodsId, string name, double price,
            string detail, DateTime? publicationDate, double? freight,
            string author = null, string press = null)
        {
            try
            {
                Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
                GoodsInfo good = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);
                good.GoodsName = name;
                good.Price = price;
                good.Details = detail;
                good.PublicationDate = publicationDate;
                good.Freight = freight == null ? 0.00 : (double)freight;
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
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 根据商品Id修改库存
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <param name="stock">库存</param>
        public bool ModifyGoodsStockByGoodsId(int employeeId, int goodsId, int stock)
        {
            try
            {
                Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
                GoodsInfo good = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);
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
                return true;
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 根据商品Id将商品下架
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <returns></returns>
        public bool OffShelvesByGoodsId(int employeeId, int goodsId)
        {
            try
            {
                Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
                GoodsInfo good = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);
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
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        /// <summary>
        /// 根据商品Id将商品上架
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public bool OnShelvesByGoodsId(int employeeId, int goodsId)
        {
            try
            {
                Employee employee = _db.Employee.SingleOrDefault(e => e.EmployeeId == employeeId);
                GoodsInfo good = _db.GoodsInfo.SingleOrDefault(g => g.GoodsId == goodsId);
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
            catch (Exception e)
            {
                Console.Out.Write(e);
                return false;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
