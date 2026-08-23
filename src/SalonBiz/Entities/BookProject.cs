using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonCRM.Models
{
    /// <summary>
    /// 消耗项目
    /// </summary>
    public class BookProject
    {
        public BookProject()
        {
            this.UserSplits = new List<BookProjectSplit>();
        }
        public long BookProjectID { get; set; }
        public long BookID { get; set; }
        /// <summary>
        /// 消耗关联卡
        /// </summary>
        public Nullable<long> MemberCardId { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        public Nullable<long> MemberProjectId { get; set; }
        public Nullable<long> MemberGiveId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// (0:卡扣) 1:现金 2:刷卡 3: 转账 4: 储值卡
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public Nullable<int> Points { get; set; }
        /// <summary>
        /// 手工费
        /// </summary>
        public decimal HandicraftFee { get; set; }
        /// <summary>
        /// 满意度
        /// </summary>
        public Nullable<int> Satisfaction { get; set; }
        /// <summary>
        /// 评语
        /// </summary>
        public string Appraisal { get; set; }
        /// <summary>
        /// 美容师
        /// </summary>
        public string BeauticianId { get; set; }
        public virtual Book Book { get; set; }
        public virtual Project Project { get; set; }

        /// <summary>
        /// 辅助美容师（）
        /// </summary>
        [Display(Name = "辅助美容师")]
        public virtual ICollection<BookProjectSplit> UserSplits { get; set; }

        public virtual ICollection<BookGoods> BookGoods { get; set; }

    }
}