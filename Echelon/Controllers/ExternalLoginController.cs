using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Entities.Users;
using Echelon.Infrastructure.Services.Login;
using Echelon.Models.BusinessModels;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ExternalLoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;

        public ExternalLoginController(ILoginService loginService, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _loginService = loginService;
        }

        public ActionResult LoginGoogle(string returnUrl)
        {
            var redirectUri = Url.Action("ExternalLoginCallback", "ExternalLogin", new {ReturnUrl = returnUrl});
            var challengeResult = new ChallengeResult("Google", redirectUri, _owinContext);

            return challengeResult;
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var externalLoginInfoAsync = await _owinContext.Authentication.GetExternalLoginInfoAsync();
            var loginEntity = Mapper.Map<UserEntity>(externalLoginInfoAsync);

            if (await _loginService.LogUserIn(loginEntity, _owinContext.Authentication) ||
                await _loginService.CreateAndLoguserIn(loginEntity, _owinContext.Authentication))
            {
                return new RedirectResult(Url.Action("Index", "Chat"));
            }

            ModelState.AddModelError("", @"Login Failed!");
            return new RedirectResult(Url.Action("Index", "Login"));
        }
    }
}