using System;
using Autofac;
using MassTransit;
using static System.Configuration.ConfigurationManager;

namespace Echelon.Core.Infrastructure.AutoFac.Modules
{
    public class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Registers all consumers with our container
            builder.RegisterConsumers(GetType().Assembly);

            // Creates our bus from the factory and registers it as a singleton against two interfaces
            builder.Register(componentContext => Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri(AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(AppSettings["RabbitMQUsername"]);
                    h.Password(AppSettings["RabbitMQPassword"]);
                });

                sbc.ReceiveEndpoint(host, AppSettings["QueueName"], ep =>
                {
                    ep.LoadFrom(componentContext.Resolve<ILifetimeScope>());
                });
            }))
                .As<IBusControl>()
                .As<IBus>()
                .SingleInstance();
        }
    }
}