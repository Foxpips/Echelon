using System;
using System.Configuration;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands;
using Echelon.Core.Infrastructure.Settings;
using MassTransit;

namespace Echelon.Core.Extensions.MassTransit
{
    public static class MassTransitExtensions
    {
        private static string QueueUrl => ConfigurationManager.AppSettings["RabbitMQHost"];

        public static async Task SendMessage<T>(this IBus bus, T command) where T : class, IBusCommand
        {
            var sendEndpoint = await bus.GetSendEndpoint(new Uri($"{QueueUrl}{QueueSettings.General}"));
            await sendEndpoint.Send(command);
        }

        public static async Task SendMessage<T>(this IBus bus, T command, string queueName) where T : class, IBusCommand
        {
            var sendEndpoint = await bus.GetSendEndpoint(new Uri($"{QueueUrl}{queueName}"));
            await sendEndpoint.Send(command);
        }

    }
}