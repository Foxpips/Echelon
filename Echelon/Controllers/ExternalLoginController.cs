using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Infrastructure.Settings;
using Echelon.Mediators;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ExternalLoginController : BaseController
    {
        private readonly ExternalLoginMediator _externalLoginMediator;

        public ExternalLoginController(ExternalLoginMediator externalLoginMediator)
        {
            _externalLoginMediator = externalLoginMediator;
        }

        public ActionResult LoginGoogle(string returnUrl)
        {
            return
                _externalLoginMediator.GoogleChallengeResult(Url.Action("ExternalLoginCallback", "ExternalLogin",
                    new { ReturnUrl = returnUrl, Provider = SiteSettings.GoogleProvider }));
        }

        public ActionResult LoginFacebook(string returnUrl)
        {
            return
                _externalLoginMediator.FacebookChallengeResult(Url.Action("ExternalLoginCallback", "ExternalLogin",
                    new { ReturnUrl = returnUrl, Provider = SiteSettings.FacebokProvider }));
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string provider)
        {
            if (await _externalLoginMediator.ExternalLoginSuccess(returnUrl, provider))
            {
                return RedirectToAction("Index", "Chat");
            }

            ModelState.AddModelError("", @"Login Failed!");
            return RedirectToAction("Index", "Login");
        }
    }
}