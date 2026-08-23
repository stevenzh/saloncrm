using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    public class PointBookQModel
    {
        /// <summary>
        /// 门店
        /// </summary>
        public int BranchId { get; set; }
        /// <summary>
        /// 客户卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string MemberName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IList<PointBook> PointList { get; set; }
    }
}
