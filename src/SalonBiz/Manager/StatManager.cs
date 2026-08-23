using System;
using System.Collections.Generic;
using System.Linq;
using SalonCRM.Models;
using System.Data;
using System.Text;
using SalonBiz.Models.Stat;

namespace SalonCRM.Manager
{
    public class StatManager
    {
        static ApplicationDbContext dbcontent = new ApplicationDbContext();


        /// <summary>
        /// 日报
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static IList<DailyViewModel> GetDailyList(int branchId, DateTime startDate, DateTime endDate)
        {
            List<DateTime> kk = new List<DateTime>();
            List<DailyViewModel> list = new List<DailyViewModel>();

            // 天数遍历
            for (var d = startDate; d.CompareTo(endDate) <= 0;)
            {
                kk.Add(d);

                DailyViewModel model = new DailyViewModel();
                model.TheDay = d;
                model.Flow = 0;

                var q = (from c in dbcontent.Books.Where(t => t.State == "20")
                         where c.BranchId == branchId && c.CreatedDate.Day == d.Day && c.CreatedDate.Year == d.Year && c.CreatedDate.Month == d.Month && c.CreatedDate.Day == d.Day
                         select c.MemberID)
                         .Union(from e in dbcontent.AccountRecords.Where(t => t.IsVaild == 1)
                                where e.BranchId == branchId && e.CreatedDate.Day == d.Day && e.CreatedDate.Year == d.Year && e.CreatedDate.Day == d.Day
                                && e.CreatedDate.Month == d.Month
                                select e.MemberID).Distinct();

                if (q.Count() > 0)
                    model.Flow = q.Count();  // 客流量
                model.BranchId = branchId;
                model.BranchName = dbcontent.Organs.Where(t => t.OrganID == branchId).FirstOrDefault().Name;

                // 实耗
                //-------------------------------------------------------------------------
                var pp = from dd in dbcontent.Books.Where(t => t.State == "20" && t.BranchId == branchId && t.CreatedDate.Day == d.Day
                                      && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month)
                         join mp in dbcontent.BookProjects on dd.BookID equals mp.BookID
                         select mp;
                if (pp.Count() > 0)
                {
                    // 实操
                    model.A1 = pp.Sum(t => t.Amount);
                    // 项目数
                    model.ProjectNum = pp.Sum(t => t.Quantity);
                }
                else
                    model.ProjectNum = 0;

                // 卡扣
                //-------------------------------------------------------------------------
                var mm = from dd in dbcontent.AccountRecords.Where(t => t.BranchId == branchId && t.IsVaild == 1 && t.CreatedDate.Day == d.Day
                                               && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month && t.Type == "3" && t.SalesType == 1)
                         select new Sample
                         {
                             Amount = dd.OutAmount
                         };
                if (mm.Count() > 0)
                    model.A3 = mm.Sum(t => t.Amount);
                //-------------------------------------------------------------------------
                // 销售
                var sl = dbcontent.MemberProjects.Where(t => t.BranchId == branchId && t.IsVaild == 1 && t.CreatedDate.Day == d.Day
                                    && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);

                var se = sl.Where(t => t.IsEntity == 1);
                if (se.Count() > 0)
                    model.A2 = se.Sum(t => t.Amount);   // 即销即耗

                var sn = sl.Count();
                if (sn > 0)
                {
                    var l = from dd in sl
                            join mp in dbcontent.Projects on dd.ProjectID equals mp.ProjectID
                            select new { dd.BookTime, dd.Amount, dd.ActualPrice, mp.Category } into zz
                            group zz by zz.Category into g
                            select new Sample
                            {
                                Categoty = g.Key,
                                Num = g.Sum(c => c.BookTime),
                                Amount = g.Sum(c => c.Amount),
                                ActualPrice = g.Sum(c => c.ActualPrice)
                            };
                    decimal num = 0;
                    decimal debt = 0;
                    foreach (Sample ss in l)
                    {
                        if (ss.Categoty.Equals("C01")) // 面部
                            model.S1 = ss.Amount;
                        else if (ss.Categoty.Equals("C02"))  // 身体
                            model.S2 = ss.Amount;
                        else if (ss.Categoty.Equals("C03"))   // 仪器
                            model.S3 = ss.Amount;
                        else if (ss.Categoty.Equals("C04"))   // 家居产品
                            model.S4 = ss.Amount;
                        else if (ss.Categoty.Equals("C05"))   // 其他
                            model.S5 = ss.Amount;
                        else
                            num += ss.Amount;  // 其他

                        debt += ss.Amount - ss.ActualPrice;
                    }
                    // 项目数
                    model.T4 = debt;
                }


                var cc = dbcontent.MemberCards.Where(t => t.BranchID == branchId && t.Status == 1 && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month
                   && t.CreatedDate.Day == d.Day && (new string[] { "0", "4", "5", "8" }).Contains(t.Type));
                if (cc.Count() > 0)
                    model.S6 = cc.Sum(t => t.Amount);


                // -------------------------------------------------------------------------------------------
                // 业绩
                var ww = dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2").Where(t => t.BranchId == branchId && t.IsVaild == 1
                                      && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);
                var wl = ww.Count();
                if (wl > 0)
                {
                    var l = from dd in ww
                            group dd by dd.PaymentType into g
                            select new Sample
                            {
                                Categoty = g.Key,
                                Amount = g.Sum(c => c.InAmount)
                            };
                    foreach (Sample ss in l)
                    {
                        if (ss.Categoty.Equals("1"))  // 现金
                            model.T1 = ss.Amount;
                        if (ss.Categoty.Equals("2"))  // 刷卡
                            model.T2 = ss.Amount;
                        if (ss.Categoty.Equals("3"))   // 转账
                            model.T3 = ss.Amount;
                    }
                }
                if (wl > 0)
                {
                    var l = from dd in ww
                            group dd by dd.Member.IsNew into g
                            select new
                            {
                                Categoty = g.Key,
                                Amount = g.Sum(c => c.InAmount)
                            };
                    foreach (var ss in l)
                    {
                        if (ss.Categoty == 1)  // 新会员
                            model.N2 = ss.Amount;
                        else
                            model.N1 = ss.Amount;
                    }
                }

                // 欠款
                var ere = dbcontent.MemberProjects.Where(t => t.BranchId == branchId && t.IsVaild == 1 && t.DebtFlag == 1
                    && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);
                var ere2 = dbcontent.MemberCards.Where(t => t.BranchID == branchId && t.DebtFlag == 1 && t.Status == 1
                  && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);
                if (ere.Count() > 0)
                    model.T4 = ere.Sum(t => t.Amount - t.ActualPrice);
                if (ere2.Count() > 0)
                    model.T4 = (model.T4 == null ? 0 : model.T4) + ere2.Sum(t => t.Amount - t.ActualPrice);

                list.Add(model);
                d = d.AddDays(1);
            }

            return list;
        }

        public static MonthViewModel GetMonthStat(int branchId, DateTime startDate, DateTime endDate)
        {
            MonthViewModel model = new MonthViewModel();

            var q = dbcontent.Books.Where(c => c.BranchId == branchId && c.State == "20" && c.CreatedDate > startDate && c.CreatedDate < endDate)
                     .Select(c => new { MemberId = c.MemberID, Year = c.CreatedDate.Year, Month = c.CreatedDate.Month, Day = c.CreatedDate.Day })
                     .Union(dbcontent.AccountRecords.Where(e => e.BranchId == branchId && e.IsVaild == 1 && e.CreatedDate > startDate && e.CreatedDate < endDate)
                     .Select(e => new { MemberId = e.MemberID, Year = e.CreatedDate.Year, Month = e.CreatedDate.Month, Day = e.CreatedDate.Day })).Distinct();

            if (q.Count() > 0)
                model.Flow = q.Count();  // 客流量
            model.BranchId = branchId;
            model.BranchName = dbcontent.Organs.Where(t => t.OrganID == branchId).FirstOrDefault().Name;

            // 实耗
            //-------------------------------------------------------------------------
            var pp = from dd in dbcontent.Books.Where(t => t.State == "20" && t.BranchId == branchId && t.CreatedDate > startDate && t.CreatedDate < endDate)
                     join mp in dbcontent.BookProjects on dd.BookID equals mp.BookID
                     select mp;
            if (pp.Count() > 0)
            {
                // 实操
                model.A1 = pp.Sum(t => t.Amount);
                // 项目数
                model.ProjectNum = pp.Sum(t => t.Quantity);
            }
            else
                model.ProjectNum = 0;

            // 卡扣
            //-------------------------------------------------------------------------
            var mm = from dd in dbcontent.AccountRecords.Where(t => t.BranchId == branchId && t.IsVaild == 1 && t.CreatedDate > startDate
                                           && t.CreatedDate > endDate && t.Type == "3" && t.SalesType == 1)
                     select new Sample
                     {
                         Amount = dd.OutAmount
                     };
            if (mm.Count() > 0)
                model.A3 = mm.Sum(t => t.Amount);

            //-------------------------------------------------------------------------
            // 销售
            var sl = dbcontent.MemberProjects.Where(t => t.BranchId == branchId && t.IsVaild == 1 && t.CreatedDate > startDate && t.CreatedDate < endDate);

            var se = sl.Where(t => t.IsEntity == 1);
            if (se.Count() > 0)
                model.A2 = se.Sum(t => t.Amount);   // 即销即耗

            var sn = sl.Count();
            if (sn > 0)
            {
                var l = from dd in sl
                        join mp in dbcontent.Projects on dd.ProjectID equals mp.ProjectID
                        select new { dd.BookTime, dd.Amount, dd.ActualPrice, mp.Category } into zz
                        group zz by zz.Category into g
                        select new Sample
                        {
                            Categoty = g.Key,
                            Num = g.Sum(c => c.BookTime),
                            Amount = g.Sum(c => c.Amount),
                            ActualPrice = g.Sum(c => c.ActualPrice)
                        };
                decimal num = 0;
                decimal debt = 0;
                foreach (Sample ss in l)
                {
                    if (ss.Categoty.Equals("C01")) // 面部
                        model.S1 = ss.Amount;
                    else if (ss.Categoty.Equals("C02"))  // 身体
                        model.S2 = ss.Amount;
                    else if (ss.Categoty.Equals("C03"))   // 仪器
                        model.S3 = ss.Amount;
                    else if (ss.Categoty.Equals("C04"))   // 家居产品
                        model.S4 = ss.Amount;
                    else if (ss.Categoty.Equals("C05"))   // 其他
                        model.S5 = ss.Amount;
                    else
                        num += ss.Amount;  // 其他

                    debt += ss.Amount - ss.ActualPrice;
                }
                // 项目数
                model.T4 = debt;
            }


            var cc = dbcontent.MemberCards.Where(t => t.BranchID == branchId && t.Status == 1 && t.CreatedDate > startDate
               && t.CreatedDate < endDate && (new string[] { "0", "4", "5", "7", "8" }).Contains(t.Type));
            if (cc.Count() > 0)
                model.S6 = cc.Sum(t => t.Amount);


            // -------------------------------------------------------------------------------------------
            // 业绩
            var ww = dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2").Where(t => t.BranchId == branchId && t.IsVaild == 1
                                  && t.CreatedDate > startDate && t.CreatedDate < endDate);
            var wl = ww.Count();
            if (wl > 0)
            {
                var l = from dd in ww
                        group dd by dd.PaymentType into g
                        select new Sample
                        {
                            Categoty = g.Key,
                            Amount = g.Sum(c => c.InAmount)
                        };
                foreach (Sample ss in l)
                {
                    if (ss.Categoty.Equals("1"))  // 现金
                        model.T1 = ss.Amount;
                    if (ss.Categoty.Equals("2"))  // 刷卡
                        model.T2 = ss.Amount;
                    if (ss.Categoty.Equals("3"))   // 转账
                        model.T3 = ss.Amount;
                }
            }
            if (wl > 0)
            {
                var l = from dd in ww
                        group dd by dd.Member.IsNew into g
                        select new
                        {
                            Categoty = g.Key,
                            Amount = g.Sum(c => c.InAmount)
                        };
                foreach (var ss in l)
                {
                    if (ss.Categoty == 1)  // 新会员
                        model.N2 = ss.Amount;
                    else if (ss.Categoty == 0)
                        model.N1 = ss.Amount;
                    else
                        model.N3 = ss.Amount;
                }
            }

