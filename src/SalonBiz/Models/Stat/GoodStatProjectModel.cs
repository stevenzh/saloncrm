using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    public class GoodStatProjectModel
    {
        public long BookGoodsID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MemberName { get; set; }
        public string ProjectName { get; set; }
        public decimal Num { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
