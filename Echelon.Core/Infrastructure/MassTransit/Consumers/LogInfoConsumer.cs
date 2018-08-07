using System;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Logging.Loggers;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class LogInfoConsumer : BaseConsumer<LogInfoCommand>
    {
        private readonly IClientLogger _clientLogger;

        public LogInfoConsumer(IClientLogger clientLogger) : base(clientLogger)
        {
            _clientLogger = clientLogger;
        }

        protected override async Task ConsumeInternal(ConsumeContext<LogInfoCommand> context)
        {
            await Console.Out.WriteLineAsync($"Logging Info: {context.Message.Content}");
            _clientLogger.Info($"Logging Info: {context.Message.Content}");
        }
    }
}