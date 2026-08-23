using System;
using System.Collections.Generic;

namespace SalonCRM.Models
{
    /// <summary>
    /// 商家
    /// </summary>
    public partial class Host
    {
        public Host()
        {
            this.Appointments = new List<Appointment>();
            this.Clients = new List<Client>();
            this.Organs = new List<Organ>();
            this.Projects = new List<Project>();
        }

        public int HostID { get; set; }
        /// <summary>
        /// 商家编号
        /// </summary>
        public string HostCode { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 店铺数量
        /// </summary>
        public Nullable<int> BranchNum { get; set; }
        /// <summary>
        /// 终端数量
        /// </summary> 
        public Nullable<int> ClientNum { get; set; }
        /// <summary>
        /// 行业
        /// </summary>
        public string Industry { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 营业地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public int IsVaild { get; set; }



        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Organ> Organs { get; set; }
        public ICollection<Project> Projects { get; set; }
        //public ICollection<Goods> Goods { get; set; }
    }
}