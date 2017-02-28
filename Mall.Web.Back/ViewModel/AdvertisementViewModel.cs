using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Data.DataBase
{    
    public class AdvertisementViewModel
    {
        public int AdvertisementID { get; set; }
        public int ImageId { get; set; }
        public string Url { get; set; }
        public int Category { get; set; }
    }
}
