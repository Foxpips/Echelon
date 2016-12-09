using System;
using System.Reflection;
using Autofac;
using Echelon.Core.Extensions.Autofac;
using Echelon.Core.Interfaces;
using MassTransit;

namespace Echelon.MessageBus
{
    public class TransitBusServer : IService, IDisposable
    {
        public IBusControl Bus;

        public void Initialize()
        {
            Bus = new ContainerBuilder()
                .RegisterCustomModules()
                .Build()
                .Resolve<IBusControl>();

            Bus.StartAsync();
        }

        public void Dispose()
        {
//            Bus.StopAsync();
        }
    }
}