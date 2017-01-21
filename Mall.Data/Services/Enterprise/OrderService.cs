using Mall.Data.DataBase;
using Mall.Data.Interface.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Services.Enterprise
{
    public class OrderService : IDisposable, IOrderEntepriseApplicationService
    {
        private MallDBContext _db;

        public OrderService()
        {
            _db = new MallDBContext();
        }

        public void AcceptOrderByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void AgreeRefundByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void AgreeReturnByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void ConfirmDeliverByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void ModifyRemarksByOrderId(Guid orderId, string remark)
        {
            throw new NotImplementedException();
        }

        public void RefuseRefundByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public void RefuseReturnByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
