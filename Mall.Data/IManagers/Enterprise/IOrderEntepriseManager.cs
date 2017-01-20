using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.IManagers.Enterprise
{
    public interface IOrderEntepriseManager
    {
        /// <summary>
        /// 接受订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void AcceptOrderByOrderId(Guid orderId);
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="remark">备注</param>
        void ModifyRemarksByOrderId(Guid orderId, string remark);
        /// <summary>
        /// 同意退款
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void AgreeRefundByOrderId(Guid orderId);
        /// <summary>
        /// 拒绝退款
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void RefuseRefundByOrderId(Guid orderId);
        /// <summary>
        /// 确认发货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void ConfirmDeliverByOrderId(Guid orderId);
        /// <summary>
        /// 同意退货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void AgreeReturnByOrderId(Guid orderId);
        /// <summary>
        /// 拒绝退货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        void RefuseReturnByOrderId(Guid orderId);
    }
}
