using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using SalonCRM.Web;
using SalonCRM.Manager;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 日报表详细-B版
    /// </summary>
    public partial class OneDayDetail : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            if (!IsPostBack)
            {
                cbBranch.DataSource = DBHelper.GetDataSet("select OrganID as code, Name from Organ where HostID = " + hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "code";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("", ""));
            }

            if (Request.QueryString["branchId"] != null)
                cbBranch.Value = Request.QueryString["branchId"];
            if (Request.QueryString["theDay"] != null)
                deStart.Value = Convert.ToDateTime(Request.QueryString["theDay"]);

            if (cbBranch.Value != null || deStart.Value != null)
            {
                grid.DataSource = StatManager.GetOneDailyDetail(Convert.ToInt32(cbBranch.Value), Convert.ToDateTime(deStart.Value));
                grid.DataBind(); //执行绑定
            }
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {

        }

        protected void grid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (e.Column.FieldName == "JoinDate")
            {
                e.Values.Clear();
                if (e.Column.Settings.HeaderFilterMode == HeaderFilterMode.List)
                    e.AddShowAll();
                e.AddValue("最近三个月", string.Empty, string.Format("[JoinDate] >= '{0}'", DateTime.Now.Date.AddMonths(-3).ToShortDateString()));
                e.AddValue("最近一个月", string.Empty, string.Format("[JoinDate] >= '{0}'", DateTime.Now.Date.AddMonths(-1).ToShortDateString()));
            }
        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "日营业明细";
            if (((string)cbBranch.Value) != "")
                fileName += "-" + cbBranch.Text;
            if (deStart.Value != null)
                fileName += deStart.Date.ToString("yyyyMMdd");
            gridExport.ReportHeader = fileName;
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}