using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Linq;
using Common.Logging;
using Quartz;
using Newtonsoft.Json;
using Senparc.Weixin.MP.CommonAPIs;
using SalonCRM.Models;
using SalonCRM.Identity;
using SalonCRM.Manager;
using SalonCRM.Cache;

namespace SalonCRM
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        ILog logger = LogManager.GetLogger("MvcApplication");

        protected void Application_Start()
        {
            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState = System.Web.SessionState.SessionStateBehavior.Disabled;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Entity Framework 初始化
            Database.SetInitializer<ApplicationDbContext>(new DataContextInitilizer());
            // DevExpress 初始化
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();


            ISchedulerFactory sf = new Quartz.Impl.StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();
            sched.Start();
            //JobKey jobkey = new JobKey("myjob", "mygroup");
            //IJobDetail job = JobBuilder.Create<MyJob>().WithIdentity(jobkey).Build();
            ////ITrigger trigger = TriggerBuilder.Create().StartNow().Build(); 
            ////比较复杂的应用 
            //IOperableTrigger trigger = new CronTriggerImpl("trigName", "group1", "0/2 * * * * ?");
            ////简单方式 
            ////SimpleTriggerImpl trigger = new SimpleTriggerImpl("simpleTrig", "simpleGroup", 10, DateTime.Now.AddSeconds(5) - DateTime.Now);
            //sched.ScheduleJob(job, trigger);  

            RegisterCache();

            //RegisterWeixinCache();//注册分布式缓存
            //RegisterWeixinThreads();//激活微信缓存（必须）
            //RegisterSenparcWeixin();//注册Demo所用微信公众号的账号信息
            //RegisterWeixinPay();//注册微信支付
            //RegisterWeixinThirdParty(); //注册微信第三方平台

            //Senparc.Weixin.Config.IsDebug = true;//这里设为Debug状态时，/App_Data/目录下会生成日志文件记录所有的API请求日志，正式发布版本建议关闭
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.UserId = serializeModel.UserId;
                newUser.UserCnName = serializeModel.UserCnName;
                newUser.roles = serializeModel.roles;
                newUser.Type = serializeModel.Type;
                newUser.IsAdminUser = serializeModel.IsAdminUser;
                HttpContext.Current.User = newUser;
            }
        }

        /// <summary>
        /// 自定义缓存策略
        /// </summary>
        //private void RegisterWeixinCache()
        //{
        //    //如果留空，默认为localhost（默认端口）

        //    var redisConfiguration = System.Configuration.ConfigurationManager.AppSettings["Cache_Redis_Configuration"];
        //    RedisManager.ConfigurationOption = redisConfiguration;

        //    //如果不执行下面的注册过程，则默认使用本地缓存
        //    if (redisConfiguration != "Redis配置")
        //    {
        //        CacheStrategyFactory.RegisterContainerCacheStrategy(() => RedisContainerCacheStrategy.Instance);//Redis
        //    }
        //}

        /// <summary>
        /// 激活微信缓存
        /// </summary>
        //private void RegisterWeixinThreads()
        //{
        //    ThreadUtility.Register();
        //}

        /// <summary>
        /// 注册Demo所用微信公众号的账号信息
        /// </summary>
        //private void RegisterSenparcWeixin()
        //{
        //    AccessTokenContainer.Register(
        //        System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"],
        //        System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"]);
        //}

        private void RegisterCache()
        {
            ApplicationDbContext dbcontent = new ApplicationDbContext();
            List<HostProfile> profiles = dbcontent.HostProfiles.OrderBy(t => t.HostID).ToList();
            List<int> dd = profiles.Select(t => t.HostID).Distinct().ToList();

            List<Host> hosts = dbcontent.Hosts.Where(t => t.IsVaild == 1).ToList();
            foreach (var host in hosts)
            {
                var result = profiles.Where(t => t.HostID == host.HostID).ToDictionary(t => t.PropertyText, t => t.PropertyValue);
                var bag = new HostContainerBag
                {
                    Key = Consts.HostCode + host.Url,
                    HostID = host.HostID,
                    AppId = result.ContainsKey("WeixinAppId") ? result["WeixinAppId"] : "",
                    Token = result.ContainsKey("Token") ? result["Token"] : "",
                    EncodingAESKey = result.ContainsKey("EncodingAESKey") ? result["EncodingAESKey"] : "",
                    Secret = result.ContainsKey("Secret") ? result["Secret"] : "",

                    Setting_MajorPercentage = result.ContainsKey("MajorPercentage") ? result["MajorPercentage"] : "",   // 顾问业绩占比
                    Setting_BeauticianPercentage = result.ContainsKey("BeauticianPercentage") ? result["BeauticianPercentage"] : "",  // 助理美容师业绩占比
                    Setting_MajorBeauticianPercentage = result.ContainsKey("MajorBeauticianPercentage") ? result["MajorBeauticianPercentage"] : "",  // 主要美容师占比
                    Setting_PercentageLock = result.ContainsKey("Setting_PercentageLock") ? result["Setting_PercentageLock"] : "",  // 设置占比锁定  1（锁定） - 0
                    Setting_LimitedCard_Num = result.ContainsKey("LimitedCardNum") ? result["LimitedCardNum"] : "80",  // 综合限时年卡分摊次数
                    Setting_PointGenerate = result.ContainsKey("PointGenerate") ? result["PointGenerate"] : "0.01",  // 积分生产比
                    Setting_MemberNewStart = result.ContainsKey("Setting_MemberNewStart") ? result["Setting_MemberNewStart"] : "1-1",  // 新会员起始月日

                    TenPayV3_Key = result.ContainsKey("TenPayV3_Key") ? result["TenPayV3_Key"] : "",
                    TenPayV3_MchId = result.ContainsKey("TenPayV3_MchId") ? result["TenPayV3_MchId"] : "",
                    TenPayV3_TenpayNotify = result.ContainsKey("TenPayV3_TenpayNotify") ? result["TenPayV3_TenpayNotify"] : "",

                    TmplMsg_Appointment = result.ContainsKey("TmplMsg_Appointment") ? result["TmplMsg_Appointment"] : "",   // 预约提醒
                    TmplMsg_Consumer = result.ContainsKey("TmplMsg_Consumer") ? result["TmplMsg_Consumer"] : "",   // 消费提醒（卡扣）
                    TmplMsg_Recharge = result.ContainsKey("TmplMsg_Recharge") ? result["TmplMsg_Recharge"] : "",   // 充值提醒
                    TmplMsg_Service = result.ContainsKey("TmplMsg_Service") ? result["TmplMsg_Service"] : "",   // 服务提醒
                    TmplMsg_ChangeCard = result.ContainsKey("TmplMsg_ChangeCard") ? result["TmplMsg_ChangeCard"] : "",   // 换卡提醒
                    TmplMsg_GetCard = result.ContainsKey("TmplMsg_GetCard") ? result["TmplMsg_GetCard"] : "",   // 购卡提醒
                };

                CacheContext.Current.Add(bag.Key, bag);

                // logger.Info("Cache pull" + bag.Key);
                if (!string.IsNullOrEmpty(bag.AppId))
                {
                    AccessTokenContainer.Register(bag.AppId, bag.Secret);
                }
            }
        }

    }
}
