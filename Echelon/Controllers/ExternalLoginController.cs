using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Services.Rest;
using Echelon.Data.Entities.Users;
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
        private readonly IMapper _mapper;

        public ExternalLoginController(ILoginService loginService, IOwinContext owinContext, IRestService restService,
            IMapper mapper)
        {
            _mapper = mapper;
            _restService = restService;
            _owinContext = owinContext;
            _loginService = loginService;
        }

        public ActionResult Index()
        {
            return View();
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
            var userEntity = _mapper.Map<UserEntity>(externalLoginInfoAsync);
            var avatarUrl = await SetGoogleAvatar(externalLoginInfoAsync);

            if (await _loginService.LogUserIn(userEntity, _owinContext.Authentication) ||
                await _loginService.CreateAndLoguserIn(userEntity, avatarUrl, _owinContext.Authentication))
            {
                return RedirectToAction("Index", "Chat");
            }

            ModelState.AddModelError("", @"Login Failed!");
            return RedirectToAction("Index", "Login");
        }

        private async Task<string> SetGoogleAvatar(ExternalLoginInfo externalLoginInfoAsync)
        {
            var requestUri =
                new Uri(SiteSettings.GoogleProfileUri + externalLoginInfoAsync.ExternalIdentity.Claims.Where(
                    c => c.Type.Equals(SiteSettings.GoogleAccessToken))
                    .Select(c => c.Value)
                    .FirstOrDefault());

            return (await _restService.MakeGenericRequest<GooglePlusInfo>(requestUri))?.Picture;
        }
    }
}