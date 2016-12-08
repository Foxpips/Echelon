using System;
using System.Configuration;
using Autofac;
using MassTransit;

namespace Echelon.MessageBus
{
    public class BusModule : Module
    {
        private readonly System.Reflection.Assembly[] _assembliesToScan;

        public BusModule(params System.Reflection.Assembly[] assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Registers all consumers with our container
            builder.RegisterConsumers(_assembliesToScan);

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
