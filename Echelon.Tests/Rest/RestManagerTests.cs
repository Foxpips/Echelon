using System;
using System.Linq;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Core.Logging.Loggers;
using Echelon.Misc.Translation;
using NUnit.Framework;

namespace Echelon.Tests.Rest
{
    public class RestManagerTests
    {
        [Test]
        public void Method_Scenario_Result()
        {
            var restManager = new RestService(new ClientLogger());
            var translatedChatModel = restManager.MakeTranslationRequest("¡No puedo esperar hasta que termine el trabajo!", LanguageEnum.En);

            Console.WriteLine(translatedChatModel.Text.First());
        }
    }
}