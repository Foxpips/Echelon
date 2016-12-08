using System;
using System.Threading.Tasks;
using Echelon.Core.Logging.Interfaces;
using MassTransit;

namespace Echelon.MessageBus
{
    public class UpdateCustomerConsumer : IConsumer<UpdateCustomerAddress>
    {
        private readonly IClientLogger _clientLogger;

        public UpdateCustomerConsumer(IClientLogger clientLogger)
        {
            _clientLogger = clientLogger;
        }

        public async Task Consume(ConsumeContext<UpdateCustomerAddress> context)
        {
            await Console.Out.WriteLineAsync($"Updating customer: {context.Message.CustomerId}");
            _clientLogger.Info($"Updating customer: {context.Message.CustomerId}");
        }
    }

    public class UpdateCustomerAddress
    {
        public string CustomerId { get; set; }
    }
}