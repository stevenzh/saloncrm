using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Common.Logging;
using EntityFramework.Extensions;
using Quartz;
using Newtonsoft.Json;
using SalonCRM.Models;
using SalonCRM.Manager;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using SalonCRM.Cache;

namespace SalonCRM.Job
{

    /// <summary>
    /// 5分钟
    /// </summary>
    public class FiveJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FiveJob));

        static ApplicationDbContext dbcontent = new ApplicationDbContext();

        public void Execute(IJobExecutionContext context)
        {
            logger.Info("FiveJob running...");

            var hosts = dbcontent.Hosts.Where(t => t.IsVaild == 1).ToList();
            foreach (Host h in hosts)
            {
                try
                {
                    #region 状态更新
                    var defaultV = dbcontent.Dictionaries.Where(t => t.HostId == h.HostID && t.Identifier == "MemberStatus" && t.IsVaild == 1 && t.IsDefault).FirstOrDefault();
                    if (defaultV != null)
                    {
                        DBHelper.ExecuteSql("update Members set Status ='" + defaultV.KeyValue + "' where HostID = " + h.HostID);
                    }

                    var rules = dbcontent.Dictionaries.Where(t => t.HostId == h.HostID && t.Identifier == "MemberStatus" && t.IsVaild == 1).OrderBy(t => t.SortOrder).ToList();
                    foreach (Dictionary dc in rules)
                    {
                        // "S1": "期间连续到店"  "S2": "期间有到过店" "S3": "期间没有到过店"
                        if (!string.IsNullOrEmpty(dc.Shell))
                        {
                            DictionaryViewModel vv = JsonConvert.DeserializeObject<DictionaryViewModel>(dc.Shell);
                            string MonthSet = vv.MonthSet;
                            string FMonth = vv.FMonth;  // 從
                            string SMonth = vv.SMonth;  // 到
                            string sql = "";

                            if (MonthSet == "S3")
                            {
                                sql = "update Members set Status ='" + dc.KeyValue + "' where MemberId not in (select Distinct MemberID from EventLog where";
                                sql += " CreatedDate > dateadd(MONTH,-" + SMonth + ",GETDATE()) and TypeId in (1,7,8,9,10) and HostID=" + h.HostID + " ) and JoinDate < dateadd(MONTH,-" + SMonth + ",GETDATE()) and HostID = " + h.HostID;
                            }
                            else if (MonthSet == "S2")
                            {
                                sql = "update Members set Status ='" + dc.KeyValue + "' where MemberId in (select Distinct MemberID from EventLog where";
                                sql += " CreatedDate > dateadd(MONTH,-" + SMonth + ",GETDATE()) and TypeId in (1,7,8,9,10) and HostID=" + h.HostID + " ) and HostID = " + h.HostID;
                            }
                            else if (MonthSet == "S1")             // 期间连续到店  有些复杂
                            {
                                sql = "update Members set Status ='" + dc.KeyValue + "' from (select MemberID, COUNT(LogId) as num from EventLog where";

                                if (SMonth == "2")    // 两个月连续到店
                                {
                                    sql += @" TypeId in (1,7,8,9,10) and CreatedDate > dateadd(MONTH,-1,GETDATE()) 
 group by MemberID ) a, 
(select MemberID, COUNT(LogId) as num from EventLog where CreatedDate > dateadd(MONTH, -2, GETDATE()) and TypeId in (1,7,8,9,10) and CreatedDate < dateadd(MONTH, -1,GETDATE())
 group by MemberID ) b
  where Members.MemberID = a.MemberID and Members.MemberID = b.MemberID and a.num > 0 and b.num > 0 and Members.HostID =" + h.HostID;

                                }
                                else if (SMonth == "3")   // 三个月连续到店
                                {
                                    sql += @" TypeId in (1,7,8,9,10) and CreatedDate > dateadd(MONTH,-1,GETDATE()) 
 group by MemberID ) a, 
(select MemberID, COUNT(LogId) as num from EventLog where TypeId in (1,7,8,9,10) and CreatedDate > dateadd(MONTH, -2, GETDATE()) and CreatedDate < dateadd(MONTH, -1,GETDATE())
 group by MemberID ) b,
(select MemberID, COUNT(LogId) as num from EventLog where TypeId in (1,7,8,9,10) and CreatedDate > dateadd(MONTH, -3, GETDATE()) and CreatedDate < dateadd(MONTH, -2,GETDATE())
 group by MemberID ) c
  where Members.MemberID = a.MemberID and Members.MemberID = b.MemberID and Members.MemberID = c.MemberID 
   and a.num > 0 and b.num > 0 and c.num > 0 and Members.HostID =" + h.HostID;
                                }
                            }

                            //logger.Info("Job Sql:" + sql);
                            DBHelper.ExecuteSql(sql);
                        }
                    }
                    #endregion

                    #region 会员等级
                    bool lDefault = false;
                    var levels = dbcontent.Dictionaries.Where(t => t.HostId == h.HostID && t.Identifier == "MemberLevel" && t.IsVaild == 1).OrderBy(t => t.SortOrder).ToList();
                    foreach (Dictionary dc in levels)
                    {
                        if (lDefault == false)
                        {
                            DBHelper.ExecuteSql("update Members set [Level] ='" + dc.KeyValue + "' where HostID=" + dc.HostId);
                            lDefault = true;
                            continue;
                        }

                        if (!string.IsNullOrEmpty(dc.Shell))
                        {
                            DictionaryViewModel vv = JsonConvert.DeserializeObject<DictionaryViewModel>(dc.Shell);
                            string MaxAmount = vv.MaxAmount;
                            string MinAmount = vv.MinAmount;

                            string sql = "update Members set [Level] = '" + dc.KeyValue + "' from (select MemberID, Sum(InAmount) as num from AccountRecords where ([Type] = 1 or [Type] = 2) and CreatedDate > dateadd(YEAR,-1,GETDATE()) group by MemberID ) b where b.MemberID = Members.MemberID and Members.HostID=" + dc.HostId;
                            sql += " and num >= " + MinAmount;
                            if (!string.IsNullOrEmpty(MaxAmount))
                                sql += " and num < " + MaxAmount;
                            DBHelper.ExecuteSql(sql);
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {

                    throw;
                }
            }


            // 会员余额， 储值卡之和
            DBHelper.ExecuteSql("update Members set Amt = isnull((select SUM(mc.Amt) from MemberCards mc where mc.MemberID = Members.MemberID and mc.Type='0' ), 0)");
            // 会员 来店次数
            DBHelper.ExecuteSql(@"update Members set BookTime = MM.row FROM (
select memberId, count(*) row from (
select distinct a.* from
(select MemberID, YEAR(CreatedDate) as y, MONTH(CreatedDate) as m , DAY(CreatedDate) as d from Books 
union
select MemberID, YEAR(CreatedDate) as y, MONTH(CreatedDate) as m , DAY(CreatedDate) as d from AccountRecords) a) b group by b.MemberID) MM where mm.MemberID = members.MemberID");

            // 会员最近生日
            if (DateTime.IsLeapYear(DateTime.Today.Year))  // 闰年 才有 2/29日
            {
                DBHelper.ExecuteSql("update Members set LastBirth = convert(varchar(4), Year(GETDATE()))+'-' + substring(convert(varchar(10),birthday,21),6,5) where Birthday is not null");
            }
            else
            {
                DBHelper.ExecuteSql("update Members set LastBirth = convert(varchar(4), Year(GETDATE()))+'-' + substring(convert(varchar(10),birthday,21),6,5) where Birthday is not null and substring(convert(varchar(10),birthday,21),6,5)<>'02-29' ");
            }
            DBHelper.ExecuteSql("update Members set LastBirth = DateAdd(Year, 1, LastBirth) where LastBirth < GETDATE()");

            // 积分失效扣除

            // 赠送项目过期扣除

            // 综合限时卡失效
            dbcontent.MemberCards.Where(t => t.Type == "4" || t.Type == "8").Where(t => t.ExpiryDate < DateTime.Today).Update(t => new MemberCard { Status = 0 });


            // 客户余额（各卡总和）
            DBHelper.ExecuteSql("update Members set amt = isnull((select sum(A.amt) from MemberCards A where A.MemberID = Members.MemberID),0)");
            // 客户预约微信提醒
            var pp = dbcontent.Appointments.Where(t => t.Approved == 1 && t.BookDate.Year == DateTime.Today.Year && t.BookDate.Month == DateTime.Today.Month && t.BookDate.Day == DateTime.Today.Day).ToList();
            foreach (var app in pp)
            {
                if (!string.IsNullOrEmpty(app.Member.OpenID))
                {
                    var oo = app.BookDate.AddHours(-1);
                    if (DateTime.Compare(DateTime.Now, oo) > 0 && DateTime.Compare(DateTime.Now.AddMinutes(5), oo) < 0)
                    {
                        var bag = (HostContainerBag)CacheContext.Current.Get("Host." + app.HostID);
                        var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
                        var testData = new
                        {
                            first = new TemplateDataItem(string.Format("您好，今天【{0}】服务，请按时到达。", "dd")),
                            keyword1 = new TemplateDataItem("客户：张某，电话：13888888888，服务项目：测试"),
                            keyword2 = new TemplateDataItem("2016-06-01 01:00"),
                            remark = new TemplateDataItem("点击预约详情")
                        };
                        string url = "http://cn.mdss.hk/wap/appointment/" + app.AppointmentID;
                        var result = TemplateApi.SendTemplateMessage(accessToken, app.Member.OpenID, bag.TmplMsg_Appointment, "#FF0000", url, testData);

                    }
                }
            }
            //HostContainer.TryGetItem("Host." + httpContext.Request.Url.Host);

            //#region 门店状态
            //foreach (Organ o in dbcontent.Organs.Where(t => t.IsVaild == 1 && t.Host.IsVaild == 1).ToList())
            //{
            //    // 当天
            //    var now = DateTime.Now;
            //    var d = dbcontent.BranchLogs.Where(t => t.LYear == now.Year && t.LMonth == now.Month && t.LDay == now.Day && t.BranchId == o.OrganID);
            //    var daycustomer = (from b in dbcontent.Books.Where(a => a.BranchId == o.OrganID   // a.State == "20" && 
            //                              && a.CreatedDate.Year == now.Year && a.CreatedDate.Month == now.Month && a.CreatedDate.Day == now.Day)
            //                       select b.MemberID)
            //        .Union(
            //        from mb in dbcontent.MemberProjects.Where(a => a.BranchId == o.OrganID && a.CreatedDate.Year == now.Year
            //                      && a.CreatedDate.Month == now.Month && a.CreatedDate.Day == now.Day)
            //        select mb.MemberID).Distinct().Count();  //当天客流量

            //    int worknum = dbcontent.Users.Where(a => a.OrganId == o.OrganID).Count();
            //    if (d.Count() > 0)
            //    {
            //        var dd = d.FirstOrDefault();
            //        dd.WorkerNum = worknum;
            //        dd.CustomerNum = daycustomer;
            //        dd.CreatedDate = now;
            //    }
            //    else
            //    {
            //        BranchLog w = new BranchLog
            //        {
            //            LYear = now.Year,
            //            LMonth = now.Month,
            //            LDay = now.Day,
            //            BranchId = o.OrganID,
            //            HostId = o.HostID,
            //            WorkerNum = worknum,
            //            CustomerNum = daycustomer,
            //            CreatedDate = now
            //        };
            //        dbcontent.BranchLogs.Add(w);
            //    }

            //    // 当月
            //    var s = dbcontent.BranchLogs.Where(a => a.BranchId == o.OrganID && a.LYear == now.Year && a.LMonth == now.Month && a.LDay > 0);
            //    int monthcount = s.Count() > 0 ? s.Sum(t => t.CustomerNum) : 0;
            //    var l = dbcontent.BranchLogs.Where(t => t.LYear == now.Year && t.LMonth == now.Month && t.LDay == 0 && t.BranchId == o.OrganID);
            //    if (l.Count() > 0)
            //    {
            //        var ll = l.FirstOrDefault();
            //        ll.WorkerNum = worknum;
            //        ll.CustomerNum = monthcount;
            //        ll.CreatedDate = now;
            //    }
            //    else
            //    {
            //        BranchLog e = new BranchLog
            //        {
            //            LYear = now.Year,
            //            LMonth = now.Month,
            //            BranchId = o.OrganID,
            //            HostId = o.HostID,
            //            WorkerNum = worknum,
            //            CustomerNum = monthcount,
            //            CreatedDate = now
            //        };
            //        dbcontent.BranchLogs.Add(e);
            //    }
            //}
            //dbcontent.SaveChanges();

            //#endregion

            // 更新客户是否“新客户”
            foreach (Host h in hosts)
            {
                var qq = dbcontent.HostProfiles.Where(t => t.HostID == h.HostID && t.PropertyText == "Setting_MemberNewStart").FirstOrDefault();
                if (qq != null)
                {
                    var dd = DateTime.Today.Year.ToString() + "-" + qq.PropertyValue;
                    if (DateTime.Parse(dd) > DateTime.Today)
                    {
                        dd = (DateTime.Today.Year - 1).ToString() + "-" + qq.PropertyValue;
                    }
                    DBHelper.ExecuteSql("update Members set IsNew = 0 where Type='L02' AND JoinDate <'" + dd + "' and HostID =" + h.HostID);
                    DBHelper.ExecuteSql("update Members set IsNew = 1 where Type='L02' AND JoinDate >'" + dd + "' and HostID =" + h.HostID);
                }
            }

            logger.Info("FiveJob run finished.");
        }
    }
}