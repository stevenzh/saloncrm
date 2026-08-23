using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using SalonCRM.Web;
using SalonCRM.Manager;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 客户考勤月报表
    /// </summary>
    public partial class Customer_attendance : AdminPage
    {
        /// <summary>
        /// 耗卡
        /// </summary>
        protected string Kaoqing { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            if (!IsPostBack)
            {
                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                for (int i = 2015; i <= DateTime.Now.Year; i++)
                {
                    cbYear.Items.Add(new DevExpress.Web.ListEditItem(i.ToString(), i.ToString()));
                }

                cbType.DataSource = CommonManager.GetDictionaries("MemberType");
                cbType.ValueField = "KeyValue";
                cbType.TextField = "Contents";
                cbType.DataBind();
                cbType.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBranch.Text = GlobalContext.Current.UserDepartment.Name; //默认
                cbYear.Text = DateTime.Today.Year.ToString();
            }

            var dt = StatManager.Customer_receptions(hostId, Convert.ToInt32(cbBranch.Value), cbYear.Text, Convert.ToString(cbType.Value));
            grid.DataSource = dt;//指定Grid的数据
            grid.DataBind(); //执行绑定

            Kaoqing = "[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Jan") ? 0 : s.Field<int>("Jan")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Feb") ? 0 : s.Field<int>("Feb")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Mar") ? 0 : s.Field<int>("Mar")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Apr") ? 0 : s.Field<int>("Apr")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("May") ? 0 : s.Field<int>("May")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Jun") ? 0 : s.Field<int>("Jun")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Jul") ? 0 : s.Field<int>("Jul")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Aug") ? 0 : s.Field<int>("Aug")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Sep") ? 0 : s.Field<int>("Sep")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Oct") ? 0 : s.Field<int>("Oct")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Nov") ? 0 : s.Field<int>("Nov")).ToString() + ",";
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("Dec") ? 0 : s.Field<int>("Dec")).ToString() + ",0";
            }

            Kaoqing = Kaoqing.Substring(0, Kaoqing.Length - 1) + "]";
        }

        private decimal NullVal(object value)
        {
            decimal val = 0;
            if (!Convert.IsDBNull(value))
                val = Convert.ToDecimal(value);
            return val;
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {
        }
        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "考勤月报表" + "-" + cbYear.Text;
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }

    }
}