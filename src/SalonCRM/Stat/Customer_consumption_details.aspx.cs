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
    /// 客户消费明细
    /// </summary>
    public partial class Customer_consumption_details : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            if (!IsPostBack)
            {
                string cardNo = Request.QueryString["cardno"];
                if (!String.IsNullOrEmpty(cardNo))
                    tbCardNo.Text = cardNo;
                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                //cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));
                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;

                deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                deEnd.Value = DateTime.Today;
            }

            int branchId = Convert.ToInt32(cbBranch.Value);
            string CardNo = tbCardNo.Text.Trim();
            DateTime startDate = Convert.ToDateTime(deStart.Text);
            DateTime endDate = Convert.ToDateTime(deEnd.Text);
            grid.DataSource = StatManager.GetCustomerPTDetail(hostId, branchId, startDate, endDate,  CardNo);
            grid.DataBind(); //执行绑定
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
        }
        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "客户消费明细";
            if (tbCardNo.Text != "")
                fileName += "-卡号[" + tbCardNo.Text + "]";

            fileName += "-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}