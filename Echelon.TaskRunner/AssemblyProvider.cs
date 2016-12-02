using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Echelon.TaskRunner
{
    public class DefaultAssemblyProvider : IAssemblyProvider
    {
        private readonly IList<System.Reflection.Assembly> _assemblies;

        public DefaultAssemblyProvider(Predicate<string> predicate,
            Func<System.Reflection.Assembly, bool> orderFunc)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Contains(@"\bin")
                ? AppDomain.CurrentDomain.BaseDirectory
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    AppDomain.CurrentDomain.SetupInformation.PrivateBinPath ?? string.Empty);

            var files = Directory.EnumerateFiles(path).Where(x =>
            {
                var fileInfo = new FileInfo(x);
                return predicate(fileInfo.Name) &&
                       (fileInfo.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase) ||
                        fileInfo.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase));
            });

            _assemblies =
                files.Select(System.Reflection.Assembly.LoadFrom)
                    .OrderBy(orderFunc)
                    .ToList();
        }

        public IEnumerable<System.Reflection.Assembly> GetAssemblies()
        {
            return _assemblies;
        }
    }
}