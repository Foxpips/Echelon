using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Core.Infrastructure.Exceptions;
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
            if (ModelState.IsValid)
            {
                var registrationResult = await _registerMediator.Register(registerViewModel, Url.Action("RegistrationSuccess", "Register", null, Request.Url?.Scheme));

                switch (registrationResult)
                {
                    case RegistrationEnum.Success:
                        return RedirectToAction("ConfirmRegistration");
                    case RegistrationEnum.AlreadyRegistered:
                        ModelState.AddModelError(string.Empty, Login.AccountAlreadyExists);
                        break;
                    case RegistrationEnum.Failure:
                        ModelState.AddModelError(string.Empty, Login.AccountCreationError);
                        break;
                    default:
                        throw new RegisterAccountException(string.Empty);
                }
            }

            ModelState.AddModelError("", @"Apologies something went wrong, please try again later!");
            return View(registerViewModel);
        }

        [HttpGet]
        public ActionResult ConfirmRegistration()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> RegistrationSuccess(string id)
        {
            if (await _registerMediator.CompleteRegistration(id))
            {
                return View();
            }

            return RedirectToAction("Index", "Error", new { errorId = id });
        }
    }
}