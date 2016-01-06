using System.Web;
using System.Web.Optimization;

namespace MvcPWy
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/bower_components/jquery/dist/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/bower_components/jquery-ui/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/bower_components/jquery.validate/jquery.validate.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/bower_components/modernizr/Modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/bower_components/bootstrap/dist/js/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/bower_components/bootstrap/dist/css/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                "~/bower_components/angularjs/anguar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                         "~/Scripts/Main.js"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
