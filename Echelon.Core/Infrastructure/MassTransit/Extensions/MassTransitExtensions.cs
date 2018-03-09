using System;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.Settings;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Extensions
{
    public static class MassTransitExtensions
    {
        public static async Task SendMessage<T>(this IBus bus, T command) where T : class, IBusCommand
        {
            var sendEndpoint = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/general_queue"));
            await sendEndpoint.Send(command);
        }

        public static async Task SendMessage<T>(this IBus bus, T command, string queueName) where T : class, IBusCommand
        {
            var sendEndpoint = await bus.GetSendEndpoint(new Uri($"{QueueSettings.QueueEndpoint}{queueName}"));
            await sendEndpoint.Send(command);
        }
    }

    public interface IBusCommand
    {
    }
}