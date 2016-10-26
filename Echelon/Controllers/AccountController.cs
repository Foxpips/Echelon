using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            var challengeResult = new ChallengeResult("Google",
                Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
            return challengeResult;
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            return new RedirectResult(returnUrl);
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