using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalonCRM.Models
{
    public class EventLogModel
    {
        public long LogId { get; set; }
        public int HostId { get; set; }
        public string HostName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Nullable<long> MemberId { get; set; }
        public string MemberName { get; set; }
        public int TypeId { get; set; }
        public int Level { get; set; }
        public string Content { get; set; }
        public string Shell { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ClientId { get; set; }
        public Member Member { get; set; }
        public string Sales { get; set; }
    }

    public class EventLogQModel
    {
        public int HostID { get; set; }
        public int TypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<EventLogModel> LogList { get; set; }

        public EventLog EventLog { get; set; }

        public List<AccountRecordModel> RecordList { get; set; }

        public List<MemberCard> CardList { get; set; }

        public Member Member { get; set; }
        public Organ Branch { get; set; }
    }
}
