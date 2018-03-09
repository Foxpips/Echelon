using System;
using Autofac;
using Echelon.Core.Infrastructure.Settings;
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

                sbc.ReceiveEndpoint(host, QueueSettings.Registration, ep =>
                {
                    ep.LoadFrom(componentContext.Resolve<ILifetimeScope>());
                });

                sbc.ReceiveEndpoint(host, QueueSettings.General, ep =>
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