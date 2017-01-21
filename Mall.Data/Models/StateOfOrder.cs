using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.Models
{
    public static class StateOfOrder
    {
        public enum State
        {
            Cancle = 0,
            ToPay = 1,
            ToDelivery = 2,
            ToReceipt = 3,
            ToEvaluate = 4,
            ToRefund = 5,
            Finish = 6,
        }
    }
}
