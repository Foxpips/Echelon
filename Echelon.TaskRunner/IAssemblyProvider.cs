using System.Collections.Generic;
using System.Reflection;

namespace Echelon.TaskRunner
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}