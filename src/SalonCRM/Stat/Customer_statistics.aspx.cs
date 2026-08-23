using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SalonCRM.Web;
using DevExpress.Web;
using SalonCRM.Manager;
using SalonCRM.Identity;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 客户统计
    /// </summary>
    public partial class Customer_statistics : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;
            //int pl = 0; // 0:客户统计 1客户类别统计 2 客户级别统计

            if (!this.IsPostBack)
            {
                if (Request.QueryString["pl"] != null)
                {

                }

                cbType.DataSource = CommonManager.GetDictionaries("MemberType");
                cbType.TextField = "Contents";
                cbType.ValueField = "KeyValue";
                cbType.DataBind();
                cbType.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbLevel.DataSource = CommonManager.GetDictionaries(hostId, "MemberLevel");
                cbLevel.TextField = "Contents";
                cbLevel.ValueField = "KeyValue";
                cbLevel.DataBind();
                cbLevel.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbSex.DataSource = CommonManager.GetDictionaries("MemberGender");
                cbSex.TextField = "Contents";
                cbSex.ValueField = "KeyValue";
                cbSex.DataBind();
                cbSex.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));


                if (cu.Type != "2")
                {
                    cbBranch.Text  = GlobalContext.Current.UserDepartment.Name;
                    cbBranch.ReadOnly = true;
                }
            }

            int branch = default(int);
            string memberType = Convert.ToString(cbType.Value);
            string memberLevel = Convert.ToString(cbLevel.Value);
            string memberGender = Convert.ToString(cbSex.Value);
            int inCount = default(int);
            if (cbBranch.Value != null)
                branch = Convert.ToInt32(cbBranch.Value);
            if (tbCount.Value != null)
                inCount = Convert.ToInt32(tbCount.Value);

            grid.DataSource = StatManager.GetCustomerSAList(hostId, branch, memberType, memberLevel, memberGender,
                Convert.ToString(tbName.Value), Convert.ToString(tbCard.Value), inCount, Convert.ToDateTime(deStart.Value),
                Convert.ToDateTime(deEnd.Value), Convert.ToDecimal(tbAmtStart.Value), Convert.ToDecimal(tbAmtEnd.Value),
                Convert.ToDecimal(tbUseStart.Value), Convert.ToDecimal(tbUseEnd.Value));
            grid.DataBind();

        }

        protected void btnStat_Click(object sender, EventArgs e)
        {
        }

        protected void grid_HeaderFilterFillItems(object sender, DevExpress.Web.ASPxGridViewHeaderFilterEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (e.Column.FieldName == "RechargeAmount")
            {
                e.Values.Clear();
                if (e.Column.SettingsHeaderFilter.Mode == GridHeaderFilterMode.List)
                    e.AddShowAll();
                e.AddValue("0-4999", string.Empty, "[RechargeAmount] >= 0 and [RechargeAmount] < 5000");
                e.AddValue("5000-2万", string.Empty, "[RechargeAmount] >= 5000 and [RechargeAmount] < 20000");
                e.AddValue("2万-10万", string.Empty, "[RechargeAmount] >= 20000 and [RechargeAmount] < 10000");
                e.AddValue("10万以上", string.Empty, "[RechargeAmount] > 100000");
            }
            else if (e.Column.FieldName == "ExpenseAmount")
            {
                e.Values.Clear();
                if (e.Column.SettingsHeaderFilter.Mode == GridHeaderFilterMode.List)
                    e.AddShowAll();
                e.AddValue("0-4999", string.Empty, "[ExpenseAmount] >= 0 and [ExpenseAmount] < 5000");
                e.AddValue("5000-2万", string.Empty, "[ExpenseAmount] >= 5000 and [ExpenseAmount] < 20000");
                e.AddValue("2万-10万", string.Empty, "[ExpenseAmount] >= 20000 and [ExpenseAmount] < 10000");
                e.AddValue("10万以上", string.Empty, "[ExpenseAmount] > 100000");
            }
            else if (e.Column.FieldName == "JoinDate")
            {
                e.Values.Clear();
                if (e.Column.SettingsHeaderFilter.Mode == GridHeaderFilterMode.List)
                    e.AddShowAll();
                e.AddValue("最近三个月", string.Empty, string.Format("[JoinDate] >= '{0}'", DateTime.Now.Date.AddMonths(-3).ToShortDateString()));
                e.AddValue("最近一个月", string.Empty, string.Format("[JoinDate] >= '{0}'", DateTime.Now.Date.AddMonths(-1).ToShortDateString()));
            }
        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "客户统计";
            if (deStart.Text != "")
                fileName += "-" + deStart.Value;
            else if (deEnd.Text != "")
                fileName += "-" + deEnd.Value;
            else
                fileName += "-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.ReportHeader = "客户统计";
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }


        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.RowType != GridViewRowType.Data) return;
                string value = e.GetValue("Status").ToString();

                if (value == "有效会员" || value == "有效客户")
                    e.Row.BackColor = System.Drawing.Color.PaleGreen;
                else if (value == "准有效会员" || value == "准有效客户")
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                else if (value == "休眠会员" || value == "沉睡会员" || value == "休眠客户" || value == "沉睡客户")
                    e.Row.BackColor = System.Drawing.Color.Moccasin;
                else if (value == "流失会员" || value == "流失客户")
                    e.Row.BackColor = System.Drawing.Color.DarkSalmon;
            }
            catch { }
        }
    }
}