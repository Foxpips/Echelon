using Autofac;
using Echelon.Core.Extensions.Autofac;
using Echelon.Core.Infrastructure.Services.Windows;
using MassTransit;

namespace Echelon.TaskRunner
{
    public class MassTransitService : IWindowsService
    {
        public void Initialize()
        {
            var targetAssembly = GetType().Assembly;
            var builder = new ContainerBuilder();
            var container = builder.RegisterCustomModules(true, targetAssembly).Build();

            var bus = container.Resolve<IBusControl>();
            bus.StartAsync();
        }
    }
}