using Rhino.ServiceBus.StructureMap;

namespace Echelon.TaskRunner
{
    public class TaskRunnerBusBootstrapper : StructureMapBootStrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.Configure(cfg => { cfg.AddRegistry<LoggerRegistry>(); });
        }
    }
}