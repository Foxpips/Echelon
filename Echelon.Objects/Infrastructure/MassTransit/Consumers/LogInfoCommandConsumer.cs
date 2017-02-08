using System;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands;
using Echelon.Core.Logging.Loggers;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class LogInfoCommandConsumer : IConsumer<LogInfoCommand>
    {
        private readonly IClientLogger _clientLogger;

        public LogInfoCommandConsumer(IClientLogger clientLogger)
        {
            _clientLogger = clientLogger;
        }

        public async Task Consume(ConsumeContext<LogInfoCommand> context)
        {
            await Console.Out.WriteLineAsync($"Logging Info: {context.Message.Content}");
            _clientLogger.Info($"Logging Info: {context.Message.Content}");
        }
    }
}