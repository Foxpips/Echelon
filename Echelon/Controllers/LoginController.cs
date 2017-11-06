using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Core.Logging.Loggers;
using Echelon.Mediators;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class LoginController : BaseController
    {
        private readonly LoginMediator _loginMediator;

        public LoginController(LoginMediator loginMediator)
        {
            _loginMediator = loginMediator;
        }

        [HttpGet]
        public ActionResult Help()
        {
            throw new StackOverflowException("Test overflow message!");
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Navigation");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid && await _loginMediator.Login(loginViewModel))
            {
                return RedirectToActionPermanent("Index", "Chat");
            }

            ModelState.AddModelError("", @"Email or Password is incorrect!");
            return View(loginViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            return await _loginMediator.Logout()
                ? RedirectToActionPermanent("Index", "Login")
                : RedirectToAction("Account", "Error");
        }
    }

    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            var clientLogger = DependencyResolver.Current.GetService<IClientLogger>();

            var newGuid = Guid.NewGuid();
            clientLogger.Error($"Error Guid: {newGuid}");
            clientLogger.Error(filterContext.Exception.Message);
            clientLogger.Error(filterContext.Exception.StackTrace);
            clientLogger.Error(filterContext.Exception.InnerException?.Message);

            filterContext.ExceptionHandled = true;

            // Redirect on error:
            filterContext.Result = RedirectToAction("Index", "Error", new {errorId = newGuid });
        }
    }
}