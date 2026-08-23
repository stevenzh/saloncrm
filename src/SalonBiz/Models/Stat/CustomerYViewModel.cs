using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonBiz.Models.Stat
{
    /// <summary>
    /// 客户预约到店统计
    /// </summary>
    public class CustomerYViewModel
    {
        public DateTime BookDate { get; set; }
        public string BranchName { get; set; }
        /// <summary>
        /// 预约人数
        /// </summary>
        public int? AppointmentPax { get; set; }
        /// <summary>
        /// 到店人数
        /// </summary>
        public int? InPax { get; set; }
        /// <summary>
        /// 消耗人头数
        /// </summary>
        public int BookPax { get; set; }
        /// <summary>
        /// 消耗金额
        /// </summary>
        public decimal BookAmount { get; set; }

        public int? NewInPax { get; set; }
        /// <summary>
        /// 新客消耗人头数
        /// </summary>
        public int NewBookPax { get; set; }
        /// <summary>
        /// 新客消耗金额
        /// </summary>
        public decimal NewBookAmount { get; set; }
    }
}
