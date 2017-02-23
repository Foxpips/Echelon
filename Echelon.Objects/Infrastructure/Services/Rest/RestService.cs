using System;
using System.Net;
using Echelon.Core.Logging.Loggers;
using Echelon.Misc.Translation;
using Newtonsoft.Json;

namespace Echelon.Core.Infrastructure.Services.Rest
{
    public class RestService : IRestService
    {
        private readonly IClientLogger _clientLoDgger;

        public RestService(IClientLogger clientLoDgger)

        {
            _clientLoDgger = clientLoDgger;
        }

        public TType MakeGenericRequest<TType>(Uri requestUri)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var json = webClient.DownloadString(requestUri);
                    var result = JsonConvert.DeserializeObject<TType>(json);
                    return result;
                }
            }
            catch (WebException ex)
            {
                _clientLoDgger.Error($"Error connecting to {requestUri} " + ex.Message);
                throw;
            }
        }

        public TranslatedChatModel MakeTranslationRequest(string textToTranslate, LanguageEnum selectedLanguage)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var translationKey =
                        "trnsl.1.1.20170207T182449Z.f2e3fa4def47a6cd.1181df127be1ae95c33e20868b61e1f44a73d1af";
                    var translationApiUri = "https://translate.yandex.net/api/v1.5/tr.json/translate";

                    client.QueryString.Add("key", translationKey);
                    client.QueryString.Add("text", textToTranslate);
                    client.QueryString.Add("lang", selectedLanguage.ToString());
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.UploadString(new Uri(translationApiUri), "Post");

                    _clientLoDgger.Info(response);
                    return JsonConvert.DeserializeObject<TranslatedChatModel>(response);
                }
            }
            catch (CookieException ex)
            {
                _clientLoDgger.Error(ex.Message);
                throw;
            }
        }
    }
}