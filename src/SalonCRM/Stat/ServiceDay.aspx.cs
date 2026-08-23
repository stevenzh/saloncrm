using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using SalonCRM.Manager;
using SalonCRM.Web;
using SalonCRM.Identity;

namespace SalonCRM.Stat
{
    public partial class ServiceDay : AdminPage
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
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("", ""));

                cbCategory.DataSource = CommonManager.GetDictionaries("ProjectCategory");
                cbCategory.ValueField = "KeyValue";
                cbCategory.TextField = "Contents";
                cbCategory.DataBind();
                cbCategory.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                for (var i = 2015; i <= DateTime.Now.Year; i++)
                {
                    cbYear.Items.Add(new DevExpress.Web.ListEditItem(i.ToString(), i.ToString()));
                }
                if (cu.Type != "2")
                {
                    cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                    cbBranch.ReadOnly = true;
                }
                cbYear.Text = DateTime.Today.Year.ToString();
                cbMonth.Value = DateTime.Today.Month;
            }

            string Month = Convert.ToString(cbMonth.Value);
            if (cbYear.Text != "" && Month != "")
            {
                int Organ = default(int);
                if (cbBranch.Value != null)
                    Organ = Convert.ToInt32(cbBranch.Value);
                string Brand = txt_Brand.Text.Trim();
                string Category = cbCategory.Text;
                string Name = txt_Name.Text.Trim();

                grid.DataSource = new StatManager().ServiceDay(hostId, Organ, Brand, Category, Name, cbYear.Text, Month);
                grid.DataBind();
                ApplyLayout(Convert.ToInt32(cbFields.Value));
            }

        }

        protected void btnStat_Click(object sender, EventArgs e)
        {
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ApplyLayout(Int32.Parse(e.Parameters));
        }

        void ApplyLayout(int layoutIndex)
        {
            grid.BeginUpdate();
            try
            {
                grid.ClearSort();
                switch (layoutIndex)
                {
                    case 0:
                        grid.GroupBy(grid.Columns["BranchName"]);
                        break;
                    case 1:
                        grid.GroupBy(grid.Columns["ProjectName"]);
                        break;
                }
            }
            finally
            {
                grid.EndUpdate();
            }
            grid.ExpandAll();
        }

        protected void btnXlsExport_Click(object sender, EventArgs e)
        {
            string fileName = "项目销售、消耗一览表";
            if (cbYear.Text != "")
                fileName += "-" + cbYear.Text;
            else if (cbMonth.Text != "")
                fileName += "-" + cbMonth.Text;
            else
                fileName += "-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }
    }
}