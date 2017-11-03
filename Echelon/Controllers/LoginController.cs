using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Mediators;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class LoginController : Controller
    {
        private readonly LoginMediator _loginMediator;

        public LoginController(LoginMediator loginMediator)
        {
            _loginMediator = loginMediator;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            // Redirect on error:
            filterContext.Result = RedirectToAction("Index", "Error");

            //// OR set the result without redirection:
            //filterContext.Result = new ViewResult
            //{
            //    ViewName = "~/Views/Error/Account.cshtml"
            //};
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
}