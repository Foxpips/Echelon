using Autofac;
using Rhino.ServiceBus.Impl;

namespace Echelon.TaskRunner
{
    public class TaskRunnerClient<TBusType>
    {
        public TBusType Bus { get; set; }

        public TaskRunnerClient(IContainer container)
        {
            if (Bus == null)
            {
                new OnewayRhinoServiceBusConfiguration().UseAutofac(container).Configure();
                Bus = container.Resolve<TBusType>();
            }
        }
    }
}