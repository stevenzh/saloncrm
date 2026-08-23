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
    public partial class CardSalesDay : AdminPage
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
                cbBranch.Items.Insert(0, new ListEditItem("", ""));

                cbCardType.DataSource = CommonManager.GetDictionaries("MemberCardType");
                cbCardType.ValueField = "KeyValue";
                cbCardType.TextField = "Contents";
                cbCardType.DataBind();
                cbCardType.Items.Insert(0, new ListEditItem("请选择", ""));

                cbCardTmpl.DataSource = CommonManager.GetCardTmpls(hostId, "");
                cbCardTmpl.ValueField = "TmplID";
                cbCardTmpl.TextField = "Title";
                cbCardTmpl.DataBind();
                cbCardTmpl.ValueType = typeof(Int32);
                cbCardTmpl.Items.Insert(0, new ListEditItem("请选择", ""));

                for (var i = 2016; i <= DateTime.Now.Year; i++)
                {
                    cbYear.Items.Add(new ListEditItem(i.ToString(), i.ToString()));
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
                string cardTmpl = Convert.ToString(cbCardTmpl.Value);
                string cardType = Convert.ToString(cbCardType.Value);
                string Name = txt_Name.Text.Trim();

                grid.DataSource = new StatManager().CardSalesDay(hostId, Organ, cardType, cardTmpl, cbYear.Text, Month, Convert.ToString( cbStat.Value));
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
                        grid.GroupBy(grid.Columns["CardTitle"]);
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
            string fileName = "会员卡销售一览表";
            if (cbYear.Text != "")
                fileName += "-" + cbYear.Text;
            else if (cbMonth.Text != "")
                fileName += "-" + cbMonth.Text;
            else
                fileName += "-" + DateTime.Now.ToString("yyyy-MM-dd");
            gridExport.FileName = fileName;
            gridExport.WriteXlsToResponse();
        }

        protected void cbCardTmpl_Callback(object sender, CallbackEventArgsBase e)
        {
            FillCityCombo(e.Parameter);
        }

        protected void FillCityCombo(string cardType)
        {
            if (string.IsNullOrEmpty(cardType)) return;
            int hostId = GlobalContext.Current.UserHost.HostID;


            cbCardTmpl.DataSource = CommonManager.GetCardTmpls(hostId, Convert.ToString(cardType));
            cbCardTmpl.ValueField = "TmplID";
            cbCardTmpl.TextField = "Title";
            cbCardTmpl.DataBind();
            //cbCardTmpl.Value = country.City.CityName;
            cbCardTmpl.Items.Insert(0, new ListEditItem("请选择", ""));
        }
    }
}