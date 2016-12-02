using Rhino.ServiceBus.Hosting;

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
}