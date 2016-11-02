using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Echelon.Entities.Users;
using Echelon.Infrastructure.Services.Login;
using Microsoft.Owin.Security;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class AccountController : Controller
    {
        private ILoginService _loginService;

        public AccountController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public ActionResult Login(string returnUrl)
        {
            var challengeResult = new ChallengeResult("Google",
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
            return challengeResult;
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
//            var externalLoginInfo =  HttpContext.GetOwinContext().Authentication.GetExternalIdentity("ExternalCookie");
            var externalLoginInfoAsync = await HttpContext.GetOwinContext().Authentication.GetExternalLoginInfoAsync();

            var loginEntity = new LoginEntity {Email = externalLoginInfoAsync.Email};

            if (await _loginService.LogUserIn(loginEntity))
            {
                return new RedirectResult(Url.Action("Index", "Home"));
            }

            ModelState.AddModelError("", @"Login Failed!");
            return new RedirectResult(Url.Action("Index", "Login"));
        }

        // Implementation copied from a standard MVC Project, with some stuff
        // that relates to linking a new external login to an existing identity
        // account removed.
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            private string LoginProvider { get; }
            private string RedirectUri { get; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}