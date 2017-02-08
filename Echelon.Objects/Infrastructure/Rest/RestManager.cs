using System;
using System.Collections.Generic;
using System.Net;
using Echelon.Core.Logging.Loggers;
using Newtonsoft.Json;

namespace Echelon.Core.Infrastructure.Rest
{
    public class RestManager
    {
        private IClientLogger _clientLoDgger;

        public RestManager(IClientLogger clientLoDgger)
        {
            _clientLoDgger = clientLoDgger;
        }

        public TranslatedChatModel MakeRequest(string textToTranslate, LanguageEnum selectedLanguage)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.QueryString.Add("key", "trnsl.1.1.20170207T182449Z.f2e3fa4def47a6cd.1181df127be1ae95c33e20868b61e1f44a73d1af");
                    client.QueryString.Add("text", textToTranslate);
                    client.QueryString.Add("lang", selectedLanguage.ToString());
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var response = client.UploadString(new Uri("https://translate.yandex.net/api/v1.5/tr.json/translate"), "Post");

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

    public enum LanguageEnum
    {
        En,
        Es
    }
}
public class TranslatedChatModel
{
    public int Code { get; set; }
    public string Lang { get; set; }
    public List<string> Text { get; set; }
}
