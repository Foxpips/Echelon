using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Mediators;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ExternalLoginController : Controller
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
                    new {ReturnUrl = returnUrl}));
        }

        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
          if(await _externalLoginMediator.ExternalLoginSuccess(returnUrl)) 
            {
                return RedirectToAction("Index", "Chat");
            }

            ModelState.AddModelError("", @"Login Failed!");
            return RedirectToAction("Index", "Login");
        }
    }
}