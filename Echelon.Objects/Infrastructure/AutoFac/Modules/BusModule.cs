using System;
using System.Configuration;
using Autofac;
using MassTransit;

namespace Echelon.Objects.Infrastructure.AutoFac.Modules
{
    public class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Registers all consumers with our container
            builder.RegisterConsumers(GetType().Assembly);

            // Creates our bus from the factory and registers it as a singleton against two interfaces
            builder.Register(c => Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                sbc.ReceiveEndpoint(host, ConfigurationManager.AppSettings["QueueName"], ep =>
                {
                    ep.LoadFrom(c.Resolve<ILifetimeScope>());
                });
            }))
                .As<IBusControl>()
                .As<IBus>()
                .SingleInstance();

        }
    }
}
