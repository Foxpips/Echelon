using System.Web.Optimization;

namespace TwilioIpMessaging
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/javascript").Include("~/out/javascript/minified/*.js"));
            bundles.Add(new StyleBundle("~/css").Include("~/out/css/minified/*.css"));
        }
    }
}