using System;
using System.Threading.Tasks;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Extensions
{
    public static class MassTransitExtensions
    {
        public static async Task SendMessage<T>(this IBus bus, T command) where T : class, IBusCommand
        {
            var sendEndpoint = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/echelon_queue"));
            await sendEndpoint.Send(command);
        }

        public static async Task SendMessage<T>(this IBus bus, T command, string endpoint) where T : class, IBusCommand
        {
            var sendEndpoint = await bus.GetSendEndpoint(new Uri(endpoint));
            await sendEndpoint.Send(command);
        }
    }

    public interface IBusCommand
    {
    }
}