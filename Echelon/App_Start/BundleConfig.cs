using System.Web.Optimization;

namespace Echelon
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/js")
                .IncludeDirectory("~/assets/js/minified/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .IncludeDirectory("~/assets/css/minified/", "*.css"));
        }
    }
}