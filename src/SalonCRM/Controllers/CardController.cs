using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using System.Drawing.Printing;
using SalonCRM.Models;
using SalonCRM.Web;
using SalonCRM.Identity;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.Web;

namespace SalonCRM.Controllers
{
    /// <summary>
    /// 换卡
    /// </summary>
    [CustomAuthorize]
    public class CardController : Controller
    {
        ApplicationDbContext dbcontent = new ApplicationDbContext();

        /// <summary>
        /// 换卡
        /// </summary>
        /// <param name="qmodel"></param>
        /// <returns></returns>
        public ActionResult Index(CardRepQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;

            List<SelectListItem> items = new SelectList(dbcontent.Organs.Where(t => t.HostID == hostId).ToList(), "OrganID", "Name").ToList();
            items.Insert(0, new SelectListItem { Value = "0", Text = "--请选择--" });
            ViewBag.OrganId = items;

            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            qmodel.RelaceList = GetReplaceList(qmodel);
            return View(qmodel);
        }

        /// <summary>
        /// 换卡
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplaceList(CardRepQModel qmodel)
        {
            ViewData["Name"] = qmodel.Name;
            ViewData["BranchId"] = qmodel.BranchId;
            ViewData["CardNo"] = qmodel.CardNo;
            ViewData["Mobile"] = qmodel.Mobile;
            return PartialView("ReplaceList", GetReplaceList(qmodel));
        }


        /// <summary>
        /// 换卡列表
        /// </summary>
        /// <returns></returns>
        private IList<CardReplaceViewModel> GetReplaceList(CardRepQModel qmodel)
        {
            int hostId = GlobalContext.Current.UserHost.HostID;
            var query = dbcontent.EventLogs.Where(t => t.HostId == hostId && t.TypeId == 4);
            var mem = dbcontent.Members.AsQueryable();
            if (qmodel.BranchId != default(int))
                mem = mem.Where(t => t.JoinBranch == qmodel.BranchId);
            if (!String.IsNullOrEmpty(qmodel.Name))
                mem = mem.Where(t => t.Name.Contains(qmodel.Name));
            if (!String.IsNullOrEmpty(qmodel.CardNo))
                mem = mem.Where(t => t.CardNo == qmodel.CardNo);
            if (!String.IsNullOrEmpty(qmodel.Mobile))
                mem = mem.Where(t => t.MobileNumber == qmodel.Mobile);

            IList<CardReplaceViewModel> logs = (from vm in query
                                                join br in dbcontent.Organs on vm.BranchId equals br.OrganID
                                                join mv in mem on vm.MemberId equals mv.MemberID
                                                select new CardReplaceViewModel
                                                {
                                                    LogId = vm.LogId,
                                                    MemberId = mv.MemberID,
                                                    BranchName = br.Name,
                                                    MemberName = mv.Name,
                                                    Shell = vm.Shell,
                                                    CreatedDate = vm.CreatedDate
                                                }).ToList();
            return logs;

        }


        public ActionResult ExportToPDF(CardRepQModel viewModel)
        {
            var printable = GridViewExtension.CreatePrintableObject(GridViewSettings, GetReplaceList(viewModel));

            PrintingSystem ps = new PrintingSystem();

            PrintableComponentLink link1 = new PrintableComponentLink(ps);
            link1.Component = printable;

            link1.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            link1.Landscape = true;
            CompositeLink compositeLink = new CompositeLink(ps);
            compositeLink.Links.Add(link1);


            compositeLink.CreateDocument();
            using (MemoryStream stream = new MemoryStream())
            {
                compositeLink.PrintingSystem.ExportToXls(stream);
                WriteToResponse("客户列表", true, "xls", stream);
            }
            ps.Dispose();

            return Index(new CardRepQModel());
        }

        static GridViewSettings exportGridViewSettings;
        public static GridViewSettings GridViewSettings
        {
            get
            {
                if (exportGridViewSettings == null)
                    exportGridViewSettings = GetGridViewSettings();
                return exportGridViewSettings;
            }
        }
        static GridViewSettings GetGridViewSettings()
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "GridView";
            settings.CallbackRouteValues = new { Controller = "LogView", Action = "Grid" };
            settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            settings.Theme = "BlackGlass";
            settings.KeyFieldName = "Id";
            settings.SettingsPager.Visible = true;
            settings.Settings.ShowGroupPanel = true;
            settings.Settings.ShowFilterRow = true;
            settings.SettingsBehavior.AllowSelectByRowClick = true;
            settings.SettingsPager.PageSize = 25;
            settings.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;
            settings.Settings.ShowHeaderFilterButton = true;
            settings.SettingsPopup.HeaderFilter.Height = 200;
            settings.SettingsExport.Landscape = true;
            settings.SettingsExport.TopMargin = 0;
            settings.SettingsExport.LeftMargin = 0;
            settings.SettingsExport.RightMargin = 0;
            settings.SettingsExport.BottomMargin = 0;
            settings.SettingsExport.PaperKind = PaperKind.A4;
            settings.SettingsExport.RenderBrick = (sender, e) =>
            {
                if (e.RowType == GridViewRowType.Data && e.VisibleIndex % 2 == 0)
                    e.BrickStyle.BackColor = System.Drawing.Color.FromArgb(0xEE, 0xEE, 0xEE);
            };

            settings.Columns.Add("MemberName", "客户");
            settings.Columns.Add("BranchName", "门店");
            settings.Columns.Add("UserName", "操作人");
            settings.Columns.Add("OriginalCardNo", "原卡号");
            settings.Columns.Add("NewCardNo", "新卡号");
            settings.Columns.Add(column =>
            {
                column.FieldName = "CreatedDate";
                column.Caption = "操作时间";
                column.PropertiesEdit.DisplayFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            settings.Settings.ShowPreview = true;
            return settings;
        }

        void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
        {
            string disposition = saveAsFile ? "attachment" : "inline";
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition",
            string.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat));
            Response.BinaryWrite(stream.GetBuffer());
            Response.End();
        }
    }
}