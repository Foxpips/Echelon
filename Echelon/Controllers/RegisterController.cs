using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Mediators;
using Echelon.Models.ViewModels;
using Echelon.Resources;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class RegisterController : BaseController
    {
        private readonly RegisterMediator _registerMediator;

        public RegisterController(RegisterMediator registerMediator)
        {
            _registerMediator = registerMediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid && await _registerMediator.Register(registerViewModel))
            {
                return RedirectToActionPermanent("Index", "Profile");
            }

            ModelState.AddModelError(string.Empty, Login.AccountCreationError);
            return View(registerViewModel);
        }
    }
}