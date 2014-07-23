using System.Web;
using System.Web.Optimization;

namespace alieziaherman.co.za
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-2.1.1.js",
                        "~/Scripts/jquery/jquery.easing.min.js",
                        "~/Scripts/jquery/supersized.3.2.7.js",
                         "~/Scripts/jquery/supersized.shutter.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular/angular.js",
                      "~/Scripts/angular/angular-*",
                       "~/Scripts/angular/ui-bootstrap-tpls-0.11.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
            "~/Scripts/app/app.js",
            "~/Scripts/app/services.js",
            "~/Scripts/app/controllers/main.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/reset.css",
                "~/Content/jquery/supersized.css",
                 "~/Content/jquery/supersized.shutter.css",
                 "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap/bootstrap.css",
                "~/Content/bootstrap/bootstrap-theme.css"));


        }
    }
}