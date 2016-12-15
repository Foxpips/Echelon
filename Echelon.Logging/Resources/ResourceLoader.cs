using System.IO;
using System.Linq;

namespace Echelon.Core.Logging.Resources
{
    public class ResourceLoader
    {
        public static string GetResourceContent(string fileNameWithExtension)
        {
            string result;
            var executingAssembly = typeof(ResourceLoader).Assembly;

            using (var stream = executingAssembly.GetManifestResourceStream(
                executingAssembly.GetManifestResourceNames()
                    .FirstOrDefault(rn => rn.Contains(fileNameWithExtension))))
            {
                if (stream == null) return string.Empty;

                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
    }
}