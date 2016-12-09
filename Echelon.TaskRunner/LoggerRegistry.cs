using Echelon.Core.Logging.Interfaces;
using Echelon.Core.Logging.Loggers;
using StructureMap.Configuration.DSL;

namespace Echelon.TaskRunner
{
    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            Scan(scan => For<IClientLogger>().Transient().Use(scope => new ClientLogger()));
        }
    }
}