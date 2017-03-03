using System;
using System.Threading.Tasks;
using Echelon.Misc.Translation;

namespace Echelon.Core.Infrastructure.Services.Rest
{
    public interface IRestService :IService
    {
        Task<TranslatedChatModel> MakeTranslationRequest(string textToTranslate, LanguageEnum selectedLanguage);
        Task<TType> MakeGenericRequest<TType>(Uri requestUri);
    }
}