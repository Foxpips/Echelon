using System.Threading.Tasks;
using Autofac;
using Echelon.Core.Extensions.Autofac;
using Echelon.Core.Infrastructure.Services.Windows;
using MassTransit;

namespace Echelon.TaskRunner
{
    public class MassTransitService : IWindowsService
    {
        private IBusControl _bus;

        public async Task Initialize()
        {
            var targetAssembly = GetType().Assembly;
            var builder = new ContainerBuilder();
            var container = builder.RegisterCustomModules(true, targetAssembly).Build();

            _bus = container.Resolve<IBusControl>();
            await _bus.StartAsync();
        }

        public async Task Shutdown()
        {
            await _bus.StopAsync();
        }
    }
}