using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalonCRM.Web;
using SalonCRM.Manager;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 客户考勤日报表
    /// </summary>
    public partial class Customer_attendance_daily : AdminPage
    {
        /// <summary>
        /// 耗卡
        /// </summary>
        protected string Kaoqing { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            if (!this.IsPostBack)
            {
                //cbYear.Items.Add(new DevExpress.Web.ListEditItem("", ""));
                for (int i = 2017; i <= DateTime.Now.Year; i++)
                {
                    cbYear.Items.Add(new DevExpress.Web.ListEditItem(i.ToString(), i.ToString()));
                }

                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbType.DataSource = CommonManager.GetDictionaries("MemberType");
                cbType.ValueField = "KeyValue";
                cbType.TextField = "Contents";
                cbType.DataBind();
                cbType.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBranch.Text = GlobalContext.Current.UserDepartment.Name; //默认
                cbYear.Text = DateTime.Today.Year.ToString();
                cbMonth.Value = DateTime.Today.Month.ToString();
            }

            string year = DateTime.Now.Year.ToString();
            if (Request.QueryString["year"] != null)
            {
                year = Request.QueryString["year"].ToString();
                cbYear.Text = year;
            }

            string month = DateTime.Now.Month.ToString();
            if (cbMonth.Value != null)
                month = Convert.ToString(cbMonth.Value);
            else if (Request.QueryString["month"] != null)
            {
                month = Request.QueryString["month"].ToString();
                cbMonth.Value = month;
            }

            DataTable dt = new StatManager().Customer_receptions(hostId, Convert.ToInt32(cbBranch.Value), year, month, Convert.ToString(cbType.Value));
            grid.DataSource = dt;//指定Grid的数据
            grid.DataBind(); //执行绑定

            Kaoqing = "[";
            for (int i = 1; i < 32; i++)
            {
                Kaoqing += dt.AsEnumerable().Sum(s => s.IsNull("D" + i) ? 0 : s.Field<int>("D" + i)).ToString() + ",";
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

        }

        protected void grid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName.StartsWith("D"))
            {
                string day = e.DataColumn.FieldName.Substring(1, e.DataColumn.FieldName.Length - 1);
                try
                {
                    DateTime tod = new DateTime(Convert.ToInt32(cbYear.Text), Convert.ToInt32(cbMonth.Value), Convert.ToInt32(day));
                    if (tod.DayOfWeek == DayOfWeek.Saturday || tod.DayOfWeek == DayOfWeek.Sunday)
                        e.Cell.BackColor = System.Drawing.Color.FromArgb(238, 199, 216);
                }
                catch (Exception) { }
            }
        }
    }
}