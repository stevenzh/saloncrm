using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Manager
{
    [Serializable]
    public class HostContainerBag
    {
        public int HostID { get; set; }
        public string AppId { get; set; }

        public string Token { get; set; }

        public string EncodingAESKey { get; set; }
        public string Secret { get; set; }


        public string TenPayV3_MchId { get; set; }
        public string TenPayV3_Key { get; set; }
        public string TenPayV3_TenpayNotify { get; set; }

        /// <summary>
        /// 预约提醒
        /// </summary>
        public string TmplMsg_Appointment { get; set; }
        /// <summary>
        /// 消费提醒
        /// </summary>
        public string TmplMsg_Consumer { get; set; }
        /// <summary>
        /// 充值提醒
        /// </summary>
        public string TmplMsg_Recharge { get; set; }
        /// <summary>
        /// 服务提醒
        /// </summary>
        public string TmplMsg_Service { get; set; }
        /// <summary>
        /// 购卡提醒
        /// </summary>
        public string TmplMsg_GetCard { get; set; }
        /// <summary>
        /// 换卡提醒
        /// </summary>
        public string TmplMsg_ChangeCard { get; set; }

        /// <summary>
        /// 设置顾问业绩（例如0.6）
        /// </summary>
        public string Setting_MajorPercentage { get; set; }
        /// <summary>
        /// 设置美容师业绩占比（例如0.3）
        /// </summary>
        public string Setting_MajorBeauticianPercentage { get; set; }
        /// <summary>
        /// 设置占比锁定
        /// </summary>
        public string Setting_PercentageLock { get; set; }
        /// <summary>
        /// 设置助理美容师业绩占比（例如 0.1）
        /// </summary>
        public string Setting_BeauticianPercentage { get; set; }
        /// <summary>
        /// 综合限时年卡分摊次数
        /// </summary>
        public string Setting_LimitedCard_Num { get; set; }

        /// <summary>
        /// 设置充值送积分比率（例如：0.01）
        /// </summary>
        public string Setting_PointGenerate { get; set; }

        public string Key { get; set; }
        /// <summary>
        /// 新会员起始月日
        /// </summary>
        public string Setting_MemberNewStart { get; set; }
    }
}