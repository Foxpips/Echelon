using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.Settings;
using Echelon.Models.BusinessModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Echelon.Mediators
{
    public class ExternalLoginMediator : IMediator
    {
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;
        private readonly IRestService _restService;
        private readonly IMapper _mapper;

        public ExternalLoginMediator(ILoginService loginService, IOwinContext owinContext, IRestService restService,
            IMapper mapper)
        {
            _mapper = mapper;
            _restService = restService;
            _owinContext = owinContext;
            _loginService = loginService;
        }

        internal ChallengeResult GoogleChallengeResult(string redirectUri)
        {
            return new ChallengeResult(SiteSettings.GoogleProvider, redirectUri, _owinContext);
        }

        internal ChallengeResult FacebookChallengeResult(string redirectUri)
        {
            return new ChallengeResult(SiteSettings.FacebokProvider, redirectUri, _owinContext);
        }

        public async Task<bool> ExternalLoginSuccess(string returnUrl, string provider)
        {
            var externalLoginInfoAsync = await _owinContext.Authentication.GetExternalLoginInfoAsync();
            var userEntity = _mapper.Map<UserEntity>(externalLoginInfoAsync);
            var avatarUrl = await SetAvatar(externalLoginInfoAsync, provider);

            if (await _loginService.LogUserIn(userEntity, _owinContext.Authentication))
            {
                return true;
            }

            await _loginService.CreateUser(userEntity, avatarUrl);
            return await _loginService.LogUserIn(userEntity, _owinContext.Authentication);
        }

        private async Task<string> SetAvatar(ExternalLoginInfo externalLoginInfo, string provider)
        {
            switch (provider)
            {
                case SiteSettings.FacebokProvider:
                    return SiteSettings.FacebookProfileUri.Replace("{userId}", externalLoginInfo.ExternalIdentity.GetUserId());
                case SiteSettings.GoogleProvider:
                    var requestUri =
                        new Uri(SiteSettings.GoogleProfileUri + externalLoginInfo.ExternalIdentity.Claims
                            .Where(c => c.Type.Equals(SiteSettings.GoogleAccessToken))
                            .Select(c => c.Value)
                            .FirstOrDefault());

                    return (await _restService.MakeGenericRequest<GooglePlusInfo>(requestUri))?.Picture;
                default: return string.Empty;
            }
        }
    }
}