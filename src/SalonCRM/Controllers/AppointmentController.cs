using System;
using System.Linq;
using System.Web.Mvc;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;
using SalonCRM.Manager;
using Common.Logging;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace SalonCRM.Controllers
{

    /// <summary>
    /// 预约操作
    /// </summary>
    [CustomAuthorize]
    public class AppointmentController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();
        ILog logger = LogManager.GetLogger("AppointmentController");

        //
        // GET: /Book/
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppCreate(long MemberID, string Name, string Phone,
            DateTime BookDate, string Projects, string ProjectName, string Salesman, string Wokers, string BookRooms,
            int HostID, int BranchId, string CreatedBy, string ClientId)
        {
            var bag = (HostContainerBag)this.RouteData.Values["tenant"];
            var entity = new Appointment();
            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID.Equals(MemberID)).FirstOrDefault();

                entity.HostID = HostID;
                entity.BranchId = BranchId;
                entity.MemberID = member.MemberID;
                entity.Name = Name;
                entity.Phone = Phone;
                entity.BookDate = BookDate;
                entity.Projects = Projects;
                entity.ClientId = ClientId;
                entity.Salesman = Salesman;
                entity.Wokers = Wokers;
                entity.BookRooms = BookRooms;
                entity.BookStatus = "0";
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = CreatedBy;

                if (member == null)
                {
                    var result = new
                    {
                        code = 2,
                        message = "无效的会员卡号"
                    };
                    return Json(result);
                }
                else
                {

                    dbcontent.Appointments.Add(entity);
                    dbcontent.SaveChanges();

                    // 微信客户提醒
                    if (!string.IsNullOrEmpty(member.OpenID))
                    {
                        var accessToken = AccessTokenContainer.TryGetAccessToken(bag.AppId, bag.Secret);
                        var testData = new
                        {
                            first = new TemplateDataItem("您的预约成功"),
                            keyword1 = new TemplateDataItem(ProjectName),
                            keyword2 = new TemplateDataItem(entity.CreatedDate.ToShortDateString()),
                            remark = new TemplateDataItem("预约时间:" + BookDate + @"\n\n点击查看预约详情。")
                        };
                        string url = "http://cn.mdss.hk/wap/appointment/" + entity.AppointmentID;
                        var result1 = TemplateApi.SendTemplateMessage(accessToken, member.OpenID, bag.TmplMsg_Appointment, "#FF0000", url, testData);
                    }

                    var result = new
                    {
                        code = 1,
                        message = string.Empty
                    };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    code = 3,
                    message = "其他错误:" + ex.Message
                };
                logger.Error("预约失败", ex);
                return Json(entity);
            }
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppModify(Appointment model)
        {
            try
            {
                var member = dbcontent.Members.Where(_ => _.MemberID.Equals(model.MemberID)).FirstOrDefault();
                if (member == null)
                {
                    var result = new
                    {
                        code = 2,
                        message = "无效的会员卡号"
                    };
                    return Json(result);
                }
                else
                {
                    var entity = dbcontent.Appointments.Where(_ => _.AppointmentID == model.AppointmentID).FirstOrDefault();
                    if (null != entity)
                    {
                        //if (model.BookStatus.Equals("3") && !entity.BookStatus.Equals("3"))
                        //{
                        // 改约操作， 添加新的预约
                        //    entity.BookStatus = model.BookStatus;
                        //    var newEntity = new Appointment();
                        //    newEntity.HostID = 1;
                        //    newEntity.MemberID = member.MemberID;
                        //    newEntity.Name = model.Name;
                        //    newEntity.Phone = model.Phone;
                        //    newEntity.BookDate = model.BookDate;
                        //    newEntity.Projects = model.Projects;
                        //    newEntity.Salesman = model.Salesman;
                        //    newEntity.Wokers = model.Wokers;
                        //    newEntity.BookRooms = model.BookRooms;
                        //    newEntity.BookStatus = "3";
                        //    newEntity.CreatedDate = DateTime.Now;
                        //    newEntity.CreatedBy = model.CreatedBy;
                        //    dbcontent.Appointments.Add(newEntity);
                        //}
                        //else
                        //{
                        //entity.MemberID = member.MemberID;
                        entity.Name = model.Name;
                        entity.Phone = model.Phone;
                        entity.BookDate = model.BookDate;
                        entity.Projects = model.Projects;
                        entity.Salesman = model.Salesman;
                        entity.Wokers = model.Wokers;
                        entity.BookRooms = model.BookRooms;
                        //entity.BookStatus = model.BookStatus;
                        //}

                        dbcontent.SaveChanges();

                        var result = new
                        {
                            code = 1,
                            message = string.Empty
                        };
                        return Json(result);
                    }
                    else
                    {
                        var result = new
                        {
                            code = 3,
                            message = "数据不存在或已删除"
                        };
                        return Json(result);
                    }
                }
            }
            catch (Exception ex)
            {
                var result = new
                {
                    code = 4,
                    message = "其他错误:" + ex.Message
                };
                logger.Error("修改预约失败", ex);
                return Json(model);
            }
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppList(int hostId, int branchId, string cardNo, int index)
        {
            try
            {
                int pageCount = 2;
                var list = from _ in dbcontent.Appointments
                           join __ in dbcontent.Projects on _.Projects equals __.ProjectID.ToString() into ___
                           from __ in ___.DefaultIfEmpty()
                           where _.HostID == hostId && _.BranchId == branchId &&
                           (string.IsNullOrEmpty(cardNo) ? true : _.Member.CardNo.Contains(cardNo))
                           select new
                           {
                               Id = _.AppointmentID,
                               Name = _.Name,
                               Phone = _.Phone,
                               BookDate = _.BookDate,
                               Projects = _.Projects,
                               Wokers = _.Wokers,
                               BookRooms = _.BookRooms,
                               BookStatus = _.BookStatus,
                               MemberId = _.Member.MemberID,
                               CardNo = _.Member.CardNo,
                               ProjectName = __.Name
                           };
                var count = list.Count();
                count = (count % pageCount) != 0 ?
                    (int)(count / pageCount) + 1 :
                    (int)(count / pageCount);
                list = list.OrderBy(_ => _.BookDate);
                list = list.Skip(index * pageCount).Take(pageCount);

                var result = new
                {
                    count = count,
                    list = list.ToList()
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("预约列表取得失败", ex);
                //throw ex;
            }

            var result1 = new
            {
                count = 0
            };
            return Json(result1);
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult AppDayList(int hostId, int branchId, string cardNo, int dayindex, string status, string q, string userId)
        {
            try
            {
                var entity = dbcontent.Users.Where(t => t.Id == userId).FirstOrDefault();

                DateTime d = DateTime.Today.AddDays(dayindex);
                var query = dbcontent.Members.Where(a => a.HostID == hostId);
                if (entity != null && (entity.Type == "3" || entity.Type == "1"))    //美容师 或 顾问 
                {
                    query = query.Where(t => t.SalesmanId == userId);
                }
                var app = dbcontent.Appointments.AsQueryable();
                if (!String.IsNullOrEmpty(status))
                    app = app.Where(a => a.BookStatus.Equals(status));
                if (!String.IsNullOrEmpty(q))
                    query = query.Where(a => a.Name.Contains(q) || a.MobileNumber.Contains(q) || a.CardNo.Contains(q));
                var list = from _ in app
                           join __ in dbcontent.Projects on _.Projects equals __.ProjectID.ToString() into ___
                           join ____ in dbcontent.Dictionaries on _.BookStatus equals ____.KeyValue
                           from __ in ___.DefaultIfEmpty()
                           join m in query on _.MemberID equals m.MemberID
                           where _.HostID == hostId && _.BranchId == branchId
                                 && _.BookDate.Year == d.Year && _.BookDate.Month == d.Month && _.BookDate.Day == d.Day
                                 && ____.Identifier == "AppointmentStaus"
                           select new
                           {
                               Id = _.AppointmentID,
                               Name = _.Name,
                               Phone = _.Phone,
                               BookDate = _.BookDate,
                               Projects = _.Projects,
                               Wokers = _.Wokers,
                               BookRooms = _.BookRooms,
                               BookStatus = ____.Contents,
                               MemberId = _.Member.MemberID,
                               CardNo = _.Member.CardNo,
                               ProjectName = __.Name,
                               CreatedDate = _.CreatedDate
                           };
                var count = list.Count();
                list = list.OrderBy(_ => _.BookDate);

                var result = new
                {
                    count = count,
                    list = list.ToList()
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Error("预约列表取得失败", ex);
                //throw ex;
            }

            var result1 = new
            {
                count = 0
            };
            return Json(result1);
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult GetAppDetailById(long id)
        {
            try
            {
                Appointment model = new Appointment();
                Appointment entity = dbcontent.Appointments.Where(t => t.AppointmentID == id).FirstOrDefault();
                if (entity != null)
                {
                    var projects = entity.Projects.Split(',');
                    string pName = "";
                    foreach (var pid in projects)
                    {
                        if (!string.IsNullOrEmpty(pid))
                        {
                            var did = Convert.ToInt32(pid);
                            var p = dbcontent.Projects.Where(t => t.ProjectID == did).FirstOrDefault();
                            pName += (p != null ? p.Name + "," : "");
                        }
                    }
                    model.AppointmentID = entity.AppointmentID;
                    model.HostID = entity.HostID;
                    model.BranchId = entity.BranchId;
                    model.BookStatus = dbcontent.Dictionaries.Where(t => t.Identifier == "AppointmentStaus" && t.KeyValue == entity.BookStatus).FirstOrDefault().Contents;
                    model.Projects = entity.Projects;
                    model.ProjectNames = pName;
                    model.Name = entity.Name;
                    model.CardNo = entity.Member.CardNo;
                    model.Phone = entity.Phone;
                    model.Wokers = entity.Wokers;
                    model.BookRooms = entity.BookRooms;
                    model.BookDate = entity.BookDate;
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                logger.Error("预约列表取得失败", ex);
                //throw ex;
            }

            var result1 = new
            {
                count = 0
            };
            return Json(result1);
        }

        [AllowAnonymous]
        [AllowCrossDomain]
        public ActionResult GetMemberList()
        {
            try
            {
                var list = (from _ in dbcontent.Members
                            select new
                            {
                                code = _.MemberID,
                                name = _.CardNo
                            }).ToList();
                return Json(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}