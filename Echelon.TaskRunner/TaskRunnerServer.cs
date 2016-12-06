using Echelon.Core.Logging.Interfaces;
using Echelon.Core.Logging.Loggers;
using Rhino.ServiceBus.Hosting;
using Rhino.ServiceBus.StructureMap;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Echelon.TaskRunner
{
    public class TaskRunnerServer
    {
        public void Start()
        {
            var host = new DefaultHost();
            host.Start<TaskRunnerBusBootstrapper>();
        }
    }

    public class TaskRunnerBusBootstrapper : StructureMapBootStrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.Configure(cfg =>
            {
                cfg.AddRegistry<LoggerRegistry>();
            });
        }
    }

    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            Scan(scan => For<IClientLogger>().Transient().Use(scope => new ClientLogger()));
        }
    }
}