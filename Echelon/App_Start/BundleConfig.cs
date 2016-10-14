using System.Web.Optimization;

namespace Echelon
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/js")
                .IncludeDirectory("~/out/javascript/minified/", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                .IncludeDirectory("~/out/css/minified/", "*.css"));
        }
    }
}