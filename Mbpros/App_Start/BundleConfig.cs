using System.Web;
using System.Web.Optimization;

namespace Mbpros
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/content/css/Allcss").Include(
                "~/content/css/bootstrap.min.css",
                "~/content/css/font-awesome.css",
                "~/content/css/custom.css",
                "~/content/css/site.css",
                "~/content/css/style.css",
                "~/content/css/bootstrap-datetimepicker.css",
                "~/content/css/jquery-ui.min.css",
                "~/content/assets/jqgrid/themes/redmond/jquery-ui-1.7.1.custom.css",
                "~/content/assets/jqgrid/themes/ui.jqgrid.css"
                //,
                //"~/content/css/dataTables.bootstrap.min.css",
                //"~/content/css/dataTables.responsive.css.css"
            ));
            //bundles.Add(new ScriptBundle("~/Bundle/js").Include(
            //     "~/content/js/bootstrap.min.js"
            //     , "~/content/js/moment.js"
            //     , "~/content/js/bootstrap-datetimepicker.min.js"
            //     //, "~/content/js/jquery.dataTables.min.js"
            //     //, "~/content/js/dataTables.bootstrap.min.js"
            //    , "~/Scripts/jquery.maskedinput.min.js"
            //     ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryGrid").Include(
            //"~/content/assets/jqgrid/js/i18n/grid.locale-en.js",
            //"~/content/assets/jqgrid/js/jquery.jqGrid.min.js"));

            //bundles.Add(new ScriptBundle("~/Bundle/js2").Include(
            //    // "~/Scripts/jquery-1.12.0.min.js"
            //    "~/Scripts/jquery-ui-1.10.3.min.js"
            //    , "~/Scripts/jquery-2.0.3.min.js"
            //    , "~/Scripts/jquery.validate.min.js"
            //    //, "~/Scripts/jquery.confirm.min.js"
            //    ,"~/Scripts/jquery.jconfirm-1.0.js"
            //    ));

            bundles.Add(new ScriptBundle("~/Bundle/Patient").Include(
                   
                   //"~/Scripts/patient.js",
                   "~/Scripts/api.js"
                   ,"~/Scripts/datepickerjs.js"
                ));

            bundles.Add(new ScriptBundle("~/Bundle/Billing").Include(

               //"~/Scripts/billing.js",
                   "~/Scripts/api.js",
                   "~/Scripts/datepickerjs.js"
            ));

     
        }
    }
}