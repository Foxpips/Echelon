using System.Threading.Tasks;
using Echelon.Core.Logging.Loggers;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public abstract class BaseConsumer<T> : IConsumer<T>, IConsumer<Fault<T>> where T : class
    {
        protected readonly IClientLogger ClientLogger;

        protected BaseConsumer(IClientLogger logger)
        {
            ClientLogger = logger;
        }

        //External Consume Method
        protected abstract Task ConsumeInternal(ConsumeContext<T> context);

        //Internal Consume Method
        public async Task Consume(ConsumeContext<T> context)
        {
            await ConsumeInternal(context);
        }

        //Fault Handler
        public Task Consume(ConsumeContext<Fault<T>> context)
        {
            ClientLogger.Error(context.Message.Message);
            foreach (var exceptionInfo in context.Message.Exceptions)
            {
                ClientLogger.Error(exceptionInfo.Message);
            }

            return null;
        }
    }
}