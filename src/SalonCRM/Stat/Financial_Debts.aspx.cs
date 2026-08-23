using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalonCRM.Models;
using SalonCRM.Manager;
using SalonCRM.Web;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 欠款一览表
    /// </summary>
    /// <remarks>
    /// 欠款实为欠款购买的项目
    /// </remarks>
    public partial class Financial_Debts : AdminPage
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
                // cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                deEnd.Value = DateTime.Today;
            }

            if (cbBranch.Value != null || (deStart.Value != null && deStart.Value == deEnd.Value))
            {
                DebtQModel model = new DebtQModel();
                model.HostId = hostId;
                model.BranchId = Convert.ToInt32(cbBranch.Value);
                model.CardNo = txt_CardNo.Text.Trim();
                model.MemberName = txt_Name.Text.Trim();
                model.StartDate = Convert.ToDateTime(deStart.Text);
                model.EndDate = Convert.ToDateTime(deEnd.Text);
                model.Salesman = txt_xsr.Text.Trim();

                grid.DataSource = StatManager.GetDebtList(model);
                grid.DataBind();
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
        }
        protected void grid_HeaderFilterFillItems(object sender, DevExpress.Web.ASPxGridViewHeaderFilterEventArgs e)
        {

        }
        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "欠款一览表";
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }

    }
}