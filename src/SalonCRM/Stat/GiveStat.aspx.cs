using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalonCRM.Web;
using SalonCRM.Manager;
using SalonCRM.Models;
using SalonCRM.Identity;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 客户赠送统计
    /// </summary>
    public partial class GiveStat : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            CustomPrincipal cu = (CustomPrincipal)User;

            if (!IsPostBack)
            {
                cbBranch.DataSource = CommonManager.GetBranchs(hostId);
                cbBranch.TextField = "Name";
                cbBranch.ValueField = "OrganID";
                cbBranch.DataBind();
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                deStart.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                deEnd.Value = DateTime.Today;

                if (cu.Type != "2")
                {
                    cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                    cbBranch.ReadOnly = true;
                }
            }

            if (cbBranch.Value != null || deStart.Value != null && deEnd.Value != null)
            {
                Int32 branchId = Convert.ToInt32(cbBranch.Value);
                DateTime start = Convert.ToDateTime(deStart.Value);
                DateTime end = Convert.ToDateTime(deEnd.Value).AddDays(1);
                List<GiveModel> dt = StatManager.Customer_Giving_statistical(hostId, branchId, start, end);
                grid.DataSource = dt;//指定Grid的数据
                grid.DataBind(); //执行绑定
            }
        }

        protected void btnStat_Click(object sender, EventArgs e)
        {

        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "赠送统计";
            if (cbBranch.Value != null)
                fileName += "-" + cbBranch.Text;
            if (deStart.Value != null)
                fileName += deStart;
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}