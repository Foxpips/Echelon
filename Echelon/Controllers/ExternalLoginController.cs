using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Entities.Users;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Infrastructure.Settings;
using Echelon.Models.BusinessModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ExternalLoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;
        private readonly IRestService _restService;

        public ExternalLoginController(ILoginService loginService, IOwinContext owinContext, IRestService restService)
        {
            _restService = restService;
            _owinContext = owinContext;
            _loginService = loginService;
        }

        public ActionResult LoginGoogle(string returnUrl)
        {
            var redirectUri = Url.Action("ExternalLoginCallback", "ExternalLogin", new {ReturnUrl = returnUrl});
            var challengeResult = new ChallengeResult(SiteSettings.GoogleProvider, redirectUri, _owinContext);

            return challengeResult;
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var externalLoginInfoAsync = await _owinContext.Authentication.GetExternalLoginInfoAsync();
            var loginEntity = Mapper.Map<UserEntity>(externalLoginInfoAsync);
            SetGoogleAvatar(externalLoginInfoAsync, loginEntity);

            if (await _loginService.LogUserIn(loginEntity, _owinContext.Authentication) ||
                await _loginService.CreateAndLoguserIn(loginEntity, _owinContext.Authentication))
            {
                return RedirectToAction("Index", "Chat");
            }

            ModelState.AddModelError("", @"Login Failed!");
            return RedirectToAction("Login", "Login");
        }

        private void SetGoogleAvatar(ExternalLoginInfo externalLoginInfoAsync, UserEntity loginEntity)
        {
            var requestUri =
                new Uri(SiteSettings.GoogleProfileUri +
                        externalLoginInfoAsync.ExternalIdentity.Claims.Where(
                            c => c.Type.Equals(SiteSettings.GoogleAccessToken))
                            .Select(c => c.Value)
                            .FirstOrDefault());

            loginEntity.AvatarUrl = _restService.MakeGenericRequest<GooglePlusInfo>(requestUri).Picture;
        }
    }
}