using Rhino.ServiceBus;
using Rhino.ServiceBus.Impl;
using StructureMap;

namespace Echelon.TaskRunner
{
    public class TaskRunnerClient<TBusType>
    {
        public TBusType Bus { get; set; }

        public TaskRunnerClient(IContainer container)
        {
            if (Bus == null)
            {
                new OnewayRhinoServiceBusConfiguration().UseStructureMap(container).Configure();
                Bus = container.GetInstance<TBusType>();
            }
        }
    }
}