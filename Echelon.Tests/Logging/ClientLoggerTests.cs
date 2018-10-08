using System;
using AutoMapper;
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

        [Test]
        public void c_Scenario_Result()
        {
            try { throw new AutoMapperMappingException("{asdjiasd} error  trying t {with}"); }
            catch (Exception ex)
            {
                _clientLogger.Error(ex.ToString());
            }
        }
    }
}