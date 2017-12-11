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

        [Test]
        public async Task Call_Dailymotion_Api_Services()
        {
            var restManager = new RestService(new ClientLogger());
            var translatedChatModel =
                await
                    restManager.MakeGenericRequest<object>(
                        new Uri(
                            "http://www.dailymotion.com/services/oembed?url=http://www.dailymotion.com/video/x5epodb_horizon-zero-dawn-ravager-trial_videogames"));
            Console.WriteLine(translatedChatModel);
        }

        [Test]
        public async Task Call_Vimeo_Api_Services()
        {
            var restManager = new RestService(new ClientLogger());
            var translatedChatModel =
                await
                    restManager.MakeGenericRequest<object>(
                        new Uri("https://vimeo.com/api/oembed.json?url=https://vimeo.com/112483572"));
            Console.WriteLine(translatedChatModel);
        }
    }
}