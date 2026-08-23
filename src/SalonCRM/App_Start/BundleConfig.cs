using System.Web.Optimization;

namespace SalonCRM
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.pagination.js",
                        "~/Scripts/underscore.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-multiselect.js",
                      "~/Scripts/locales/bootstrap-datepicker.zh-CN.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-multiselect.css",
                      "~/Content/datepicker3.css",
                      "~/Content/site.css"));

            #region AdminLTE
            bundles.Add(new ScriptBundle("~/bundles/adminlte").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js",
                      "~/Content/slimScroll/jquery.slimscroll.min.js",
                      "~/Content/AdminLTE/app.js",
                      "~/Content/AdminLTE/demo.js"));

            bundles.Add(new StyleBundle("~/Content/adminlte").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/AdminLTE/AdminLTE.min.css",
                      "~/Content/AdminLTE/skins/_all-skins.min.css"));
            #endregion
        }
    }
}
