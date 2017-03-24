using System;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands.Email;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Logging.Loggers;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class SendSignupEmailConsumer : IConsumer<SendSignupEmailCommand>
    {
        private readonly IClientLogger _clientLogger;

        public SendSignupEmailConsumer(IClientLogger clientLogger)
        {
            _clientLogger = clientLogger;
        }

        public async Task Consume(ConsumeContext<LogInfoCommand> context)
        {
            await Console.Out.WriteLineAsync($"Logging Info: {context.Message.Content}");
            _clientLogger.Info($"Logging Info: {context.Message.Content}");
        }

        public async Task Consume(ConsumeContext<SendSignupEmailCommand> context)
        {
            await Console.Out.WriteLineAsync($"Logging Info: {context.Message.UserName}");
        }
    }
}