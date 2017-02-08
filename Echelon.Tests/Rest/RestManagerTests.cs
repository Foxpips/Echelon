using System;
using System.Linq;
using Echelon.Core.Infrastructure.Rest;
using Echelon.Core.Logging.Loggers;
using NUnit.Framework;

namespace Echelon.Tests.Rest
{
    public class RestManagerTests
    {
        [Test]
        public void Method_Scenario_Result()
        {
            var restManager = new RestManager(new ClientLogger());
            var translatedChatModel = restManager.MakeRequest("¡No puedo esperar hasta que termine el trabajo!", LanguageEnum.En);

            Console.WriteLine(translatedChatModel.Text.First());
        }
    }
}