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
using SalonCRM.Identity;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 日报表
    /// </summary>
    public partial class DailySTA : AdminPage
    {
        /// <summary>
        /// 
        /// </summary>
        protected string DayStr { get; set; }
        /// <summary>
        /// 耗卡
        /// </summary>
        protected string Haokai { get; set; }
        /// <summary>
        /// 销售
        /// </summary>
        protected string Xiaoshou { get; set; }
        /// <summary>
        /// 收款
        /// </summary>
        protected string Shoukuan { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;

            if (!IsPostBack)
            {
                cbBranch.DataSource = DBHelper.GetDataSet("select OrganID as code, Name from Organ where HostID = " + hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "code";
                cbBranch.DataBind();
                //cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("", ""));
                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                if (cu.Type != "2") cbBranch.ReadOnly = true;

                deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                deEnd.Value = DateTime.Today;
            }

            if (cbBranch.Value != null || (deStart.Value != null && deStart.Value == deEnd.Value))
            {
                var list = StatManager.GetDailyList(Convert.ToInt32(cbBranch.Value), Convert.ToDateTime(deStart.Value), Convert.ToDateTime(deEnd.Value));
                grid.DataSource = list;
                grid.DataBind(); //执行绑定

                DayStr = "[\"" + string.Join("\",\"", list.Select(t => t.TheDay.ToShortDateString()).ToArray()) + "\"]";
                Haokai = "[" + string.Join(",", list.Select(t => NullVal(t.A1) + NullVal(t.A2) + NullVal(t.A3)).ToArray()) + "]";
                Xiaoshou = "[" + string.Join(",", list.Select(t => NullVal(t.S1) + NullVal(t.S2) + NullVal(t.S3) + NullVal(t.S4)).ToArray()) + "]";
                Shoukuan = "[" + string.Join(",", list.Select(t => NullVal(t.T1) + NullVal(t.T2) + NullVal(t.T3) + NullVal(t.T4)).ToArray()) + "]";
            }
        }

        private decimal NullVal(decimal? value)
        {
            return (value.HasValue ? value.Value : 0);
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {

        }

        protected void grid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (e.Column.FieldName == "RechargeAmount")
            {
                e.Values.Clear();
                if (e.Column.Settings.HeaderFilterMode == HeaderFilterMode.List)
                    e.AddShowAll();
                e.AddValue("0-4999", string.Empty, "[RechargeAmount] >= 0 and [RechargeAmount] < 5000");
                e.AddValue("5000-2万", string.Empty, "[RechargeAmount] >= 5000 and [RechargeAmount] < 20000");
                e.AddValue("2万-10万", string.Empty, "[RechargeAmount] >= 20000 and [RechargeAmount] < 10000");
                e.AddValue("10万以上", string.Empty, "[RechargeAmount] > 100000");
            }
            else if (e.Column.FieldName == "ExpenseAmount")
            {
                e.Values.Clear();
                if (e.Column.Settings.HeaderFilterMode == HeaderFilterMode.List)
                    e.AddShowAll();
                e.AddValue("0-4999", string.Empty, "[ExpenseAmount] >= 0 and [ExpenseAmount] < 5000");
                e.AddValue("5000-2万", string.Empty, "[ExpenseAmount] >= 5000 and [ExpenseAmount] < 20000");
                e.AddValue("2万-10万", string.Empty, "[ExpenseAmount] >= 20000 and [ExpenseAmount] < 10000");
                e.AddValue("10万以上", string.Empty, "[ExpenseAmount] > 100000");
            }
            else if (e.Column.FieldName == "JoinDate")
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
            string fileName = "日报表";
            if (((string)cbBranch.Value) != "")
                fileName += "-" + cbBranch.Text;
            if (deStart.Value != null)
                fileName += deStart.Date.ToString("yyyyMMdd");
            if (deEnd.Value != null)
                fileName += deEnd.Date.ToString("-yyyyMMdd");
            gridExport.ReportHeader = fileName;
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}