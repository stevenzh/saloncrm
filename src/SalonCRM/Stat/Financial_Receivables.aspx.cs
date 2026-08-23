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
using SalonCRM.Models;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 应收款一览表
    /// </summary>
    public partial class Financial_Receivables : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            if (!this.IsPostBack)
            {
                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                deDate.Value = DateTime.Today;

                // 销售初始化
                cbSalesman.DataSource = CommonManager.GetUsers(GlobalContext.Current.UserDepartment.OrganID);
                cbSalesman.TextField = "UserCnName";
                cbSalesman.ValueField = "Id";
                cbSalesman.DataBind();
            }

            if (cbBranch.Text != "" && deStart.Value != null && deDate.Value != null)
            {
                ReceivablesQModel model = new ReceivablesQModel { BranchId = Convert.ToInt32(cbBranch.Value), StartDate = Convert.ToDateTime(deStart.Text), EndDate = Convert.ToDateTime(deDate.Text), Salesman = (string)cbSalesman.Value };
                grid.DataSource = StatManager.GetReceivables(model);
                grid.DataBind();
                grid.ExpandAll();
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
        }
        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "应收款一览表";
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }

        protected void cbSalesman_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxComboBox box = (ASPxComboBox)sender;
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                box.DataSource = CommonManager.GetUsers(Convert.ToInt32(e.Parameter));
                box.TextField = "UserCnName";
                box.ValueField = "Id";
                box.DataBind();
            }
        }


    }
}