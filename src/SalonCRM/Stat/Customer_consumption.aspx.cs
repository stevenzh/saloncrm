using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalonCRM.Web;
using DevExpress.Web;
using SalonCRM.Manager;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 客户消费/消耗一览表
    /// </summary>
    public partial class Customer_consumption : AdminPage
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
                //cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));
                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;

                deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                deEnd.Value = DateTime.Today;
            }

            if (Convert.ToString(cbBranch.Value) != "" && deStart.Text != "" && deEnd.Text != "")
            {
                int branchId = Convert.ToInt32(cbBranch.Value);
                string Name = txt_Name.Text.Trim();
                string CardNo = txt_CardNo.Text.Trim();
                decimal Amount_s = string.IsNullOrEmpty(txt_Amount_s.Text) ? 0 : Convert.ToDecimal(txt_Amount_s.Text.Trim());
                decimal Amount_b = string.IsNullOrEmpty(txt_Amount_b.Text) ? 0 : Convert.ToDecimal(txt_Amount_b.Text.Trim());
                DateTime startDate = Convert.ToDateTime(deStart.Text);
                DateTime endDate = Convert.ToDateTime(deEnd.Text);
                grid.DataSource = StatManager.GetCustomerPTList(hostId, branchId, startDate, endDate, Name, CardNo);
                grid.DataBind();
                grid.DetailRows.ExpandRow(0);
            }
        }


        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "客户消费/消耗统计";
            if (deStart.Text != "")
                fileName += "-" + deStart.Text;
            else if (deEnd.Text != "")
                fileName += "-" + deEnd.Text;
            else
                fileName += "-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }

        protected void detailGrid_DataSelect(object sender, EventArgs e)
        {
            Session["MemberId"] = (sender as ASPxGridView).GetMasterRowKeyValue();
            Session["StartDate"] = Convert.ToDateTime(deStart.Text);
            Session["EndDate"] = Convert.ToDateTime(deEnd.Text);
        }

    }
}