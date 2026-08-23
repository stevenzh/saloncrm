using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SalonCRM.Controllers;
using SalonCRM.Web;
using SalonCRM.Manager;

namespace SalonCRM.Stat
{
    /// <summary>
    /// 美容师服务明细
    /// </summary>
    public partial class Financial_Service_details : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
            }
            string userId = Request.QueryString["ID"].ToString();
            DateTime StartDate = DateTime.Parse( Request.QueryString["StartDate"].ToString());
            DateTime EndDate = DateTime.Parse( Request.QueryString["EndDate"].ToString());
            if (!string.IsNullOrEmpty(userId))
            {
                var employee = UserManager.FindById(userId);
                if (employee != null)
                {
                    lbl_Employee.Text = employee.UserCnName;
                    grid.DataSource = new StatManager().WorkServiceDetails(userId, StartDate, EndDate);
                    grid.DataBind();
                }
            }
        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "美容师服务明细[" + lbl_Employee.Text;
            fileName += "]-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}