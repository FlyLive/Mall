using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mall.Web.Back.ViewModel
{
    public class AdminLogViewModel
    {
        public long RecordId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime OperationTime { get; set; }
        public string OperatDetail { get; set; }
        public string Operater { get; set; }
        public string Object { get; set; }
        public string Permission { get; set; }
        public string Style { get; set; }
    }
}
