using System.Globalization;
using System.Web;
using System.Web.Optimization;

namespace Qct.ERP.Retailing
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.3.min.js"));

            bundles.Add(
                new ScriptBundle("~/bundles/jquery.validate")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.additional-methods.js")
                .Include("~/Scripts/jquery.validate.custom-methods.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);
           
            //BundleTable.EnableOptimizations = false;
        }
        public static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                "~/Scripts/easyui-validatebox-ext.js",
                "~/Scripts/easyui-ex.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/qcts").Include(
                "~/Scripts/jquery-ex.js",
                "~/Scripts/qcts.js"));

            #region localization script bundles

            bundles.Add(new StyleBundle("~/bundles/jquery-culture").Include(
                string.Format("~/Scripts/lang/easyui-lang-{0}.js", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName ?? CultureInfo.CurrentUICulture.Name)));

            #endregion
        }
        public static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/qcts").Include("~/Content/site.css","~/Content/qcts.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap/easyui").Include("~/Content/themes/easyui/bootstrap/easyui.css"));
            bundles.Add(new StyleBundle("~/Content/default/easyui").Include("~/Content/themes/easyui/default/easyui.css"));
            bundles.Add(new StyleBundle("~/Content/easyui").Include("~/Content/themes/easyui/icon.css"));
        }
    }
}
