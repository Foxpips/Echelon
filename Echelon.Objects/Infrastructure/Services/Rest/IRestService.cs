using System;
using Echelon.Misc.Translation;

namespace Echelon.Core.Infrastructure.Services.Rest
{
    public interface IRestService :IService
    {
        TranslatedChatModel MakeTranslationRequest(string textToTranslate, LanguageEnum selectedLanguage);
        TType MakeGenericRequest<TType>(Uri requestUri);
    }
}