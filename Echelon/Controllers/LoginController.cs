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

        [HttpGet]
        public ActionResult Help()
        {
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
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            return await _loginMediator.Logout()
                ? RedirectToActionPermanent("Index", "Login")
                : RedirectToAction("Account", "Error");
        }
    }
}