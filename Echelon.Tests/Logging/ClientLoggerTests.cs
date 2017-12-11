using Echelon.Core.Logging.Loggers;
using NUnit.Framework;

namespace Echelon.Tests.Logging
{
    public class ClientLoggerTests
    {
        private readonly ClientLogger _clientLogger;

        public ClientLoggerTests()
        {
            _clientLogger = new ClientLogger();
        }

        [Test]
        public void Test_Client_LogSuccess()
        {
            _clientLogger.Info("test TESTSETSETSETSETSETSETSETSE");
        }
    }
}