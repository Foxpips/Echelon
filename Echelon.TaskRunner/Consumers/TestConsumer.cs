using Echelon.Core.Logging.Interfaces;
using Echelon.TaskRunner.Commands;
using Rhino.ServiceBus;

namespace Echelon.TaskRunner.Consumers
{
    public class TestConsumer : ConsumerOf<TestCommand>
    {
        private readonly IClientLogger _clientLogger;

        public TestConsumer(IClientLogger clientLogger)
        {
            _clientLogger = clientLogger;
        }

        public void Consume(TestCommand message)
        {
            _clientLogger.Info("Test");
        }
    }
}