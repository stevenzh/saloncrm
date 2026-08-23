using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalonCRM.Models;

namespace SalonCRM.Manager
{
    public class MemberManager
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AccountRecordModel> GetExpenseRecords(long id)
        {
            var list = (from ar in dbcontent.AccountRecords.Where(t => t.MemberID == id && t.IsVaild == 1 && t.Type == "3")
                        select new AccountRecordModel
                        {
                            Branch = ar.Branch,
                            InAmount = ar.InAmount,
                            OutAmount = ar.OutAmount,
                            PaymentType = ar.PaymentType,
                            CreatedDate = ar.CreatedDate,
                            Splits = ar.Splits,
                            SalesType = ar.SalesType,
                            MemberProjects = dbcontent.MemberProjects.Where(t => t.AccountRecordID == ar.RecordID).ToList()
                        }).OrderByDescending(t => t.CreatedDate).ToList();
            return list;
        }
        /// <summary>
        /// 消耗项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<BookProjectModel> GetExpenseProjects(long id)
        {
            return (from b in dbcontent.BookProjects.Where(t => t.Book.MemberID == id && t.Book.State == "20")
                    join c in dbcontent.MemberCards.Where(t => t.Status == 1) on b.MemberCardId equals c.MemberCardId
                    join d in dbcontent.Organs on b.Book.BranchId equals d.OrganID
                    select new BookProjectModel
                    {
                        Amount = b.Amount,
                        BookID = b.BookID,
                        BookProjectID = b.BookProjectID,
                        MemberCardId = b.MemberCardId,
                        MemberCardTitle = c.Title,
                        MemberGiveId = b.MemberGiveId,
                        MemberProjectId = b.MemberProjectId,
                        Points = b.Points,
                        ProjectID = b.ProjectID,
                        Quantity = b.Quantity,
                        HandicraftFee = b.HandicraftFee,
                        Type = b.Type,
                        BranchName = d.Name,
                        CreatedDate = b.Book.CreatedDate,
                        Project = b.Project,
                        Splits = b.UserSplits
                    })
                .OrderByDescending(t => t.CreatedDate).ToList();
        }

        /// <summary>
        /// 账户充值记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<AccountRecord> GetRechargeRecords(long id)
        {
            return dbcontent.AccountRecords.Where(t => t.MemberID == id && t.IsVaild == 1)
                .Where(t => t.Type == "1" || t.Type == "2")
                .OrderByDescending(t => t.CreatedDate).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<FeedbackViewModel> GetFeedbacks(long id)
        {
            return (from fb in dbcontent.Feedbacks.Where(t => t.MemberId == id)
                    select new FeedbackViewModel
                    {
                        FeedbackId = fb.FeedbackId,
                        CallUserName = dbcontent.Users.Where(t => t.Id == fb.CallUserId).FirstOrDefault().UserCnName,
                        CreatedDate = fb.CreatedDate,
                        Centent = fb.Centent,
                        Purpose = dbcontent.Dictionaries.Where(a => a.KeyValue == fb.Purpose && a.Identifier == "FeedbackType").FirstOrDefault().Contents,
                        NextDate = fb.NextDate
                    }).ToList();
        }


        /// <summary>
        /// 取得客户所有项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<MemberProjectViewModel> GetAllProjects(long id)
        {
            var mb = (from l in dbcontent.MemberProjects.Where(t => t.MemberID == id)
                      join al in dbcontent.AccountRecords.Where(t => t.IsVaild == 1) on l.AccountRecordID equals al.RecordID into mp
                      from ll in mp.DefaultIfEmpty()
                      join mc in dbcontent.MemberCards.Where(t => t.Status == 1) on ll.MemberCardId equals mc.MemberCardId into mpc
                      from ur in mpc.DefaultIfEmpty()
                      select new MemberProjectViewModel
                      {
                          MemberCardId = l.MemberCardId,
                          CardTitle = ur.Title,
                          ProjectName = l.Project.Name,
                          MemberProjectId = l.MemberProjectId,
                          Type = l.Type,
                          BookTime = l.BookTime,
                          UsedTime = l.UsedTime,
                          LastCount = l.LastCount,
                          UnitPrice = l.UnitPrice,
                          Amount = l.Amount,
                          ActualPrice = l.ActualPrice,
                          CreatedDate = l.CreatedDate,
                          Sales = ll.Splits,
                          CardType = (ur.Type == null) ? "" : ur.Type,
                          IsEntity = l.IsEntity,
                          BranchName = ll.Branch.Name
                      }).ToList();
            foreach (var bp in mb)
            {
                if (bp.Sales != null)
                    bp.SalesStr = String.Join(",", bp.Sales.Select(t => t.User.UserCnName).ToArray());
            }
            return mb;
        }


    }
}
