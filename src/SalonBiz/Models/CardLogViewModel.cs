using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SalonCRM.Models
{

    /// <summary>
    /// 转卡
    /// </summary>
    public class CardTransferViewModel
    {
        public long LogId { get; set; }
        public int HostId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CardNo { get; set; }
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        public string OriginalProjects { get; set; }
        public decimal OriginalPrjAmt { get; set; }
        public string NewProjects { get; set; }
        public decimal NewPrjAmt { get; set; }
        public System.DateTime CreatedDate { get; set; }
        private string _Shell;
        public string Shell
        {
            get { return _Shell; }
            set
            {
                _Shell = value;
                if (!string.IsNullOrEmpty(value))
                {
                    EventLogShell sh = JsonConvert.DeserializeObject<EventLogShell>(value);
                    this.OriginalProjects = sh.OriginalProjects;
                    this.OriginalPrjAmt = sh.OriginalPrjAmt;
                    this.NewProjects = sh.NewProjects;
                    this.NewPrjAmt = sh.NewPrjAmt;
                }

            }
        }
        public string Content { get; set; }
    }

    /// <summary>
    /// 换卡
    /// </summary>
    public class CardReplaceViewModel
    {
        public long LogId { get; set; }
        public int HostId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long MemberId { get; set; }
        public string MemberName { get; set; }
        private string _Shell;
        public string Shell
        {
            get { return _Shell; }
            set
            {
                _Shell = value;
                if (!string.IsNullOrEmpty(value))
                {
                    EventLogShell sh = JsonConvert.DeserializeObject<EventLogShell>(value);
                    this.OriginalCardNo = sh.OriginalCardNo;
                    this.NewCardNo = sh.NewCardNo;
                }

            }
        }
        public string OriginalCardNo { get; set; }
        public string NewCardNo { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// 换卡
    /// </summary>
    public class CardRepQModel
    {
        public CardRepQModel()
        {
        }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string CardNo { get; set; }
        public string Mobile { get; set; }
        public IList<CardReplaceViewModel> RelaceList { get; set; }
        public IList<CardTransferViewModel> TransferList { get; set; }
    }
}