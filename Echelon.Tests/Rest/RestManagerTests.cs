using System;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Core.Logging.Loggers;
using Echelon.Misc.Translation;
using NUnit.Framework;

namespace Echelon.Tests.Rest
{
    public class RestManagerTests
    {
        [Test]
        [Ignore("Yandex api has changed need read up on what url to use now")]
        public async Task Method_Scenario_Result()
        {
            var restManager = new RestService(new ClientLogger());
            var translatedChatModel =
                await
                    restManager.MakeTranslationRequest("¡No puedo esperar hasta que termine el trabajo!",
                        LanguageEnum.En);

            Console.WriteLine(translatedChatModel.Text.First());
        }
    }
}