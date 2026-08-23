namespace SalonCRM
{
    public class Consts
    {
        #region cache key 缓存关键字

        public static readonly string HostCode = "Host."; //产品字典

        #endregion

        #region cachetime

        /// <summary>
        /// 页面缓存时间2.5分钟
        /// </summary>
        public const int OutputCacheDuration1 = 150;

        /// <summary>
        /// 页面缓存时间10分钟
        /// </summary>
        public const int OutputCacheDuration2 = 600;

        /// <summary>
        /// 页面缓存时间1小时 
        /// </summary>
        public const int OutputCacheDuration3 = 3600;

        /// <summary>
        /// 页面缓存时间8小时 
        /// </summary>
        public const int OutputCacheDuration4 = 30000;

        /// <summary>
        /// 页面缓存时间24小时 
        /// </summary>
        public const int OutputCacheDurationDay = 30000 * 4;

        #endregion

        #region Test 
        public static string GetTestHost(string host)
        {
            var result = host;
            if (host.Equals("localhost")) result = "cn.mdss.hk";
            //if (host.Equals("localhost")) result = "gjyk.mdss.hk";
            //if (host.Equals("localhost")) result = "zhimei.mdss.hk";
            //if (host.Equals("localhost")) result = "mrhs.mdss.hk";
            //if (host.Equals("localhost")) result = "jnbls.mdss.hk";
            //if (host.Equals("localhost")) result = "hfm.mdss.hk";
            //if (host.Equals("localhost")) result = "xsx.mdss.hk";
            //if (host.Equals("localhost")) result = "kbl.mdss.hk";
            //if (host.Equals("localhost")) result = "msh.mdss.hk";

            return result;
        }
        #endregion
    }
}
