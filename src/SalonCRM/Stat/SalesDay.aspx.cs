using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using SalonCRM.Manager;
using SalonCRM.Web;

namespace SalonCRM.Stat
{
    public partial class SalesDay : AdminPage
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
                cbBranch.Items.Insert(0, new DevExpress.Web.ListEditItem("", ""));

                cbCategory.DataSource = CommonManager.GetDictionaries("ProjectCategory");
                cbCategory.ValueField = "KeyValue";
                cbCategory.TextField = "Contents";
                cbCategory.DataBind();
                cbCategory.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                cbBrand.DataSource = CommonManager.GetBrands(hostId);
                cbBrand.ValueField = "KeyValue";
                cbBrand.TextField = "Contents";
                cbBrand.DataBind();
                cbBrand.Items.Insert(0, new DevExpress.Web.ListEditItem("请选择", ""));

                for (var i = 2016; i <= DateTime.Now.Year; i++)
                {
                    cbYear.Items.Add(new DevExpress.Web.ListEditItem(i.ToString(), i.ToString()));
                }
                cbBranch.Text = GlobalContext.Current.UserDepartment.Name;
                cbYear.Text = DateTime.Today.Year.ToString();
                cbMonth.Value = DateTime.Today.Month;
            }

            string Month = Convert.ToString(cbMonth.Value);
            if (cbYear.Text != "" && Month != "")
            {
                int Organ = default(int);
                if (cbBranch.Value != null)
                    Organ = Convert.ToInt32(cbBranch.Value);
                string Brand = Convert.ToString(cbBrand.Value);
                string Category = Convert.ToString(cbCategory.Value);
                string Name = txt_Name.Text.Trim();

                grid.DataSource = new StatManager().SalesDay(hostId, Organ, Brand, Category, Name, cbYear.Text, Month);
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