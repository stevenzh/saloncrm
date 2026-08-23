using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common.Logging;
using SalonCRM.Models;
using SalonCRM.Web;

namespace SalonCRM.Controllers.STA
{
    /// <summary>
    /// 欠款一览表
    /// </summary>
    [Authorize]
    public class DebtController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("DebtController");

        /// <summary>
        /// 还款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Repayment(long id, long cardId)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            MemberProjectViewModel mpe = null;

            if (id > 0)
            {
                mpe = (from v in dbcontent.MemberProjects.Where(t => t.MemberProjectId == id)
                       select new MemberProjectViewModel
                       {
                           MemberProjectId = v.MemberProjectId,
                           MemberID = v.MemberID,
                           MemberName = v.Member.Name,
                           ProjectName = v.Project.Name,
                           Amount = v.Amount,
                           ActualPrice = v.ActualPrice,
                           Payment = v.Amount - v.ActualPrice,
                           AccountRecordID = v.AccountRecordID
                       }).FirstOrDefault();
                var ss = dbcontent.AccountRecords.Where(t => t.RecordID == mpe.AccountRecordID).FirstOrDefault();
                mpe.SalesStr = ss.SaleID;
                mpe.Beautician = ss.Splits.Select(t => t.UserID).ToArray();
            }


            if (cardId > 0)
            {
                // 卡项
                mpe = (from v in dbcontent.MemberCards.Where(t => t.MemberCardId == cardId)
                       select new MemberProjectViewModel
                       {
                           MemberCardId = v.MemberCardId,
                           CardTitle = v.Title,
                           MemberID = v.MemberID,
                           MemberName = v.Member.Name,
                           Amount = v.Amount,
                           ActualPrice = v.ActualPrice,
                           Payment = v.Amount - v.ActualPrice
                       }).FirstOrDefault();

                var ss = dbcontent.AccountRecords.Where(t => t.MemberCardId == cardId && t.Type == "2").FirstOrDefault();
                mpe.SalesStr = ss.SaleID;
                mpe.Beautician = ss.Splits.Select(t => t.UserID).ToArray();
            }


            // 所有顾问
            ViewBag.Salesman = new SelectList(dbcontent.Users.Where(t => t.HostId == hostId && t.Type == "3").ToList(), "Id", "UserCnName");
            ViewData["Workers"] = new SelectList(dbcontent.Users.Where(t => t.HostId == hostId && t.Type == "1").ToList(), "Id", "UserCnName");