            // 欠款
            var ere = dbcontent.MemberProjects.Where(t => t.BranchId == branchId && t.IsVaild == 1 && t.DebtFlag == 1
                && t.CreatedDate > startDate && t.CreatedDate < endDate);
            var ere2 = dbcontent.MemberCards.Where(t => t.BranchID == branchId && t.DebtFlag == 1 && t.Status == 1
              && t.CreatedDate > startDate && t.CreatedDate < endDate);
            if (ere.Count() > 0)
                model.T4 = ere.Sum(t => t.Amount - t.ActualPrice);
            if (ere2.Count() > 0)
                model.T4 = (model.T4 == null ? 0 : model.T4) + ere2.Sum(t => t.Amount - t.ActualPrice);

            return model;
        }

        public static List<CardViewModel> GetCardStatList(CardQModel model)
        {
            var end = model.EndDate.AddDays(1);
            var query = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1 && t.Type != "9" && t.CreatedDate > model.StartDate && t.CreatedDate < end);
            var query1 = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1 && t.Type != "9" && t.CreatedDate > model.StartDate && t.CreatedDate < end);

            if (model.BranchID != default(int))
            {
                query = query.Where(t => t.BranchID == model.BranchID);
                query1 = query1.Where(t => t.BranchID == model.BranchID);
            }
            if (!string.IsNullOrEmpty(model.CardTmplID))
            {
                int id = Convert.ToInt32(model.CardTmplID);
                query = query.Where(t => t.TmplID == id);
                query1 = query1.Where(t => t.TmplID == id);
            }
            if (!string.IsNullOrEmpty(model.CardType))
            {
                query = query.Where(t => t.Type == model.CardType);
                query1 = query1.Where(t => t.Type == model.CardType);
            }

            var list = (from dd in query
                        group dd by dd.Type into g
                        select new CardViewModel
                        {
                            Type = g.Key,
                            TypeValue = dbcontent.Dictionaries.Where(t => t.Identifier == "MemberCardType" && t.KeyValue == g.Key).FirstOrDefault().Contents,
                            Quantity = g.Count(),
                            Amount = g.Sum(c => c.Amount),
                            Amt = g.Sum(c => c.Amt),
                            BookTime = g.Sum(c => c.BookTime),
                            LastCount = g.Sum(c => c.LastCount),
                            Person = query1.Where(t => t.Type == g.Key).Select(t => t.MemberID).Distinct().Count()
                        }).ToList();

            return list;
        }

        public static List<MemberCard> GetCardList(CardQModel model)
        {
            var end = model.EndDate.AddDays(1);
            var query = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1 && t.CreatedDate > model.StartDate && t.CreatedDate < end);
            if (model.BranchID != default(int))
                query = query.Where(t => t.BranchID == model.BranchID);
            if (!string.IsNullOrEmpty(model.CardTmplID))
            {
                int id = Convert.ToInt32(model.CardTmplID);
                query = query.Where(t => t.TmplID == id);
            }
            if (!string.IsNullOrEmpty(model.CardType))
                query = query.Where(t => t.Type == model.CardType);

            var list = query.ToList();

            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="qmodel"></param>
        /// <returns></returns>
        public static List<GoodsViewModel> GetGoods(GoodsQModel qmodel)
        {
            var d = qmodel.EndDate.AddDays(1);

            // 消耗
            var book = dbcontent.Books.Where(t => t.HostID == qmodel.HostID && t.State == "20" && t.CreatedDate > qmodel.StartDate && t.CreatedDate < d);
            if (qmodel.BranchID != null)
                book = book.Where(t => t.BranchId == qmodel.BranchID.Value);
            var query = dbcontent.BookGoods.AsQueryable();
            if (!string.IsNullOrEmpty(qmodel.GoodsName))
                query = query.Where(t => t.Goods.Name.Contains(qmodel.GoodsName));
            if (!string.IsNullOrEmpty(qmodel.Category))
                query = query.Where(t => t.Goods.Category == qmodel.Category);

            // 即销即耗
            var book1 = dbcontent.MemberProjects.Where(t => t.HostID == qmodel.HostID && t.IsVaild == 1 && t.IsEntity == 1 && t.CreatedDate > qmodel.StartDate && t.CreatedDate < d);
            if (qmodel.BranchID != null)
                book1 = book1.Where(t => t.BranchId == qmodel.BranchID.Value);
            var query1 = dbcontent.MemberProjectGoods.AsQueryable();
            if (!string.IsNullOrEmpty(qmodel.GoodsName))
                query1 = query1.Where(t => t.Goods.Name.Contains(qmodel.GoodsName));
            if (!string.IsNullOrEmpty(qmodel.Category))
                query1 = query1.Where(t => t.Goods.Category == qmodel.Category);

            var list1 = (from ll in book
                         join gg in query on ll.BookID equals gg.BookID
                         select new { id = gg.BookGoodsID, GoodsID = gg.GoodsID, Name = gg.Goods.Name, Category = gg.Goods.Category, Quantity = gg.Quantity, Unit = gg.Goods.Unit })
                         .Union
                        (from ll in book1
                         join gg in query1 on ll.MemberProjectId equals gg.MemberProjectId
                         select new { id = ll.MemberProjectId, GoodsID = gg.GoodsID, Name = gg.Goods.Name, Category = gg.Goods.Category, Quantity = gg.Quantity, Unit = gg.Goods.Unit });


            var list = (from zz in list1
                        group zz by new { zz.GoodsID, zz.Name, zz.Category, zz.Unit } into g
                        select new GoodsViewModel
                        {
                            GoodsID = g.Key.GoodsID,
                            Name = g.Key.Name,
                            Category = g.Key.Category,
                            CategoryText = dbcontent.Dictionaries.Where(t => t.Identifier == "ProjectCategory" && t.KeyValue == g.Key.Category).FirstOrDefault().Contents,
                            Quantity = g.Sum(t => t.Quantity),
                            Unit = g.Key.Unit
                        }).ToList();
            return list;
        }


        /// <summary>
        /// 日报详细 -B版
        /// </summary>
        /// <param name="branchId">门店</param>
        /// <param name="d">日期</param>
        /// <returns></returns>
        //public static IList<CustomerDailyViewModel> GetOneDailyDetail(int branchId, DateTime d)
        //{
        //    List<DateTime> kk = new List<DateTime>();
        //    List<CustomerDailyViewModel> list = new List<CustomerDailyViewModel>();

        //    //有记录的客户
        //    var q = (from c in dbcontent.Books
        //             where c.BranchId == branchId && c.CreatedDate.Day == d.Day && c.CreatedDate.Year == d.Year && c.CreatedDate.Month == d.Month
        //             select c.MemberID)
        //                 .Union(from e in dbcontent.MemberProjects
        //                        where e.BranchId == branchId && e.CreatedDate.Day == d.Day && e.CreatedDate.Year == d.Year
        //                        && e.CreatedDate.Month == d.Month
        //                        select e.MemberID).Distinct();

        //    //单条生成
        //    foreach (long m in q.ToList())
        //    {
        //        List<CustomerDailyViewModel> alist = new List<CustomerDailyViewModel>();

        //        // 消费
        //        var slist = (from dd in dbcontent.Books.Where(t => t.State == "20" && t.MemberID == m && t.BranchId == branchId
        //            && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month)
        //                     join mp in dbcontent.BookProjects on dd.BookID equals mp.BookID
        //                     join pj in dbcontent.Projects on mp.ProjectID equals pj.ProjectID
        //                     select new CustomerDailyViewModel
        //                     {
        //                         MemberId = m,
        //                         ServiceProjectName = pj.Name,
        //                         ServiceProjectNum = mp.Quantity,
        //                         ExpenseAmount = mp.Amount,
        //                         Workers = mp.UserSplits,
        //                         ExpenseTime = dd.CreatedDate
        //                     }).ToList();

        //        // 销售
        //        var elist = (from dd in dbcontent.AccountRecords.Where(t => t.MemberID == m && t.BranchId == branchId
        //                                && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month)
        //                     join mp in dbcontent.MemberProjects on dd.RecordID equals mp.AccountRecordID
        //                     join p in dbcontent.Projects on mp.ProjectID equals p.ProjectID
        //                     select new CustomerDailyViewModel
        //                     {
        //                         ProjectName = p.Name,
        //                         ProjectNum = mp.BookTime,
        //                         Amount = mp.Amount,
        //                         Debt = mp.Amount - mp.ActualPrice,
        //                         Sales = dd.Splits,
        //                         SalesTime = dd.CreatedDate
        //                     }).ToList();

        //        // 收款
        //        var rlist = (from dd in dbcontent.AccountRecords.Where(t => t.MemberID == m && t.BranchId == branchId
        //            && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month)
        //                     select new CustomerDailyViewModel
        //                     {
        //                         RechangeType = dd.PaymentType,
        //                         RechargeAmount = dd.OutAmount,
        //                         RechangeSales = dd.Splits,
        //                         RechangeTime = dd.CreatedDate
        //                     }).ToList();

        //        int row = 0;
        //        if (slist.Count() > 0) row = slist.Count();
        //        if (elist.Count() > 0 && elist.Count() > row) row = elist.Count();
        //        if (rlist.Count() > 0 && rlist.Count() > row) row = rlist.Count();

        //        for (int i = 0; i < row; i++)
        //        {
        //            CustomerDailyViewModel model = new CustomerDailyViewModel();
        //            model.TheDay = d;

        //            if (i == 0)
        //            {
        //                model.BranchId = branchId;
        //                model.BranchName = dbcontent.Organs.Where(t => t.OrganID == branchId).FirstOrDefault().Name;
        //                var mb = dbcontent.Members.Where(a => a.MemberID == m).FirstOrDefault();
        //                model.MemberId = mb.MemberID;
        //                model.CardNo = mb.CardNo;
        //                model.MemberName = mb.Name;
        //            }

        //            if (slist.Count() > i)
        //            {
        //                model.ServiceProjectName = slist[i].ServiceProjectName;
        //                model.ServiceProjectNum = slist[i].ServiceProjectNum;
        //                model.ExpenseAmount = slist[i].ExpenseAmount;
        //                model.Workers = slist[i].Workers;
        //                model.Worker = string.Join(",", slist[i].Workers.Select(t => t.User.UserCnName).ToArray());
        //                model.ExpenseTime = slist[i].ExpenseTime;
        //            }
        //            if (elist.Count() > i)
        //            {
        //                model.ProjectName = elist[i].ProjectName;
        //                model.ProjectNum = elist[i].ProjectNum;
        //                model.Amount = elist[i].Amount;
        //                model.Sales = elist[i].Sales;
        //                model.SalesTime = elist[i].SalesTime;
        //            }
        //            if (rlist.Count() > i)
        //            {
        //                model.RechangeType = rlist[i].RechangeType;
        //                model.RechargeAmount = rlist[i].RechargeAmount;
        //                model.RechangeSales = rlist[i].RechangeSales;
        //                model.RechangeTime = rlist[i].RechangeTime;
        //            }

        //            list.Add(model);
        //        }
        //    }

        //    return list;
        //}


        /// <summary>
        /// 日报详细
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static IList<DailyViewModel> GetOneDailyList(int branchId, DateTime d)
        {
            List<DateTime> kk = new List<DateTime>();
            List<DailyViewModel> list = new List<DailyViewModel>();
            var q = (from c in dbcontent.Books
                     where c.BranchId == branchId && c.CreatedDate.Day == d.Day && c.CreatedDate.Year == d.Year && c.CreatedDate.Month == d.Month
                     select c.MemberID)
                        .Union(from e in dbcontent.AccountRecords
                               where e.BranchId == branchId && e.CreatedDate.Day == d.Day && e.CreatedDate.Year == d.Year
                               && e.CreatedDate.Month == d.Month
                               select e.MemberID)
                                .Union(from e in dbcontent.MemberCards
                                       where e.BranchID == branchId && e.CreatedDate.Day == d.Day && e.CreatedDate.Year == d.Year
                                       && e.CreatedDate.Month == d.Month
                                       select e.MemberID).Distinct();
            int row = 0;
            foreach (long m in q.ToList())  // 会员列表
            {
                DailyViewModel model = new DailyViewModel();
                model.TheDay = d;
                model.RowNum = ++row;
                var mb = dbcontent.Members.Where(a => a.MemberID == m).FirstOrDefault();
                model.MemberId = mb.MemberID;
                model.CardNo = mb.CardNo;
                model.MemberName = mb.Name;


                // 销售
                var sl = dbcontent.MemberProjects.Where(t => t.MemberID == m && t.IsVaild == 1 && t.BranchId == branchId
                                && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);

                var se = sl.Where(t => t.IsEntity == 1);
                if (se.Count() > 0)
                    model.A2 = se.Sum(t => t.Amount);   // 即销即耗

                var sn = sl.Count();
                if (sn > 0)
                {
                    var l = from dd in sl
                            join mp in dbcontent.Projects on dd.ProjectID equals mp.ProjectID
                            select new { dd.BookTime, dd.Amount, dd.ActualPrice, mp.Category } into zz
                            group zz by zz.Category into g
                            select new Sample
                            {
                                Categoty = g.Key,
                                Num = g.Sum(c => c.BookTime),
                                Amount = g.Sum(c => c.Amount),
                                ActualPrice = g.Sum(c => c.ActualPrice)
                            };
                    decimal num = 0;
                    decimal debt = 0;
                    foreach (Sample ss in l)
                    {
                        if (ss.Categoty.Equals("C01"))        // 面部
                            model.S1 = ss.Amount;
                        else if (ss.Categoty.Equals("C02"))   // 身体
                            model.S2 = ss.Amount;
                        else if (ss.Categoty.Equals("C03"))   // 仪器
                            model.S3 = ss.Amount;
                        else if (ss.Categoty.Equals("C04"))   // 家居产品
                            model.S4 = ss.Amount;
                        else if (ss.Categoty.Equals("C05"))   // 其他
                            model.S5 = ss.Amount;
                        else
                            num += ss.Amount;  // 其他
                        debt += ss.Amount - ss.ActualPrice;
                    }
                    // 项目数
                    model.T4 = debt;
                }

                // 卡项
                var cc = dbcontent.MemberCards.Where(t => t.MemberID == m && t.BranchID == branchId && t.Status == 1
                           && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month
                           && t.CreatedDate.Day == d.Day && (new string[] { "0", "4", "5", "8" }).Contains(t.Type));
                if (cc.Count() > 0)
                    model.S6 = cc.Sum(t => t.Amount);
                // --------------------------------------------------------------------------------------
                // 收款
                var ww = dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2")
                       .Where(t => t.MemberID == m && t.BranchId == branchId && t.IsVaild == 1
                      && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);
                var wl = ww.Count();
                if (wl > 0)
                {
                    var l = from dd in ww
                            group dd by dd.PaymentType into g
                            select new Sample
                            {
                                Categoty = g.Key,
                                Amount = g.Sum(c => c.InAmount)
                            };
                    decimal num = 0;
                    foreach (Sample ss in l)
                    {
                        num += ss.Amount;
                        if (ss.Categoty.Equals("1"))  // 现金
                            model.T1 = ss.Amount;
                        if (ss.Categoty.Equals("2"))  // 刷卡
                            model.T2 = ss.Amount;
                        if (ss.Categoty.Equals("3"))   // 转账
                            model.T3 = ss.Amount;
                    }
                }

                // 欠款
                var ere = dbcontent.MemberProjects.Where(t => t.MemberID == m && t.BranchId == branchId && t.DebtFlag == 1 && t.IsVaild == 1
                    && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);
                var ere2 = dbcontent.MemberCards.Where(t => t.MemberID == m && t.BranchID == branchId && t.DebtFlag == 1 && t.Status == 1
                  && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month);
                if (ere.Count() > 0)
                    model.T4 = ere.Sum(t => t.Amount - t.ActualPrice);
                if (ere2.Count() > 0)
                    model.T4 = (model.T4 == null ? 0 : model.T4) + ere2.Sum(t => t.Amount - t.ActualPrice);

                // 卡扣
                //-------------------------------------------------------------------------
                var mm = from dd in dbcontent.AccountRecords.Where(t => t.BranchId == branchId && t.MemberID == m && t.IsVaild == 1
                         && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month && t.Type == "3" && t.SalesType == 1)
                         select new Sample
                         {
                             Amount = dd.OutAmount
                         };
                if (mm.Count() > 0)
                    model.A3 = mm.Sum(t => t.Amount);
                //-------------------------------------------------------------------------

                // 消耗
                var slist = (from dd in dbcontent.Books.Where(t => t.State == "20" && t.MemberID == m && t.BranchId == branchId
                                       && t.CreatedDate.Day == d.Day && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month)
                             join mp in dbcontent.BookProjects on dd.BookID equals mp.BookID
                             join pj in dbcontent.Projects on mp.ProjectID equals pj.ProjectID
                             join mrp in dbcontent.MemberProjects on mp.MemberProjectId equals mrp.MemberProjectId into qp
                             from mrp in qp.DefaultIfEmpty()
                             select new DailyViewModel
                             {
                                 MemberId = m,
                                 ddType = mp.Type,
                                 ProjectName = pj.Name + (mrp.Type == "1" ? "[赠]" : "") + (mrp.Type == "2" ? "[赠]" : ""),
                                 ProjectNum = mp.Quantity,
                                 ExpenseAmount = mp.Amount,
                                 Splits = mp.UserSplits,
                                 ExpenseTime = dd.CreatedDate
                             }).ToList();

                if (slist.Count() > 0)
                {
                    model.ProjectName = slist[0].ProjectName;
                    model.ProjectNum = slist[0].ProjectNum;
                    model.Splits = slist[0].Splits;
                    model.Worker = string.Join(",", slist[0].Splits.Select(t => t.User.UserCnName).ToArray());
                    model.ExpenseTime = slist[0].ExpenseTime;
                    model.A1 = slist[0].ExpenseAmount; // 实操
                }
                list.Add(model);


                if (slist.Count() > 1)
                {
                    for (int i = 1; i < slist.Count(); i++)
                    {
                        DailyViewModel model1 = new DailyViewModel();

                        model1.ProjectName = slist[i].ProjectName;
                        model1.ProjectNum = slist[i].ProjectNum;
                        model1.Splits = slist[i].Splits;
                        model1.Worker = string.Join(",", slist[i].Splits.Select(t => t.User.UserCnName).ToArray());
                        model1.ExpenseTime = slist[i].ExpenseTime;
                        model1.A1 = slist[i].ExpenseAmount;  // 实操

                        list.Add(model1);
                    }
                }
            }
            return list;
        }


        class Sample
        {
            public string Categoty { get; set; }
            public int Type { get; set; }
            /// <summary>
            /// 应付
            /// </summary>
            public decimal Amount { get; set; }
            /// <summary>
            /// 实付
            /// </summary>
            public decimal ActualPrice { get; set; }
            public int Num { get; set; }
        }


        /// <summary>
        /// 应收一览
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="Salesman"></param>
        /// <returns></returns>
        public static List<ReceivablesViewModel> GetReceivables(ReceivablesQModel model)
        {
            var query = dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2");
            if (model.BranchId != default(int))
                query = query.Where(a => a.BranchId == model.BranchId);
            if (model.StartDate != default(DateTime))
                query = query.Where(a => a.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                query = query.Where(a => a.CreatedDate < d);
            }
            //if (!string.IsNullOrEmpty(Salesman))
            //{
            //    var u = dbcontent.Users.Where(a => a.Id == Salesman).FirstOrDefault();
            //    if (u != null)
            //        query = query.Where(a => a.Salesmans.Contains(u));
            //}
            if (!string.IsNullOrEmpty(model.Salesman))
            {
                var u = dbcontent.Users.Where(a => a.Id == model.Salesman).FirstOrDefault();
                if (u != null)
                    query = query.Where(a => a.Splits.Select(t => t.UserID).Contains(model.Salesman));
            }

            var mb = (from v in query
                      select new ReceivablesViewModel
                      {
                          _Salesman = v.Splits,
                          BranchName = dbcontent.Organs.Where(t => t.OrganID == v.BranchId).FirstOrDefault().Name,
                          MemberName = v.Member.Name,
                          PaymentType = v.PaymentType,
                          Sales = v.OutAmount,
                          Cash = (v.PaymentType.Equals("1") ? v.InAmount : 0),
                          CardMoney = (v.PaymentType.Equals("2") ? v.InAmount : 0),
                          Transfer = (v.PaymentType.Equals("3") ? v.InAmount : 0),
                          CreatedDate = v.CreatedDate,
                      }).ToList();
            foreach (var bp in mb)
            {
                bp.Salesman = String.Join(",", bp._Salesman.Select(t => t.User.UserCnName).ToArray());
            }

            return mb;
        }


        /// <summary>
        /// 欠款一览
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<DebtViewModel> GetDebtList(DebtQModel model)
        {
            #region 项目欠款

            var query = dbcontent.MemberProjects.Where(t => t.DebtFlag == 1 && t.IsVaild == 1 && t.HostID == model.HostId);
            var log = dbcontent.AccountRecords.Where(t => t.IsVaild == 1 && t.Type == "3");
            if (model.BranchId != default(int))
                query = query.Where(t => t.BranchId == model.BranchId);
            if (!string.IsNullOrEmpty(model.Salesman))
            {
                var u = dbcontent.Users.Where(t => t.Id == model.Salesman).FirstOrDefault();
                log = log.Where(t => t.SaleID == u.Id);
            }
            if (!string.IsNullOrEmpty(model.CardNo))
                query = query.Where(t => t.Member.CardNo == model.CardNo);
            if (model.StartDate != default(DateTime))
                query = query.Where(t => t.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                query = query.Where(t => t.CreatedDate < d);
            }

            var v = (from vm in query
                     join ll in log on vm.AccountRecordID equals ll.RecordID
                     select new DebtViewModel
                     {
                         MemberProjectId = vm.MemberProjectId,
                         Amount = vm.Amount,
                         Debt = vm.Amount - vm.ActualPrice,
                         ProjectName = vm.Project.Name,
                         Quantity = vm.BookTime,
                         _Salesman = ll.Splits,
                         CreatedDate = vm.CreatedDate,
                         MemberName = vm.Member.Name,
                         CardNo = vm.Member.CardNo,
                         Status = vm.status,
                         BranchName = dbcontent.Organs.Where(t => t.OrganID == vm.BranchId).FirstOrDefault().Name
                     }).ToList();

            foreach (var bp in v)
            {
                bp.Salesman = String.Join(",", bp._Salesman.Select(t => t.User.UserCnName).ToArray());
            }

            #endregion

            var query1 = dbcontent.MemberCards.Where(t => t.DebtFlag == 1 && t.Status == 1 && t.HostID == model.HostId);
            if (model.BranchId != default(int))
                query1 = query1.Where(t => t.BranchID == model.BranchId);
            var log2 = dbcontent.AccountRecords.Where(t => t.Type == "2" && t.IsVaild == 1);
            if (!string.IsNullOrEmpty(model.Salesman))
            {
                var u = dbcontent.Users.Where(t => t.Id == model.Salesman).FirstOrDefault();
                log2 = log2.Where(t => t.SaleID == u.Id);
            }
            if (!string.IsNullOrEmpty(model.CardNo))
                query1 = query1.Where(t => t.Member.CardNo == model.CardNo);
            if (model.StartDate != default(DateTime))
                query1 = query1.Where(t => t.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                query1 = query1.Where(t => t.CreatedDate < d);
            }
            var v2 = (from vm in query1
                      join ll in log2 on vm.MemberCardId equals ll.MemberCardId
                      select new DebtViewModel
                      {
                          MemberCardId = vm.MemberCardId,
                          Amount = vm.Amount,
                          Debt = vm.Amount - vm.ActualPrice,
                          CardTitle = vm.Title,
                          Quantity = vm.BookTime,
                          _Salesman = ll.Splits,
                          CreatedDate = vm.CreatedDate,
                          MemberName = vm.Member.Name,
                          CardNo = vm.Member.CardNo,
                          Status = vm.DebtStatus,
                          BranchName = dbcontent.Organs.Where(t => t.OrganID == vm.BranchID).FirstOrDefault().Name
                      }).ToList();

            foreach (var bp in v2)
            {
                bp.Salesman = String.Join(",", bp._Salesman.Select(t => t.User.UserCnName).ToArray());
            }

            return v.Union(v2).ToList(); ;
        }


        /// <summary>
        /// 门店排名
        /// </summary>
        /// <returns></returns>
        public static IList<BranchRankingViewModel> GetBranchRankList(BranchQModel model)
        {
            var query = dbcontent.Organs.Where(a => a.HostID == model.HostID);
            var end = model.EndDate.AddDays(1);
            var list = (from mb in query
                        select new BranchRankingViewModel
                        {
                            BranchId = mb.OrganID,
                            BranchName = mb.Name,
                            StatDate = "All"
                        }).ToList();

            foreach (var ll in list)
            {
                var q1 = dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2").Where(t => t.BranchId == ll.BranchId && t.IsVaild == 1
                                    && t.CreatedDate > model.StartDate && t.CreatedDate < end);
                if (q1.Count() > 0)
                    ll.Income = q1.Sum(t => t.InAmount);   // 业绩
                var q2 = dbcontent.BookProjects.Where(b => b.Book.State == "20" && b.Book.BranchId == ll.BranchId
                                   && b.Book.CreatedDate > model.StartDate && b.Book.CreatedDate < end);
                if (q2.Count() > 0)
                    ll.ExpenseAmount = q2.Sum(t => t.Amount);  // 实耗
                var q3 = dbcontent.MemberProjects.Where(t => t.BranchId == ll.BranchId && t.IsEntity == 1 && t.IsVaild == 1
                            && t.CreatedDate > model.StartDate && t.CreatedDate < end);
                if (q3.Count() > 0)
                    ll.EntityAmount = q3.Sum(t => t.Amount);    // 即销即耗
                var q4 = dbcontent.AccountRecords.Where(t => t.BranchId == ll.BranchId && t.Type == "3" && t.SalesType == 1 && t.IsVaild == 1
                               && t.CreatedDate > model.StartDate && t.CreatedDate < end);
                if (q4.Count() > 0)
                    ll.SalesAmount = q4.Sum(t => t.OutAmount);     // 卡扣
                var q5 = dbcontent.AccountRecords.Where(t => t.BranchId == ll.BranchId && t.Type == "3" && t.SalesType == 2 && t.IsVaild == 1
                               && t.CreatedDate > model.StartDate && t.CreatedDate < end);
                if (q5.Count() > 0)
                    ll.CashSalesAmount = q5.Sum(t => t.OutAmount);     // 卡扣

            }

            return list;
        }


        /// <summary>
        /// 客户统计
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="memberType"></param>
        /// <param name="memberLevel"></param>
        /// <param name="memberGender"></param>
        /// <param name="name"></param>
        /// <param name="cardNo"></param>
        /// <param name="inCount"></param>
        /// <param name="joinStart"></param>
        /// <param name="joinEnd"></param>
        /// <param name="amtStart"></param>
        /// <param name="amtEnd"></param>
        /// <param name="useStart"></param>
        /// <param name="useEnd"></param>
        /// <returns></returns>
        public static IList<CustomerSTAViewModel> GetCustomerSAList(int hostId, int branchId, string memberType, string memberLevel,
            string memberGender, string name, string cardNo, int inCount, DateTime joinStart, DateTime joinEnd, decimal amtStart,
            decimal amtEnd, decimal useStart, decimal useEnd)
        {
            var query = dbcontent.Members.Where(t => t.HostID == hostId);
            if (branchId != default(int))
                query = query.Where(a => a.JoinBranch == branchId);
            if (!string.IsNullOrEmpty(memberType))
                query = query.Where(a => a.Type == memberType);
            if (!string.IsNullOrEmpty(memberLevel))
                query = query.Where(a => a.Level == memberLevel);
            if (!string.IsNullOrEmpty(memberGender))
                query = query.Where(a => a.Sex == memberGender);
            if (!string.IsNullOrEmpty(name))
                query = query.Where(a => a.Name.Contains(name));
            if (!string.IsNullOrEmpty(cardNo))
                query = query.Where(a => a.CardNo == cardNo);
            if (inCount != default(int))
                query = query.Where(a => a.BookTime == inCount);
            if (joinStart != default(DateTime))
                query = query.Where(a => a.JoinDate >= joinStart);
            if (joinEnd != default(DateTime))
            {
                var dd = joinEnd.AddDays(1);
                query = query.Where(a => a.JoinDate < dd);
            }

            var list = from mb in query
                       select new CustomerSTAViewModel
                       {
                           BranchId = mb.JoinBranch,
                           BranchName = dbcontent.Organs.Where(t => t.OrganID == mb.JoinBranch).FirstOrDefault().Name,
                           MemberId = mb.MemberID,
                           Name = mb.Name,
                           CardNo = mb.CardNo,
                           JoinDate = mb.JoinDate,
                           MobileNumber = mb.MobileNumber,
                           BookTime = mb.BookTime,
                           ProjectNumber = mb.MemberCards.Sum(a => a.BookTime),
                           RemainingNumber = mb.MemberCards.Sum(a => a.LastCount),
                           RechargeAmount = dbcontent.AccountRecords.Where(b => b.MemberID == mb.MemberID && b.IsVaild == 1).Where(b => b.Type == "1" || b.Type == "2").Sum(t => t.InAmount),
                           ExpenseAmount = mb.Books.Sum(t => t.Amount),
                           RemainingAmount = (dbcontent.MemberCards.Where(b => b.MemberID == mb.MemberID && b.Status == 1).Count() > 0 ? dbcontent.MemberCards.Where(b => b.MemberID == mb.MemberID && b.Status == 1).Sum(t => t.Amt) : 0),
                           Status = dbcontent.Dictionaries.Where(a => a.KeyValue == mb.Status && a.HostId == hostId && a.Identifier == "MemberStatus").FirstOrDefault().Contents,
                           // Level = mb.Level,
                           Level = dbcontent.Dictionaries.Where(a => a.KeyValue == mb.Level && a.HostId == hostId && a.Identifier == "MemberLevel").FirstOrDefault().Contents,
                           Type = dbcontent.Dictionaries.Where(a => a.KeyValue == mb.Type && a.Identifier == "MemberType").FirstOrDefault().Contents,
                           LastService = mb.Books.Max(a => a.CreatedDate)
                       };

            if (amtStart != default(decimal))
                list = list.Where(t => t.RechargeAmount > amtStart);
            if (amtEnd != default(decimal))
                list = list.Where(t => t.RechargeAmount < amtEnd);
            if (useStart != default(decimal))
                list = list.Where(t => t.ExpenseAmount > useStart);
            if (useEnd != default(decimal))
                list = list.Where(t => t.ExpenseAmount < useEnd);

            return list.ToList();
        }


        /// <summary>
        /// 项目销售、消耗一览表
        /// </summary>
        /// <returns></returns>
        public static List<ProjectSTAViewModel> GetProjectList(ProjectQModel model)
        {
            var prj = dbcontent.Projects.Where(t => t.HostID == model.HostID);
            if (!string.IsNullOrEmpty(model.ProjectName))
                prj = prj.Where(t => t.Name.Contains(model.ProjectName));
            if (!string.IsNullOrEmpty(model.BrandCode))
                prj = prj.Where(t => t.Brand == model.BrandCode);
            if (!string.IsNullOrEmpty(model.Category))
                prj = prj.Where(t => t.Category == model.Category);
            if (!string.IsNullOrEmpty(model.ExtCategory))
                prj = prj.Where(t => t.ExtCategory == model.ExtCategory);

            // 销售
            var sale = dbcontent.MemberProjects.Where(t => t.HostID == model.HostID && t.IsVaild == 1);
            if (model.BranchID != default(int))
                sale = sale.Where(t => t.BranchId == model.BranchID);
            if (model.StartDate != default(DateTime))
                sale = sale.Where(t => t.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                sale = sale.Where(t => t.CreatedDate < d);
            }
            var mc = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1);
            if (!string.IsNullOrEmpty(model.CardType))
                mc = mc.Where(t => t.Type == model.CardType);


            // 用于销售占比
            decimal allsales = 0;
            if (sale.Count() > 0) allsales = sale.Sum(t => t.Amount);


            // 消耗
            var serv = dbcontent.BookProjects.Where(t => t.Book.HostID == model.HostID && t.Book.State == "20");
            if (model.BranchID != default(int))
                serv = serv.Where(t => t.Book.BranchId == model.BranchID);
            if (model.StartDate != default(DateTime))
                serv = serv.Where(t => t.Book.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                serv = serv.Where(t => t.Book.CreatedDate < d);
            }
            var mc1 = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1);
            if (!string.IsNullOrEmpty(model.CardType))
                mc1 = mc1.Where(t => t.Type == model.CardType);


            // 计算人头
            var pp = from dt in (from ll in sale select new { ProjectID = ll.ProjectID, MemberID = ll.MemberID })
                     .Union(from ss in serv select new { ProjectID = ss.ProjectID, MemberID = ss.Book.MemberID }).Distinct()
                     group dt by dt.ProjectID into ggg
                     select new
                     {
                         ProjectID = ggg.Key,
                         Person = ggg.Count()
                     };
            // 实操人头
            var sc = from qq in serv
                     group qq by qq.ProjectID into ggg
                     select new
                     {
                         ProjectID = ggg.Key,
                         Person = ggg.Count()
                     };

            // 项目销售
            var gl = from ll in sale
                     join mcs in mc on ll.MemberCardId equals mcs.MemberCardId
                     group ll by ll.ProjectID into g
                     select new PModel
                     {
                         ProjectId = g.Key,
                         SaleTimes = g.Sum(t => t.BookTime),
                         Amount = g.Sum(t => t.Amount),
                         UsedAmount = 0,
                         UsedTimes = 0
                     };
            // 项目消耗
            var gs = from dd in serv
                     join mm in mc1 on dd.MemberCardId equals mm.MemberCardId
                     group dd by dd.ProjectID into gg
                     select new PModel
                     {
                         ProjectId = gg.Key,
                         SaleTimes = 0,
                         Amount = 0,
                         UsedAmount = gg.Sum(t => t.Amount),
                         UsedTimes = gg.Sum(t => t.Quantity)
                     };

            var l = (from p in prj
                     join cc in pp on p.ProjectID equals cc.ProjectID
                     join pd in sc on p.ProjectID equals pd.ProjectID into op
                     from p2 in op.DefaultIfEmpty(new { ProjectID = p.ProjectID, Person = 0 })
                     join se in gl on p.ProjectID equals se.ProjectId into ob
                     from o2 in ob.DefaultIfEmpty(new PModel { ProjectId = p.ProjectID, SaleTimes = 0, Amount = 0, UsedAmount = 0, UsedTimes = 0 })
                     join ss in gs on p.ProjectID equals ss.ProjectId into os
                     from s2 in os.DefaultIfEmpty(new PModel { ProjectId = p.ProjectID, SaleTimes = 0, Amount = 0, UsedAmount = 0, UsedTimes = 0 })
                     select new ProjectSTAViewModel
                     {
                         BranchID = model.BranchID,
                         ProjectId = p.ProjectID,
                         ProjectName = p.Name,
                         Brand = dbcontent.Dictionaries.Where(t => t.HostId == model.HostID && t.KeyValue == p.Brand).FirstOrDefault().Contents,
                         Category = dbcontent.Dictionaries.Where(t => t.KeyValue == p.Category && t.Identifier == "ProjectCategory").FirstOrDefault().Contents,
                         SaleTimes = o2.SaleTimes,
                         Amount = o2.Amount,
                         SalesPercent = (allsales == 0 ? 0 : o2.Amount / allsales),
                         UsedAmount = s2.UsedAmount,
                         UsedCount = s2.UsedTimes,
                         PersonCount = cc.Person,
                         ServiceCount = p2.Person
                     }).ToList();
            return l;
        }


        /// <summary>
        /// 项目销售列表
        /// </summary>
        /// <param name="qmodel"></param>
        /// <returns></returns>
        public static List<ProjectSTAModel> GetProjectSelesList(ProjectQModel model)
        {
            var sale = dbcontent.MemberProjects.Where(t => t.HostID == model.HostID && t.IsVaild == 1 && t.ProjectID == model.ProjectID);
            if (model.BranchID != default(int))
                sale = sale.Where(t => t.BranchId == model.BranchID);
            if (model.StartDate != default(DateTime))
                sale = sale.Where(t => t.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                sale = sale.Where(t => t.CreatedDate < d);
            }
            var mc = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1);
            if (!string.IsNullOrEmpty(model.CardType))
                mc = mc.Where(t => t.Type == model.CardType);

            var gl = (from ll in sale
                      join mcs in mc on ll.MemberCardId equals mcs.MemberCardId
                      select new ProjectSTAModel
                      {
                          MemberID = ll.MemberID,
                          MemberName = ll.Member.Name,
                          Quantity = ll.BookTime,
                          UnitPrice = ll.UnitPrice,
                          Amount = ll.Amount,
                          CreatedDate = ll.CreatedDate,
                          UserSplits = dbcontent.AccountRecords.Where(t => t.RecordID == ll.AccountRecordID).FirstOrDefault()
                      }).ToList();
            return gl;
        }


        /// <summary>
        /// 项目销售列表
        /// </summary>
        /// <param name="qmodel"></param>
        /// <returns></returns>
        public static List<ProjectSTAModel> GetProjectServiceList(ProjectQModel model)
        {
            var serv = dbcontent.BookProjects.Where(t => t.Book.HostID == model.HostID && t.Book.State == "20" && t.ProjectID == model.ProjectID);
            if (model.BranchID != default(int))
                serv = serv.Where(t => t.Book.BranchId == model.BranchID);
            if (model.StartDate != default(DateTime))
                serv = serv.Where(t => t.Book.CreatedDate > model.StartDate);
            if (model.EndDate != default(DateTime))
            {
                var d = model.EndDate.AddDays(1);
                serv = serv.Where(t => t.Book.CreatedDate < d);
            }
            var mc1 = dbcontent.MemberCards.Where(t => t.HostID == model.HostID && t.Status == 1);
            if (!string.IsNullOrEmpty(model.CardType))
                mc1 = mc1.Where(t => t.Type == model.CardType);

            var gl = (from ll in serv
                      join mcs in mc1 on ll.MemberCardId equals mcs.MemberCardId
                      select new ProjectSTAModel
                      {
                          MemberID = ll.Book.MemberID,
                          MemberName = ll.Book.Member.Name,
                          Quantity = ll.Quantity,
                          UnitPrice = ll.Amount / ll.Quantity,
                          Amount = ll.Amount,
                          CreatedDate = ll.Book.CreatedDate,
                          BookUserSplits = ll.UserSplits
                      }).ToList();
            return gl;
        }


        /// <summary>
        /// 客户消费/消耗一览表
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="Name"></param>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public static IList<CustomerProjectViewModel> GetCustomerPTList(int hostId, int branchId, DateTime startDate, DateTime endDate,
            string Name, string CardNo)
        {
            var end = endDate.AddDays(1);
            var list = (from mb in dbcontent.AccountRecords.Where(t => t.Type == "1" || t.Type == "2")
                                     .Where(t => t.HostID == hostId && t.BranchId == branchId && t.IsVaild == 1 && t.CreatedDate > startDate && t.CreatedDate < end)
                        select new CustomerProjectViewModel
                        {
                            MemberId = mb.MemberID,
                            Name = mb.Member.Name,
                            CardNo = mb.Member.CardNo,
                            BranchId = mb.BranchId,
                            BranchName = dbcontent.Organs.Where(t => t.OrganID == mb.BranchId).FirstOrDefault().Name,
                            RechargeAmount = mb.InAmount,
                            // RemaindAmount = mb.Balance,
                            // ExpenseAmount = 0,
                            CreatedDate = mb.CreatedDate
                        }).ToList();


            var list2 = (from mb in dbcontent.MemberProjects.Where(t => t.HostID == hostId && t.BranchId == branchId && t.IsVaild == 1
                                    && t.CreatedDate > startDate && t.CreatedDate < end)
                         select new CustomerProjectViewModel
                         {
                             MemberId = mb.MemberID,
                             Name = mb.Member.Name,
                             CardNo = mb.Member.CardNo,
                             BranchId = mb.BranchId,
                             BranchName = dbcontent.Organs.Where(t => t.OrganID == mb.BranchId).FirstOrDefault().Name,
                             ProjectCode = mb.Project.Code,
                             ProjectName = mb.Project.Name,
                             BookTime = mb.BookTime,
                             ExpenseAmount = mb.Amount,
                             CreatedDate = mb.CreatedDate
                         }).ToList();

            return list.Union(list2).ToList();
        }


        /// <summary>
        /// 客户消费明细
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="branchId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public static IList<CustomerProjectViewModel> GetCustomerPTDetail(int hostId, int branchId, DateTime startDate, DateTime endDate, string CardNo)
        {
            var end = endDate.AddDays(1);
            var query = dbcontent.Members.Where(t => t.HostID == hostId && t.JoinBranch == branchId);
            var list = (from mb in query
                        select new CustomerProjectViewModel
                        {
                            MemberId = mb.MemberID,
                            Name = mb.Name,
                            CardNo = mb.CardNo,
                            BranchId = mb.JoinBranch,
                            BranchName = dbcontent.Organs.Where(t => t.OrganID == mb.JoinBranch).FirstOrDefault().Name,
                            RechargeAmount = mb.AccountRecords.Where(b => b.Type == "1" || b.Type == "2").Where(b => b.IsVaild == 1 && b.CreatedDate > startDate && b.CreatedDate < end).Sum(t => t.InAmount),
                            ExpenseAmount = mb.Books.Where(b => b.State == "20" && b.CreatedDate > startDate && b.CreatedDate < end).Sum(t => t.Amount)
                        }).Where(t => t.RechargeAmount != 0 || t.ExpenseAmount != 0).ToList();


            var foo = dbcontent.MemberProjects.Where(t => t.HostID == hostId && t.BranchId == branchId && t.IsVaild == 1 && t.CreatedDate > startDate && t.CreatedDate < end);
            var list2 = (from mb in foo
                         select new CustomerProjectViewModel
                         {
                             MemberId = mb.MemberID,
                             Name = mb.Member.Name,
                             CardNo = mb.Member.CardNo,
                             BranchId = mb.BranchId,
                             BranchName = dbcontent.Organs.Where(t => t.OrganID == mb.BranchId).FirstOrDefault().Name,
                             ProjectCode = mb.Project.Code,
                             ProjectName = mb.Project.Name,
                             ExpenseAmount = mb.Amount,
                             BookTime = mb.BookTime,
                             CreatedDate = mb.CreatedDate
                         }).ToList();

            return list.Union(list2).ToList();
        }


        /// <summary>
        /// 客户赠送统计
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns></returns>
        public static List<GiveModel> Customer_Giving_statistical(int hostId, int branch, DateTime start, DateTime end)
        {
            var d = end.AddDays(1);
            var query = dbcontent.MemberGives.Where(t => t.HostID == hostId && t.IsVaild == 1 && t.CreatedDate > start && t.CreatedDate < d);
            if (branch != default(int))
                query = query.Where(t => t.BranchId == branch);

            var list = (from c in query
                        join pp in dbcontent.Projects on c.ProjectID equals pp.ProjectID into gp
                        from gvp in gp.DefaultIfEmpty()
                        join dp in dbcontent.Organs on c.BranchId equals dp.OrganID
                        select new GiveModel
                        {
                            GiveId = c.GiveId,
                            BranchName = dp.Name,
                            InPoints = c.InPoints,
                            RemainPoints = c.RemainPoints,
                            BookTime = c.BookTime,
                            ExpiryDate = c.ExpiryDate,
                            LastCount = c.LastCount,
                            MemberID = c.MemberID,
                            MemberName = c.Member.Name,
                            MemberCardNo = c.Member.CardNo,
                            ProjectName = gvp.Name,
                            CreatedDate = c.CreatedDate
                        }).ToList();

            var list2 = (from e in dbcontent.BookProjects.Where(t => t.Book.State == "20")
                         join mp in dbcontent.MemberProjects.Where(t => t.IsVaild == 1) on e.MemberProjectId equals mp.MemberProjectId
                         join mg in query on mp.GiveId equals mg.GiveId
                         where mg.HostID == hostId
                         select new GiveModel
                         {
                             GiveId = mg.GiveId,
                             //InPoints = mg.InPoints,
                             //RemainPoints = mg.RemainPoints,
                             //Project = mg.Project,
                             //BookTime = mg.BookTime,
                             //ExpiryDate = mg.ExpiryDate,
                             //LastCount = mg.LastCount,
                             //MemberID = mg.MemberID
                             ServiceDate = e.Book.CreatedDate,
                             FinalProject = mp.Project.Name
                         }).ToList();

            return list.Union(list2).OrderBy(t => t.GiveId).ToList();
        }


        /// <summary>
        /// 美容师排名
        /// </summary>
        /// <param name="qmodel"></param>
        /// <returns></returns>
        public static List<WorkerRankModel> WorkServiceRnking(WorkerQModel qmodel)
        {
            var endDate = qmodel.EndDate.AddDays(1);
            List<WorkerRankModel> list = new List<WorkerRankModel>();

            string sql = @"select kk.*, u.UserCnName, u.Type 
from ApplicationUser u,
(select bpw.UserId from Books b inner join BookProjects bp on b.BookID = bp.BookID 
inner join BookProjectSplits bpw on bpw.BookProjectID = bp.BookProjectID 
where b.hostid = " + qmodel.HostID + " and b.CreatedDate >'" + qmodel.StartDate.ToShortDateString() + @"' and b.CreatedDate <'" + endDate.ToShortDateString() + @"'
union
select mp.UserId from AccountRecords m inner 
join AccountRecordSplits mp on m.RecordID = mp.RecordID
where m.HostID = " + qmodel.HostID + " and m.CreatedDate>'" + qmodel.StartDate.ToShortDateString() + @"' and m.CreatedDate<'" + endDate.ToShortDateString() + @"'
union
select b.SalesmanID UserId from Books b
where b.HostID = " + qmodel.HostID + " and b.CreatedDate>'" + qmodel.StartDate.ToShortDateString() + @"' and b.CreatedDate<'" + endDate.ToShortDateString() + @"') kk
where kk.UserId = u.Id and u.HostID = " + qmodel.HostID;

            if (qmodel.BranchID != default(int))
            {
                sql += " and u.OrganId=" + qmodel.BranchID;
            }
            if (!string.IsNullOrEmpty(qmodel.WorkerType))
            {
                sql += " and u.Type='" + qmodel.WorkerType + "' ";
            }
            if (!string.IsNullOrEmpty(qmodel.WorkerName))
            {
                sql += " and u.UserCnName like '%" + qmodel.WorkerName + "%' ";
            }
            DataTable dt = DBHelper.GetDataSet(sql.ToString());
            var workers = dt.AsEnumerable();

            // 集合日期段内有操作的美容师
            // 遍历 每一个美容师  计算每一个值
            foreach (var item in workers)
            {
                WorkerRankModel model = new WorkerRankModel();
                string userid = item.Field<string>("UserId");
                model.WorkerName = item.Field<string>("UserCnName");
                model.Type = item.Field<string>("Type");
                model.Worker = userid;

                // 业绩
                var ye = dbcontent.AccountRecordSplits.Where(t => t.Record.Type == "1" || t.Record.Type == "2")
                       .Where(t => t.Record.IsVaild == 1 && t.Record.CreatedDate > qmodel.StartDate && t.Record.CreatedDate < endDate
                         && t.UserID == userid);
                if (!string.IsNullOrEmpty(qmodel.MemberType))
                {
                    ye = ye.Where(t => t.Record.Member.Type == qmodel.MemberType);
                }
                if (!string.IsNullOrEmpty(qmodel.MemberNewType))
                {
                    int v = Int32.Parse(qmodel.MemberNewType);
                    ye = ye.Where(t => t.Record.Member.Type == "L02" && t.Record.Member.IsNew == v);
                }
                if (ye.Count() > 0)
                    model.InCome = ye.Sum(t => t.Amount);   // 业绩

                // 实耗
                var expend = (from b in dbcontent.Books.Where(t => t.State == "20" && t.CreatedDate > qmodel.StartDate && t.CreatedDate < endDate)
                              join bp in dbcontent.BookProjects on b.BookID equals bp.BookID
                              join bps in dbcontent.BookProjectSplits on bp.BookProjectID equals bps.BookProjectID
                              where bps.UserID == userid
                              select new { MemberId = b.MemberID, ppa = bps.Amount, pp = bp.Quantity, mt = b.Member.Type, mn = b.Member.IsNew, BookDate = b.CreatedDate });

                if (!string.IsNullOrEmpty(qmodel.MemberType))
                {
                    expend = expend.Where(t => t.mt == qmodel.MemberType);
                }
                if (!string.IsNullOrEmpty(qmodel.MemberNewType))
                {
                    int v = Int32.Parse(qmodel.MemberNewType);
                    expend = expend.Where(t => t.mn == v);
                }

                if (expend.Count() > 0)
                {
                    model.ServiceXC = expend.Sum(t => t.pp);         // 服务项目量
                    model.ServiceExpend = expend.Sum(t => t.ppa);    // 美容师实耗
                    model.ServiceRC = expend.ToList().Select(t => new { a = t.BookDate.ToShortDateString(), m = t.MemberId }).Distinct().Count(); // 服务人次
                    model.ServiceRT = expend.Select(t => t.MemberId).Distinct().Count();
                }

                // 服务人头数
                var expend1 = from b in dbcontent.Books.Where(t => t.State == "20" && t.CreatedDate > qmodel.StartDate && t.CreatedDate < endDate)
                              join bp in dbcontent.BookProjects on b.BookID equals bp.BookID
                              where b.SalesmanID == userid
                              select new { Type = b.Member.Type, IsNew = b.Member.IsNew, ppa = bp.Amount };
                if (!string.IsNullOrEmpty(qmodel.MemberType))
                {
                    expend1 = expend1.Where(t => t.Type == qmodel.MemberType);
                }
                if (!string.IsNullOrEmpty(qmodel.MemberNewType))
                {
                    int v = Int32.Parse(qmodel.MemberNewType);
                    expend1 = expend1.Where(t => t.IsNew == v);
                }
                if (expend1.Count() > 0)
                {
                    model.SalesServiceExpend = expend1.Sum(t => t.ppa);  // 顾问消耗业绩
                }

                // 顾问客户接待量
                if (model.Type == "3")
                {
                    model.SaleRS = dbcontent.Books.Where(t => t.State == "20" && t.CreatedDate > qmodel.StartDate && t.CreatedDate < endDate && t.SalesmanID == userid)
                        .ToList().Select(c => new { MemberID = c.MemberID, DT = c.CreatedDate.ToShortDateString() })
                            .Union(dbcontent.AccountRecords.Where(t => t.IsVaild == 1 && t.CreatedDate > qmodel.StartDate && t.CreatedDate < endDate && t.SaleID == userid)
                                .ToList().Select(c => new { MemberID = c.MemberID, DT = c.CreatedDate.ToShortDateString() }))
                                .Distinct().Count();
                }

                // 即销即耗
                var qq = from ars in dbcontent.AccountRecordSplits.Where(t => t.Record.IsVaild == 1 && t.Record.Type == "3"
                                 && t.Record.CreatedDate > qmodel.StartDate && t.Record.CreatedDate < endDate && t.UserID == userid)
                         join mp in dbcontent.MemberProjects.Where(t => t.IsEntity == 1) on ars.RecordID equals mp.AccountRecordID
                         select ars;

                if (!string.IsNullOrEmpty(qmodel.MemberType))
                {
                    qq = qq.Where(t => t.Record.Member.Type == qmodel.MemberType);
                }
                if (!string.IsNullOrEmpty(qmodel.MemberNewType))
                {
                    int v = Int32.Parse(qmodel.MemberNewType);
                    qq = qq.Where(t => t.Record.Member.Type == "L02" && t.Record.Member.IsNew == v);
                }
                if (qq.Count() > 0)
                    model.EntityExpend = qq.Sum(t => t.Amount);

                // 虚耗
                var vv = dbcontent.AccountRecordSplits.Where(t => t.Record.IsVaild == 1 && t.Record.Type == "3" && t.Record.CreatedDate > qmodel.StartDate
                                    && t.Record.CreatedDate < endDate && t.UserID == userid);
                if (!string.IsNullOrEmpty(qmodel.MemberType))
                {
                    vv = vv.Where(t => t.Record.Member.Type == qmodel.MemberType);
                }
                if (!string.IsNullOrEmpty(qmodel.MemberNewType))
                {
                    int v = Int32.Parse(qmodel.MemberNewType);
                    vv = vv.Where(t => t.Record.Member.Type == "L02" && t.Record.Member.IsNew == v);
                }

                //var rt = (from ll in vv
                //          join ee in dbcontent.MemberProjects on ll.RecordID equals ee.AccountRecordID
                //          select ee.Amount).Sum();
                if (vv.Count() > 0)
                    model.Expend = vv.Sum(t => t.Amount);

                list.Add(model);
            }

            if (qmodel.Sort == "0")   // 业绩
                list.Sort((emp2, emp1) => (emp1.InCome == null ? 0 : emp1.InCome.Value).CompareTo(emp2.InCome == null ? 0 : emp2.InCome.Value));
            else if (qmodel.Sort == "1")   // 消耗
                list.Sort((emp2, emp1) => (emp1.Expend == null ? 0 : emp1.Expend.Value).CompareTo(emp2.Expend == null ? 0 : emp2.Expend.Value));
            else if (qmodel.Sort == "2")  // 实操
                list.Sort((emp2, emp1) => (emp1.ServiceExpend == null ? 0 : emp1.ServiceExpend.Value).CompareTo(emp2.ServiceExpend == null ? 0 : emp2.ServiceExpend.Value));
            else if (qmodel.Sort == "3")  // 即消即耗
                list.Sort((emp2, emp1) => (emp1.EntityExpend == null ? 0 : emp1.EntityExpend.Value).CompareTo(emp2.EntityExpend == null ? 0 : emp2.EntityExpend.Value));
            else if (qmodel.Sort == "4")  // 服务人次
                list.Sort((emp2, emp1) => (emp1.ServiceRC == null ? 0 : emp1.ServiceRC.Value).CompareTo(emp2.ServiceRC == null ? 0 : emp2.ServiceRC.Value));
            else if (qmodel.Sort == "5")  // 服务项目量
                list.Sort((emp2, emp1) => (emp1.ServiceXC == null ? 0 : emp1.ServiceXC.Value).CompareTo(emp2.ServiceXC == null ? 0 : emp2.ServiceXC.Value));
            return list;
        }

        /// <summary>
        /// 美容师排名-服务明细
        /// </summary>
        /// <returns></returns>
        public List<WorkerRankDetailModel> WorkServiceDetails(string userId, DateTime StartDate, DateTime EndDate)
        {
            var d = EndDate.AddDays(1);
            List<WorkerRankDetailModel> list = new List<WorkerRankDetailModel>();

            // 业绩
            var ld = (from b in dbcontent.AccountRecordSplits.Where(t => t.Record.Type == "1" || t.Record.Type == "2")
                      .Where(t => t.Record.IsVaild == 1 && t.Record.CreatedDate > StartDate && t.Record.CreatedDate < d
                      && t.UserID == userId)
                      select new WorkerRankDetailModel
                      {
                          TheTime = b.Record.CreatedDate,
                          MemberName = b.Record.Member.Name,
                          IncomeTime = b.Record.CreatedDate,
                          InCome = b.Amount
                      }).ToList();

            // 购卡记录
            var kk = (from b in dbcontent.AccountRecordSplits.Where(t => t.Record.IsVaild == 1 && t.Record.Type == "3" && t.Record.CreatedDate > StartDate
                        && t.Record.CreatedDate < d && t.UserID == userId)
                      //join mp in dbcontent.MemberProjects on b.RecordID equals mp.AccountRecordID
                      select new WorkerRankDetailModel
                      {
                          TheTime = b.Record.CreatedDate,
                          MemberName = b.Record.Member.Name,
                          SalesTime = b.Record.CreatedDate,
                          InSales = b.Amount,
                          //IsEntity = mp.IsEntity
                      }).ToList();


            // 美容师消耗
            var expend = (from b in dbcontent.Books.Where(t => t.State == "20" && t.CreatedDate > StartDate && t.CreatedDate < d)
                          join bp in dbcontent.BookProjects on b.BookID equals bp.BookID
                          join bps in dbcontent.BookProjectSplits on bp.BookProjectID equals bps.BookProjectID
                          where bps.UserID == userId
                          select new WorkerRankDetailModel
                          {
                              TheTime = b.CreatedDate,
                              MemberName = b.Member.Name,
                              ExpendTime = b.CreatedDate,
                              Expend = bps.Amount,
                              ProjectName = bp.Project.Name,
                              ServiceXC = bp.Quantity,
                              HandicraftFee = bps.HandicraftFee
                          }).ToList();


            var expend1 = (from b in dbcontent.Books.Where(t => t.State == "20" && t.CreatedDate > StartDate && t.CreatedDate < d)
                           where b.SalesmanID == userId
                           select new WorkerRankDetailModel
                           {
                               TheTime = b.CreatedDate,
                               MemberName = b.Member.Name,
                               ExpendTime = b.CreatedDate,
                               SalesExpend = b.Amount,
                               ProjectName = b.BookProjects.FirstOrDefault().Project.Name,
                               ServiceXC = b.BookProjects.Sum(t => t.Quantity)
                           }).ToList();

            list.AddRange(ld);
            list.AddRange(kk);
            list.AddRange(expend);
            list.AddRange(expend1);
            return list;
        }


        /// <summary>
        /// 返回预约到店成交统计
        /// </summary>
        /// <param name="sDate">查询开始日期</param>
        /// <param name="eDate">查询结束日期</param>
        /// <returns></returns>
        public List<CustomerYViewModel> Customer_transaction(int hostId, int branch, DateTime startDate, DateTime endDate)
        {
            List<CustomerYViewModel> list = new List<CustomerYViewModel>();
            // 天数遍历
            for (var d = startDate; d.CompareTo(endDate) <= 0;)
            {
                CustomerYViewModel model = new CustomerYViewModel();
                model.BookDate = d;
                model.BranchName = (branch == default(int) ? "全部" : dbcontent.Organs.Where(t => t.OrganID == branch).FirstOrDefault().Name);

                var q1 = dbcontent.Appointments.Where(t => t.HostID == hostId && t.BookDate.Year == d.Year && t.BookDate.Month == d.Month
                                                 && t.BookDate.Day == d.Day);
                var q2 = dbcontent.AccountRecords.Where(t => t.HostID == hostId && t.IsVaild == 1 && t.CreatedDate.Year == d.Year
                                            && t.CreatedDate.Month == d.Month && t.CreatedDate.Day == d.Day);
                var q3 = dbcontent.Books.Where(t => t.HostID == hostId && t.State == "20" && t.CreatedDate.Year == d.Year
                                        && t.CreatedDate.Month == d.Month && t.CreatedDate.Day == d.Day);
                if (branch != default(int))
                {
                    q1 = q1.Where(t => t.BranchId == branch);
                    q2 = q2.Where(t => t.BranchId == branch);
                    q3 = q3.Where(t => t.BranchId == branch);
                }
                model.AppointmentPax = q1.Distinct().Count();
                model.InPax = q2.Select(t => t.MemberID).Union(q3.Select(t => t.MemberID)).Distinct().Count();


                var q5 = dbcontent.Books.Where(t => t.HostID == hostId && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month
                     && t.CreatedDate.Day == d.Day && t.State == "20" && t.Member.Type == "L02" && t.Member.IsNew == 0);
                var q6 = dbcontent.Books.Where(t => t.HostID == hostId && t.CreatedDate.Year == d.Year && t.CreatedDate.Month == d.Month
                     && t.CreatedDate.Day == d.Day && t.State == "20" && t.Member.Type == "L02" && t.Member.IsNew == 1);

                if (branch != default(int))
                {
                    q5 = q5.Where(t => t.BranchId == branch);
                    q6 = q6.Where(t => t.BranchId == branch);
                }
                if (q5.Count() > 0)
                {
                    model.BookPax = q5.Select(t => t.MemberID).Distinct().Count();
                    model.BookAmount = q5.Sum(t => t.Amount);
                }
                if (q6.Count() > 0)
                {
                    model.NewBookPax = q6.Select(t => t.MemberID).Distinct().Count();
                    model.NewBookAmount = q6.Sum(t => t.Amount);
                }

                list.Add(model);
                d = d.AddDays(1);
            }

            return list;
        }

        /// <summary>
        /// 客户考勤月报表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DataTable Customer_receptions(int hostId, int branchId, string year, string memberType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select ROW_NUMBER() OVER (ORDER BY OrganID) as Record,* from (
                                select gs.OrganID,gs.Name as dianmian,kh.Name,kh.CardNo,kh.MobileNumber,dic.Contents as [Type]
                                ,M1.LDay as Jan,M2.LDay as Feb,M3.LDay as Mar
                                ,M4.LDay as Apr,M5.LDay as May,M6.LDay as Jun
                                ,M7.LDay as Jul,M8.LDay as Aug,M9.LDay as Sep
                                ,M10.LDay as Oct,M11.LDay as Nov,M12.LDay as [Dec]
                               from Members kh inner join Organ gs on 1 = 1 
                                 left join Dictionary dic on kh.Type=dic.KeyValue");
            for (int i = 1; i <= 12; i++)
            {
                sql.Append(@" left join 
                                    (select tt.MemberID, tt.BranchId, COUNT(tt.lday) LDay from (
     select yy.MemberID,yy.BranchId, DAY(yy.CreatedDate) lday from Books yy where yy.State='20' and Month(yy.CreatedDate)=" + i.ToString() + " and year(yy.CreatedDate)=" + year +
     " union " +
                //" select dd.MemberID, dd.BranchId, DAY(dd.CreateDate) lday from MemberProjects dd where Month(dd.CreateDate)=" + i.ToString() + " and year(dd.CreateDate)=" + year + " ) tt " +
                //" group by tt.MemberID, tt.BranchId ) as M" + i.ToString() + " on kh.MemberID=M" + i.ToString() + ".MemberID and M" + i.ToString() + ".BranchId=gs.OrganId ");
                " select dd.MemberID, dd.BranchId, DAY(dd.CreatedDate) lday from AccountRecords dd where Month(dd.CreatedDate)=" + i.ToString() + " and year(dd.CreatedDate)=" + year + @" ) tt 
                 group by tt.MemberID, tt.BranchId) as M" + i.ToString() + " on kh.MemberID=M" + i.ToString() + ".MemberID and M" + i.ToString() + ".BranchId=gs.OrganId ");
            }
            sql.Append(" where gs.HostID=" + hostId + " and kh.HostID=" + hostId + " and dic.Identifier = 'MemberType'");
            if (branchId != default(int))
            {
                sql.Append(" and gs.OrganID=" + branchId + " and kh.JoinBranch=" + branchId);
            }
            if (!string.IsNullOrEmpty(memberType))
                sql.Append(" and kh.Type='" + memberType + "' ");
            sql.Append(" ) as tmp ");
            return DBHelper.GetDataSet(sql.ToString());
        }

        /// <summary>
        /// 客户考勤日报表
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public DataTable Customer_receptions(int hostId, int branchId, string year, string month, string memberType)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select ROW_NUMBER() OVER (ORDER BY OrganID) as Record,* from (
                         select gs.OrganID,gs.Name as dianmian,kh.Name,kh.CardNo,kh.MobileNumber,dic.Contents as [Type]");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(",M" + i.ToString() + ".LDay as D" + i.ToString() + "");
            }
            sql.Append(@" from Members kh inner join Organ gs on 1=1 ");  // kh.JoinBranch = gs.OrganID
            sql.Append(" left join Dictionary dic on kh.Type=dic.KeyValue ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(@" left join ");

                sql.Append(@" (select tt.MemberID, tt.BranchId, COUNT(tt.lday) LDay from (
                  select yy.MemberID,yy.BranchId, DAY(yy.CreatedDate) lday from Books yy where yy.State='20' and Day(yy.CreatedDate)=" + i.ToString() + " and Month(yy.CreatedDate)=" + month + " and year(yy.CreatedDate)=" + year +
               " union " +
               " select dd.MemberID, dd.BranchId, DAY(dd.CreatedDate) lday from AccountRecords dd where dd.IsVaild=1 and Day(dd.CreatedDate)=" + i.ToString() + " and Month(dd.CreatedDate)=" + month + " and year(dd.CreatedDate)=" + year + @" ) tt 
                 group by tt.MemberID, tt.BranchId) as M" + i.ToString());

                sql.Append(" on kh.MemberID=M" + i.ToString() + ".MemberID and M" + i.ToString() + ".BranchId=gs.OrganId ");
            }

            sql.Append(@" where gs.HostID=" + hostId + " and kh.HostID=" + hostId + " and dic.Identifier = 'MemberType'");

            if (branchId != default(int))
                sql.Append(" and gs.OrganID=" + branchId + " and kh.JoinBranch=" + branchId);
            if (!string.IsNullOrEmpty(memberType))
                sql.Append(" and kh.Type='" + memberType + "' ");
            sql.Append(" ) as tmp ");
            return DBHelper.GetDataSet(sql.ToString());
        }

        /// <summary>
        /// 消耗明细
        /// </summary>
        /// <param name="HostId"></param>
        /// <param name="BranchId"></param>
        /// <param name="Brand"></param>
        /// <param name="Category"></param>
        /// <param name="ProjectName"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataTable ServiceDay(int HostId, int BranchId, string Brand, string Category,
            string ProjectName, String Year, String Month)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select br.OrganID, br.Name as BranchName, prj.ProjectID, prj.Name as ProjectName ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(",M" + i.ToString() + ".Amount AS D" + i.ToString());
            }
            sql.Append(@" from Organ br inner join Projects prj on 1 = 1");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(" LEFT JOIN (select bp.ProjectID, b.BranchId, SUM(bp.Amount) AS Amount from Books b, BookProjects bp where b.BookID = bp.BookID AND b.State='20' ");
                sql.Append(" AND Month(b.CreatedDate) = " + Month);
                sql.Append(" AND year(b.CreatedDate)=" + Year);
                sql.Append(" AND day(b.CreatedDate)=" + i.ToString());
                sql.Append(" AND b.HostID =" + HostId);
                if (BranchId != default(int))
                    sql.Append(" AND b.BranchId = " + BranchId);
                sql.Append(" group by bp.ProjectID, b.BranchId ");
                sql.Append(") M" + i.ToString() + " ON M" + i.ToString() + ".ProjectID = prj.ProjectID AND br.OrganID=M" + i.ToString() + ".BranchId ");
            }
            sql.Append(@" where prj.HostID = " + HostId + " AND br.HostID = " + HostId);
            return DBHelper.GetDataSet(sql.ToString());
        }

        /// <summary>
        /// 销售明细
        /// </summary>
        /// <param name="HostId"></param>
        /// <param name="BranchId"></param>
        /// <param name="Brand"></param>
        /// <param name="Category"></param>
        /// <param name="ProjectName"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataTable SalesDay(int HostId, int BranchId, string Brand, string Category,
                  string ProjectName, String Year, String Month)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select br.OrganID, br.Name as BranchName, prj.ProjectID, prj.Name as ProjectName ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(",M" + i.ToString() + ".Amount as D" + i.ToString());
            }
            sql.Append(@" from Organ br inner join Projects prj on 1 = 1");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(@" left join (select mp.ProjectID, mp.BranchId, sum(mp.Amount) as Amount from MemberProjects mp ");
                sql.Append(" where mp.IsVaild=1 and Month(mp.CreateDate)=" + Month + " and year(mp.CreateDate)=" + Year + " and day(mp.CreateDate)=" + i.ToString());
                sql.Append(" AND mp.HostID =" + HostId);
                if (BranchId != default(int))
                    sql.Append(" AND mp.BranchId = " + BranchId);
                sql.Append(" group by mp.ProjectID, mp.BranchId ");
                sql.Append(") M" + i.ToString() + " on M" + i.ToString() + ".ProjectID = prj.ProjectID AND br.OrganID=M" + i.ToString() + ".BranchId ");
            }
            sql.Append(" where br.HostID=" + HostId + " and prj.HostID = " + HostId);
            //if (BranchId != default(int))
            //    sql.Append(" and br.OrganID=" + BranchId);
            if (!string.IsNullOrEmpty(ProjectName))
                sql.Append(" and prj.Name like '%" + ProjectName + "%' ");
            if (!string.IsNullOrEmpty(Brand))
                sql.Append(" and prj.Brand = '" + Brand + "' ");
            if (!string.IsNullOrEmpty(Category))
                sql.Append(" and prj.Category = '" + Category + "' ");

            return DBHelper.GetDataSet(sql.ToString());
        }

        /// <summary>
        /// 套餐卡销售明细
        /// </summary>
        /// <param name="HostId"></param>
        /// <param name="BranchId"></param>
        /// <param name="CardTyp"></param>
        /// <param name="CardTmpl"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        public DataTable CardSalesDay(int HostId, int BranchId, string CardTyp, string CardTmpl, String Year, String Month, string stat)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select br.OrganID, br.Name as BranchName, dc.Contents as CardTitle ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(",M" + i.ToString() + ".DPD as D" + i.ToString());
            }
            sql.Append(@" from Organ br inner join Dictionary dc on 1=1 ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(@" left join( select mc.BranchId, mc.Type ");
                if (stat == "2")
                    sql.Append(",Count(mc.MemberCardId) as DPD");
                else
                    sql.Append(",Sum(mc.Amount) as DPD");
                sql.Append(" from MemberCards mc");
                sql.Append(" where mc.HostID = " + HostId + " and mc.Status=1 and Month(mc.CreateDate)=" + Month + " and year(mc.CreateDate)=" + Year + " and day(mc.CreateDate)=" + i.ToString());
                if (BranchId != default(int))
                    sql.Append(" and mc.BranchId = " + BranchId);
                if (!string.IsNullOrEmpty(CardTyp))
                    sql.Append(" and mc.Type = '" + CardTyp + "' ");
                if (!string.IsNullOrEmpty(CardTmpl))
                    sql.Append(" and mc.TmplID = " + CardTmpl);

                sql.Append(" group by mc.BranchID, mc.Type ");
                sql.Append(") M" + i.ToString() + " on br.OrganID = M" + i.ToString() + ".BranchId and dc.KeyValue = M" + i.ToString() + ".Type ");
            }
            sql.Append(" where dc.Identifier = 'MemberCardType' and dc.IsVaild=1 and br.HostID=" + HostId); // + " and br.OrganID= M.BranchId ");

            if (BranchId != default(int))
                sql.Append(" and br.OrganID=" + BranchId);

            return DBHelper.GetDataSet(sql.ToString());
        }


        public DataTable CardTmplSalesDay(int HostId, int BranchId, string CardType, string CardTmpl, String Year, String Month, string stat)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select br.OrganID, br.Name as BranchName, dc.Title as CardTitle ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(",M" + i.ToString() + ".DPD as D" + i.ToString());
            }
            sql.Append(@" from Organ br inner join CardTmpl dc on 1=1 ");
            for (int i = 1; i <= 31; i++)
            {
                sql.Append(@" left join( select mc.BranchId, mc.TmplID ");
                if (stat == "2")
                    sql.Append(",Count(mc.MemberCardId) as DPD");
                else
                    sql.Append(",Sum(mc.Amount) as DPD");
                sql.Append(" from MemberCards mc");
                sql.Append(" where mc.HostID = " + HostId + " and Month(mc.CreateDate)=" + Month + " and year(mc.CreateDate)=" + Year + " and day(mc.CreateDate)=" + i.ToString());
                if (BranchId != default(int))
                    sql.Append(" and mc.BranchId = " + BranchId);

                sql.Append(" group by mc.BranchID, mc.TmplID ");
                sql.Append(") M" + i.ToString() + " on br.OrganID = M" + i.ToString() + ".BranchId and dc.TmplID = M" + i.ToString() + ".TmplID ");
            }
            sql.Append(" where dc.HostId = " + HostId + " AND dc.IsVaild=1 and br.HostID=" + HostId);

            if (BranchId != default(int))
                sql.Append(" and br.OrganID=" + BranchId);
            if (!string.IsNullOrEmpty(CardType))
                sql.Append(" and ct.CardType = '" + CardType + "' ");
            if (!string.IsNullOrEmpty(CardTmpl))
                sql.Append(" and ct.TmplID = " + CardTmpl);
            return DBHelper.GetDataSet(sql.ToString());
        }
    }


    public class PModel
    {
        public int ProjectId { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string ProjectName { get; set; }
        public decimal Amount { get; set; }
        public int SaleTimes { get; set; }
        public decimal SalesPercent { get; set; }
        public decimal UsedAmount { get; set; }
        public int UsedTimes { get; set; }
    }

}