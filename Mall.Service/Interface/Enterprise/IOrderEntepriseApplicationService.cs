using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Interface.Enterprise
{
    public interface IOrderEntepriseApplicationService
    {
        /// <summary>
        /// 接受订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        bool AcceptOrderByOrderId(int employeeId,Guid orderId);
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="remark">备注</param>
        bool ModifyRemarksByOrderId(int employeeId, Guid orderId, string remark);
        /// <summary>
        /// 同意退款
        /// </summary>
        /// <param name="orderId">订单Id</param>
        bool AgreeRefundByOrderId(int employeeId, Guid orderId);
        /// <summary>
        /// 确认发货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        bool ConfirmDeliverByOrderId(int employeeId, Guid orderId);
        /// <summary>
        /// 同意退货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        bool AgreeReturnByOrderId(int employeeId, Guid orderId);
        /// <summary>
        /// 拒绝退货
        /// </summary>
        /// <param name="orderId">订单Id</param>
        bool RefuseReturnByOrderId(int employeeId, Guid orderId);
    }
}