            return View(mpe);
        }

        /// <summary>
        /// 还款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Repayment(MemberProjectViewModel model)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            int branchId = GlobalContext.Current.UserDepartment.OrganID;

            Payment(hostId, branchId, "", model.MemberID, model.MemberProjectId, model.MemberCardId.Value, "", model.Payment,
                GlobalContext.Current.UserInfo.Id, model.SalesStr, model.SalesRadix, model.Beautician, model.WorkerRadix);

            return Redirect("/Stat/Financial_Debts.aspx");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="client"></param>
        /// <param name="memberId"></param>
        /// <param name="memberProjectId"></param>
        /// <param name="memberCardId"></param>
        /// <param name="clientID"></param>
        /// <param name="amount"></param>
        /// <param name="userId"></param>
        /// <param name="sales"></param>
        /// <param name="salesRadix"></param>
        /// <param name="workers"></param>
        /// <param name="workerRadix"></param>
        private void Payment(int hostId, int branchId, string client, long memberId, long memberProjectId, long memberCardId, string clientID,
            decimal amount, string userId, string sales, decimal salesRadix, IEnumerable<string> workers, decimal workerRadix)
        {

            // 操作任务
            var log = new EventLog
            {
                HostId = hostId,
                BranchId = branchId,
                MemberId = memberId,
                TypeId = 7,
                UserId = userId,
                ClientId = clientID,
                CreatedDate = DateTime.Now,
                Level = 5
            };
            var eventLog = dbcontent.EventLogs.Add(log);


            List<AccountRecordSplit> us = new List<AccountRecordSplit>();
            us.Add(new AccountRecordSplit
            {
                UserID = sales,
                Position = "1",
                Percentage = salesRadix,
                Amount = amount * salesRadix,
                ModifiedBy = userId,
                ModifiedTime = DateTime.Now
            });
            foreach (var works in workers)
            {
                if (!string.IsNullOrEmpty(works))
                {
                    us.Add(new AccountRecordSplit
                    {
                        UserID = works,
                        Position = "2",
                        Percentage = workerRadix,
                        Amount = amount * workerRadix,
                        ModifiedBy = userId,
                        ModifiedTime = DateTime.Now
                    });
                }
            }

            // 项目还款
            if (memberProjectId > 0)  // 项目欠款
            {
                // 其他变更
                MemberProject mp = dbcontent.MemberProjects.Find(memberProjectId);
                mp.ActualPrice = mp.ActualPrice + amount;
                if (mp.ActualPrice == mp.Amount)  // 欠款还清
                {
                    mp.status = 2;
                }

                if (mp.MemberCardId != null)
                {
                    // 加财务记录
                    var record = new AccountRecord
                    {
                        MemberCardId = mp.MemberCardId.Value,
                        //Balance = mc.Amt;
                        MemberID = memberId,
                        HostID = hostId,
                        EventLogId = eventLog.LogId,
                        InAmount = amount,
                        PaymentType = "1",
                        Type = "1",
                        BranchId = branchId,
                        ClientID = "",
                        Remark = "还款",
                        CreatedDate = DateTime.Now,
                        Splits = us, // 对应销售
                        IsVaild = 1
                    };

                    dbcontent.AccountRecords.Add(record);
                }
            }

            // 会员卡还款
            if (memberCardId > 0)   // 购卡欠款
            {
                MemberCard mp = dbcontent.MemberCards.Find(memberCardId);
                mp.ActualPrice = mp.ActualPrice + amount;
                if (mp.ActualPrice == mp.Amount)  // 欠款还清
                    mp.DebtStatus = 2;

                if (mp.Type == "0" || mp.Type == "6")  // 储值卡还款
                {
                    mp.Amt = mp.Amt + amount;
                }

                // 加应收记录
                AccountRecord record = new AccountRecord
                {
                    MemberID = memberId,
                    HostID = hostId,
                    MemberCardId = memberCardId,
                    EventLogId = eventLog.LogId,
                    InAmount = amount,
                    Balance = mp.Amt,
                    PaymentType = "1",
                    Type = "1",
                    BranchId = branchId,
                    ClientID = "",
                    Remark = "还款",
                    CreatedDate = DateTime.Now,
                    SaleID = sales,
                    Splits = us,
                    IsVaild = 1
                };

                dbcontent.AccountRecords.Add(record);
            }

            dbcontent.SaveChanges();
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppRepayment(int HostId, int branchId, string ClientId, long memberId, long memberProjectId, long memberCardId,
            decimal amount, string UserId, string Password, string Salesman, decimal SalesRadix, string Workers, decimal WorkerRadix)
        {
            var result = new
            {
                code = 0,
                message = string.Empty
            };

            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID.Equals(memberId)).FirstOrDefault();
                if (member == null)
                {
                    result = new
                    {
                        code = 2,
                        message = "会员不存在。"
                    };
                    return Json(result);
                }
                if (Password.Trim() != member.Passwd)
                {
                    result = new
                    {
                        code = 2,
                        message = "会员密码不正确。"
                    };
                    return Json(result);
                }

                Payment(HostId, branchId, ClientId, memberId, memberProjectId, memberCardId, ClientId, amount, UserId, Salesman, SalesRadix,
                    Workers.Split(',').ToArray(), WorkerRadix);

                result = new
                {
                    code = 1,
                    message = String.Empty
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return Json(result);
        }
    }

}