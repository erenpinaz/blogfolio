using System.Web.Optimization;

namespace Blogfolio.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Site

            // Site style bundle
            bundles.Add(new StyleBundle("~/assets/css/app")
                .Include("~/assets/css/normalize.css")
                .Include("~/assets/css/appicons.css")
                .Include("~/assets/css/animate.css")
                .Include("~/assets/css/remodal*")
                .Include("~/assets/css/prism.css")
                .Include("~/assets/css/site.css"));

            // Site script bundle
            bundles.Add(new ScriptBundle("~/assets/js/app")
                .Include("~/assets/js/jquery-{version}.js")
                .Include("~/assets/js/jquery.validate*")
                .Include("~/assets/js/jquery.unobtrusive-ajax.js")
                .Include("~/assets/js/jquery.lazyload.js")
                .Include("~/assets/js/remodal.js")
                .Include("~/assets/js/masonry.js")
                .Include("~/assets/js/imagesloaded.js")
                .Include("~/assets/js/prism.js")
                .Include("~/assets/js/site.js"));

            #endregion

            #region Admin

            // Admin style bundle
            bundles.Add(new StyleBundle("~/assets/css/dash")
                .Include("~/assets/css/appicons.css")
                .Include("~/assets/css/jquery.fonticonpicker*")
                .Include("~/assets/css/admin.css")
                .Include("~/assets/css/bootstrap-table.css"));

            // Admin script bundle
            bundles.Add(new ScriptBundle("~/assets/js/dash")
                .Include("~/assets/js/jquery-{version}.js")
                .Include("~/assets/js/jquery.validate*")
                .Include("~/assets/js/jquery.unobtrusive-ajax.js")
                .Include("~/assets/js/jquery.fonticonpicker.js")
                .Include("~/assets/js/sortable.js")
                .Include("~/assets/js/bootstrap.js")
                .Include("~/assets/js/bootstrap-table.js")
                .Include("~/assets/js/plupload/moxie.js")
                .Include("~/assets/js/plupload/plupload.dev.js")
                .Include("~/assets/js/admin.js"));

            #endregion

            // Enables versioning & minifying ( Web.Config override )
            //BundleTable.EnableOptimizations = true;
        }
    }
}