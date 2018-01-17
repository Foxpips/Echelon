using System;
using System.IO;
using System.Web.Optimization;

namespace Echelon
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundle/js")
                .IncludeDirectory("~/assets/js/minified/", "*.js"));

            var directories = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\assets\\css");
            foreach (var directory in directories)
            {
                var cssFolder = Path.GetFileName(directory);
                bundles.Add(new StyleBundle($"~/bundle/css/{cssFolder}")
                    .IncludeDirectory($"~/assets/css/{cssFolder}/minified/", "*.css"));
            }
        }
    }
}