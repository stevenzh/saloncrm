using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    [Serializable]
    public class WxMemberQModel
    {
        public WxMemberQModel()
        {
        }
        public int HostID { get; set; }
        public string OpenID { get; set; }
        public string Name { get; set; }
        public string Sales { get; set; }
        public string Subscribe { get; set; }
        public string Binding { get; set; }
        public string Approved { get; set; }
        /// <summary>
        /// 是否公司员工
        /// </summary>
        public string Employee { get; set; }
        public IList<MemberModel> MemberList { set; get; }
        public PagedList<MemberModel> MemberPageList { get; set; }

    }
}
