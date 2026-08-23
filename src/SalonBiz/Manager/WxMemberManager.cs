using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;
using SalonCRM.Models;

namespace SalonCRM.Manager
{
    public class WxMemberManager
    {

        ApplicationDbContext context = new ApplicationDbContext();
        ILog logger = Common.Logging.LogManager.GetLogger("WxMemberManager");

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="openID"></param>
        /// <param name="nike"></param>
        /// <param name="lang"></param>
        /// <param name="sex"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="county"></param>
        /// <param name="imgurl"></param>
        /// <param name="subscribe_time"></param>
        public void Subscribe(int hostId, string openID, string nike, string lang, int sex, string city, string province,
            string county, string imgurl, DateTime subscribe_time)
        {
            logger.Info("Subscribe New,Nike: " + nike);
            var user = context.WxMembers.Where(t => t.OpenID == openID).FirstOrDefault();
            if (user != null)
            {
                user.HostID = hostId;
                user.NickName = nike;
                user.Language = lang;
                user.Sex = sex;
                user.City = city;
                user.Province = province;
                user.Country = county;
                user.HeadImgUrl = imgurl;
                user.SubscribeTime = subscribe_time;
                user.Subscribe = "1";
            }
            else
            {
                context.WxMembers.Add(new WxMember
                {
                    OpenID = openID,
                    HostID = hostId,
                    NickName = nike,
                    Language = lang,
                    Sex = sex,
                    City = city,
                    Province = province,
                    Country = county,
                    HeadImgUrl = imgurl,
                    SubscribeTime = subscribe_time,
                    Subscribe = "1",
                });
            }
            context.SaveChanges();
        }

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="p"></param>
        public void Unsubscribe(string openID)
        {
            var user = context.WxMembers.Where(t => t.OpenID == openID).FirstOrDefault();
            if (user != null)
            {
                user.UnsubscribeTime = DateTime.Now;
                user.Subscribe = "0";
            }
            context.SaveChanges();
        }

        ApplicationDbContext dbcontent = new ApplicationDbContext();

        public IList<MemberModel> GetMember(WxMemberQModel model)
        {
            var query = context.WxMembers.Where(t => t.HostID == model.HostID);

            if (!string.IsNullOrEmpty(model.Name))
                query = query.Where(t => t.NickName.Contains(model.Name));
            //if (!string.IsNullOrEmpty(model.Sales))
            //    query = query.Where(t => t.Sales == model.Sales);
            if (!string.IsNullOrEmpty(model.OpenID))
                query = query.Where(t => t.OpenID == model.OpenID);
            if (!string.IsNullOrEmpty(model.Binding))
                query = query.Where(t => t.Binding == model.Binding);
            //if (!string.IsNullOrEmpty(model.Approved))
            //{
            //    int s = Convert.ToInt32(model.Approved);
            //    query = query.Where(t => t.Approved == s);
            //}
            if (!string.IsNullOrEmpty(model.Subscribe))
            {
                query = query.Where(t => t.Subscribe == model.Subscribe);
            }

            var list = (from dd in query
                        select new MemberModel
                        {
                            MemberID = dd.MemberID,
                            OpenID = dd.OpenID,
                            NickName = dd.NickName,
                            Sex = dd.Sex,
                            City = dd.City,
                            Province = dd.Province,
                            Country = dd.Country,
                            //PhoneNumber = dd.PhoneNumber,
                            Subscribe = dd.Subscribe,
                            SubscribeValue = context.Dictionaries.Where(t => t.Identifier == "WeixinSubscribe" && t.KeyValue == dd.Subscribe).FirstOrDefault().Contents,
                            SubscribeTime = dd.SubscribeTime,
                            Binding = dd.Binding,
                            BindingValue = context.Dictionaries.Where(t => t.Identifier == "MemberBinding" && t.KeyValue == dd.Binding).FirstOrDefault().Contents,
                            //Approved = dd.Approved,
                            //Sales = dd.Sales,
                            Language = dd.Language,
                            HeadImgUrl = dd.HeadImgUrl,
                            LastMessageTime = dd.LastMessageTime
                        }).OrderByDescending(t => t.SubscribeTime).ToList();

            return list;
        }

        public void UpdateMember(int hostID, string openID, string subscribe, string nike, int sex, string city, string province, string country, string headimgurl,
             DateTime subscribeTime)
        {
            try
            {
                var user = context.WxMembers.Where(t => t.OpenID == openID).FirstOrDefault();
                if (user != null)
                {
                    user.HostID = hostID;
                    user.NickName = nike;
                    user.Sex = sex;
                    user.City = city;
                    user.Province = province;
                    user.Country = country;
                    user.HeadImgUrl = headimgurl;
                    user.Subscribe = subscribe;
                    if (subscribeTime != DateTime.MinValue)
                        user.SubscribeTime = subscribeTime;
                }
                else
                {
                    var user1 = new WxMember
                    {
                        OpenID = openID,
                        HostID = hostID,
                        NickName = nike,
                        Sex = sex,
                        City = city,
                        Province = province,
                        Country = country,
                        HeadImgUrl = headimgurl,
                        Subscribe = subscribe,
                        SubscribeTime = DateTime.Now,
                        Binding = "0",
                    };
                    if (subscribeTime != DateTime.MinValue)
                        user1.SubscribeTime = subscribeTime;

                    context.WxMembers.Add(user1);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "OpenID:" + openID, ex);
            }
        }

        public MemberViewModel getMemberModel(string openID, int hostID)
        {
            var entity = (from model in dbcontent.Members.Where(t => t.OpenID == openID)
                          select new MemberViewModel
                          {
                              // 会员信息
                              MemberID = model.MemberID,
                              Source = dbcontent.Dictionaries.Where(a => a.KeyValue == model.Source && a.Identifier == "MemberSource").FirstOrDefault().Contents,
                              JoinBranch = model.JoinBranch,
                              JoinBranchStr = dbcontent.Organs.Where(t => t.OrganID == model.JoinBranch).FirstOrDefault().Name,
                              JoinDate = model.JoinDate,
                              CardNo = model.CardNo,
                              Amt = model.Amt,
                              Level = model.Level,
                              Status = model.Status,
                              StatusValue = dbcontent.Dictionaries.Where(a => a.HostId == hostID && a.KeyValue == model.Status && a.Identifier == "MemberStatus").FirstOrDefault().Contents,
                              Type = model.Type,
                              BookTime = model.BookTime,

                              // 个人信息
                              Name = model.Name,
                              Sex = dbcontent.Dictionaries.Where(a => a.KeyValue == model.Sex && a.Identifier == "MemberGender").FirstOrDefault().Contents,
                              Birthday = model.Birthday,
                              Vocation = dbcontent.Dictionaries.Where(a => a.KeyValue == model.Vocation && a.Identifier == "MemberVocation").FirstOrDefault().Contents,
                              Position = model.Position,
                              MaritalStatus = model.MaritalStatus,
                              Conjugal = model.Conjugal,
                              WeddingDay = model.WeddingDay,
                              MobileNumber = model.MobileNumber,
                              Email = model.Email,
                              Address = model.Address,
                              TencentQQ = model.TencentQQ,
                              WebChat = model.WebChat,
                              Company = model.Company,
                              CompanyAddress = model.CompanyAddress,
                          }).FirstOrDefault();

            return entity;
        }

        public MemberModel getMemberByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}