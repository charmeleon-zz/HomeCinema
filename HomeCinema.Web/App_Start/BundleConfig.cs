using System.Web.Optimization;

namespace HomeCinema.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var vendorPath = "~/Scripts/vendors";
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                $"{vendorPath}/modernizr.js"));
            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                $"{vendorPath}/jquery.js",
                $"{vendorPath}/bootstrap.js",
                $"{vendorPath}/toastr.js",
                $"{vendorPath}/jquery.raty.js",
                $"{vendorPath}/respond.src.js",
                $"{vendorPath}/angular.js",
                $"{vendorPath}/angular-route.js",
                $"{vendorPath}/angular-cookies.js",
                $"{vendorPath}/angular-validator.js",
                $"{vendorPath}/angular-base64.js",
                $"{vendorPath}/angular-file-upload.js",
                $"{vendorPath}/angucomplete-alt.min.js",
                $"{vendorPath}/ui-bootstrap-tpls-0.13.1.js",
                $"{vendorPath}/underscore.js",
                $"{vendorPath}/raphael.js",
                $"{vendorPath}/morris.js",
                $"{vendorPath}/jquery.fancybox.js",
                $"{vendorPath}/jquery.fancybox-media.js",
                $"{vendorPath}/loading-bar.js"));

            var spaPath = "~/Scripts/spa";
            bundles.Add(new ScriptBundle("~/bundles/spa").Include(
                $"{spaPath}/modules/common.core.js",
                $"{spaPath}/modules/common.ui.js",
                $"{spaPath}/app.js",
                $"{spaPath}/services/apiService.js",
                $"{spaPath}/services/notificationService.js",
                $"{spaPath}/services/membershipService.js",
                $"{spaPath}/services/fileUploadService.js",
                $"{spaPath}/layout/topBar.directive.js",
                $"{spaPath}/layout/sideBar.directive.js",
                $"{spaPath}/layout/customPager.directive.js",
                $"{spaPath}/account/loginCtrl.js",
                $"{spaPath}/account/registerCtrl.js",
                $"{spaPath}/customers/customersCtrl.js",
                $"{spaPath}/customers/customersRegCtrl.js",
                $"{spaPath}/customers/customersEditCtrl.js",
                $"{spaPath}/account/loginCtrl.js",
                $"{spaPath}/account/registerCtrl.js",
                $"{spaPath}/home/rootCtrl.js",
                $"{spaPath}/home/indexCtrl.js",
                $"{spaPath}/customers/customersCtrl.js",
                $"{spaPath}/customers/customersRegCtrl.js",
                $"{spaPath}/customers/customerEditCtrl.js",
                $"{spaPath}/movies/moviesCtrl.js",
                $"{spaPath}/movies/movieAddCtrl.js",
                $"{spaPath}/movies/movieDetailsCtrl.js",
                $"{spaPath}/movies/movieEditCtrl.js",
                $"{spaPath}/controllers/rentalCtrl.js",
                $"{spaPath}/rental/rentMovieCtrl.js",
                $"{spaPath}/rental/rentStatsCtrl.js"
                ));

            var content = "~/Content/css";
            bundles.Add(new StyleBundle("~/Content/css").Include(
                $"{content}/site.css",
                $"{content}/bootstrap.css",
                $"{content}/bootstrap-theme.css",
                $"{content}/font-awesome.css",
                $"{content}/morris.css",
                $"{content}/toastr.css",
                $"{content}/jquery.fancybox.css",
                $"{content}/loading-bar.css"
                ));
            BundleTable.EnableOptimizations = false;
        }
    }
}