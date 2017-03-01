using System;
using System.Net;
using System.Threading.Tasks;
using Echelon.Core.Logging.Loggers;
using Echelon.Misc.Translation;
using Newtonsoft.Json;

namespace Echelon.Core.Infrastructure.Services.Rest
{
    public class RestService : IRestService
    {
        private readonly IClientLogger _clientLogger;

        public RestService(IClientLogger clientLogger)

        {
            _clientLogger = clientLogger;
        }

        public async Task<TType> MakeGenericRequest<TType>(Uri requestUri)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var json = webClient.DownloadStringTaskAsync(requestUri);
                    var result = JsonConvert.DeserializeObject<TType>(await json);
                    return result;
                }
            }
            catch (WebException ex)
            {
                _clientLogger.Error($"Error connecting to {requestUri} " + ex.Message);
                throw;
            }
        }

        public async Task<TranslatedChatModel> MakeTranslationRequest(string textToTranslate,
            LanguageEnum selectedLanguage)
        {
            try
            {
                using (var client = new WebClient())
                {
                    const string translationKey =
                        "trnsl.1.1.20170207T182449Z.f2e3fa4def47a6cd.1181df127be1ae95c33e20868b61e1f44a73d1af";
                    const string translationApiUri = "https://translate.yandex.net/api/v1.5/tr.json/translate";

                    client.QueryString.Add("key", translationKey);
                    client.QueryString.Add("text", textToTranslate);
                    client.QueryString.Add("lang", selectedLanguage.ToString());
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.UploadStringTaskAsync(new Uri(translationApiUri), "Post");

                    return JsonConvert.DeserializeObject<TranslatedChatModel>(await response);
                }
            }
            catch (CookieException ex)
            {
                _clientLogger.Error(ex.Message);
                throw;
            }
        }
    }
}