using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalonCRM.Web;
using SalonCRM.Manager;
using SalonCRM.Identity;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 预约到店成交统计
    /// </summary>
    public partial class Customer_transaction : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;

            if (!this.IsPostBack)
            {
                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                if (deStart.Value == null)
                    deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                if (deEnd.Value == null)
                    deEnd.Value = DateTime.Today;

                if (cu.Type != "2")
                {
                    cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                    cbBranch.ReadOnly = true;
                }
            }


            int branch = default(int);
            if (cbBranch.Value != null)
                branch = Convert.ToInt32(cbBranch.Value);

            DateTime Start = Convert.ToDateTime(deStart.Value);
            DateTime End = Convert.ToDateTime(deEnd.Value);
            var dt = new StatManager().Customer_transaction(hostId, Convert.ToInt32(cbBranch.Value), Start, End);
            grid.DataSource = dt;
            grid.DataBind();
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "预约到店成交统计";
            if (deStart.Text != "")
                fileName += "-" + deStart.Text;
            else if (deEnd.Text != "")
                fileName += "-" + deEnd.Text;
            else
                fileName += "-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}