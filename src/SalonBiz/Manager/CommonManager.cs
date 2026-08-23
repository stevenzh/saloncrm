using System;
using System.Collections.Generic;
using System.Linq;
using SalonCRM.Models;

namespace SalonCRM.Manager
{
    public class CommonManager
    {
        static ApplicationDbContext dbcontent = new ApplicationDbContext();

        /// <summary>
        /// 省份列表
        /// </summary>
        /// <returns></returns>
        public static IList<Region> GetProvinces()
        {
            return dbcontent.Regions.Where(t => t.Type == 1).ToList();
        }
        /// <summary>
        /// 城市列表
        /// </summary>
        /// <param name="province">省份</param>
        /// <returns></returns>
        public static IList<Region> GetCitys(string province)
        {
            return dbcontent.Regions.Where(t => t.Type == 2 && t.Code.Substring(0, 2) == province.Substring(0, 2)).ToList();
        }
        /// <summary>
        /// 地区列表
        /// </summary>
        /// <param name="city">城市</param>
        /// <returns></returns>
        public static IList<Region> GetRegions(string city)
        {
            return dbcontent.Regions.Where(t => t.Type == 3 && t.Code.Substring(0, 4) == city.Substring(0, 4)).ToList();
        }
        /// <summary>
        /// 取得所有门店
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns></returns>
        public static IList<Organ> GetBranchs(int hostId)
        {
            return dbcontent.Organs.Where(t => t.HostID == hostId).ToList();
        }
        /// <summary>
        /// 取得所有品牌
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns></returns>
        public static IList<Dictionary> GetBrands(int hostId)
        {
            return dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "ProjectBrand" && t.IsVaild == 1).ToList();
        }
        /// <summary>
        /// 取得所有品牌
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns></returns>
        public static IList<CardTmpl> GetCardTmpls(int hostId, string type)
        {
            var query = dbcontent.CardTmpls.Where(t => t.HostID == hostId && t.IsVaild == 1);
            if (!string.IsNullOrEmpty(type))
                query = query.Where(t => t.CardType == type);
            return query.ToList();
        }
        /// <summary>
        /// 取得用户
        /// </summary>
        /// <param name="branchId">门店</param>
        /// <returns></returns>
        public static IList<ApplicationUser> GetUsers(int branchId)
        {
            return dbcontent.Users.Where(t => t.OrganId == branchId).ToList();
        }
        /// <summary>
        /// 取得词典列表
        /// </summary>
        /// <param name="hostId"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static IList<Dictionary> GetDictionaries(int hostId, string identifier)
        {
            return dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == identifier && t.IsVaild == 1).ToList();
        }
        public static IList<Dictionary> GetDictionaries(string identifier)
        {
            return dbcontent.Dictionaries.Where(t => t.Identifier == identifier && t.IsVaild == 1).ToList();
        }

        /// <summary>
        /// 最近一个月生日的会员
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public static IList<Member> GetBirth(int branchId)
        {
            DateTime d = DateTime.Now.AddMonths(1);
            return dbcontent.Members.Where(t => t.JoinBranch == branchId && t.LastBirth < d).ToList();
        }

        public static List<CommonSample> GetMemberType(int branchId)
        {
            var l = (from dd in dbcontent.Members.Where(t => t.JoinBranch == branchId)
                     group dd by dd.Type into g
                     select new CommonSample
                     {
                         Type = g.Key,
                         Num = g.Count(),
                     }).ToList();
            return l;
        }

        public static List<CommonSample> GetMemberStatus(int hostId, int branchId)
        {
            var l = (from dd in dbcontent.Members.Where(t => t.JoinBranch == branchId)
                     group dd by dd.Status into g
                     select new CommonSample
                     {
                         Type = g.Key,
                         Categoty = dbcontent.Dictionaries.Where(t => t.HostId == hostId && t.Identifier == "MemberStatus" && t.KeyValue == g.Key).FirstOrDefault().Contents,
                         Num = g.Count(),
                     }).ToList();
            return l;
        }
    }

    public class CommonSample
    {
        public string Categoty { get; set; }
        public string Type { get; set; }
        public long Num { get; set; }
    }
}