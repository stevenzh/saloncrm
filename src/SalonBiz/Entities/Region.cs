namespace SalonCRM.Models
{
    /// <summary>
    /// 行政区域
    /// </summary>
    public partial class Region
    {
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 1 省份 2 城市 3 区县
        /// </summary>
        public int Type { get; set; }
    }
}