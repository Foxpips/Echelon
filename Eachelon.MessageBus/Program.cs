using System;
using Autofac;
using Echelon.Objects.Infrastructure.AutoFac.Modules;
using MassTransit;

namespace Echelon.MessageBus
{
    internal class Program
    {
        private static void Main()
        {

            var builder = new ContainerBuilder();
            builder.RegisterModule(new BusModule(typeof(Program).Assembly));
            builder.RegisterModule(new LoggingModule());
            
            var container = builder.Build();

            var bus = container.Resolve<IBusControl>();
            using (bus.StartAsync())
            {
                bus.Publish(new MyMessage { Value = "Hello, World." });
                bus.Publish(new MyMessage { Value = "Hello, World2." });
                bus.Publish(new UpdateCustomerAddress { CustomerId = "998" });

                Console.ReadLine();
            }
        }
    }
}