using System;
using System.IO;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands.File;
using Echelon.Core.Logging.Loggers;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class DeleteFileConsumer : IConsumer<DeleteFileCommand>
    {
        private readonly IClientLogger _clientLogger;

        public DeleteFileConsumer(IClientLogger clientLogger)
        {
            _clientLogger = clientLogger;
        }

        public async Task Consume(ConsumeContext<DeleteFileCommand> context)
        {
            await Console.Out.WriteLineAsync($"Logging Info: {context.Message.FilePath}");
            _clientLogger.Info($"Deleting file: {context.Message.FilePath}");
            File.Delete(context.Message.FilePath);
        }
    }
}