using Echelon.Core.Helpers;
using Echelon.Core.Interfaces;
using Rhino.ServiceBus.Hosting;

namespace Echelon.TaskRunner
{
    public class TaskRunnerServer : IService
    {
        public void Initialize()
        {
            var host = new DefaultHost();
            host.Start<TaskRunnerBusBootstrapper>();
        }
    }
}