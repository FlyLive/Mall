using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Service.Models
{
    public static class StateOfGoods
    {
        public enum State
        {
            OnShelves = 1,
            OffShelves = 2,
            Delet = 3,
        }
    }
}
