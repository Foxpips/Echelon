using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Echelon.Core.Logging.Interfaces;
using Echelon.Entities.Users;
using Echelon.Infrastructure.Services.Login;
using Echelon.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class LoginController : Controller
    {
        private readonly IClientLogger _clientLog;
        private readonly ILoginService _loginService;

        public LoginController(IClientLogger clientLog, ILoginService loginService)
        {
            _loginService = loginService;
            _clientLog = clientLog;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Signin(LoginViewModel loginViewModel)
        {
            var loginEntity = Mapper.Map<LoginEntity>(loginViewModel);

            _clientLog.Info($"Attempting to login email: {loginEntity.Email}");

            if (ModelState.IsValid)
            {
                if (await _loginService.CheckUserExists(loginEntity))
                {
                    var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, loginEntity.Email)},
                        DefaultAuthenticationTypes.ApplicationCookie,
                        ClaimTypes.Name, ClaimTypes.Role);

                    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = loginEntity.RememberMe
                    }, identity);

                    FormsAuthentication.SetAuthCookie(loginViewModel.Email, loginViewModel.RememberMe);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", @"Login data is incorrect!");
            }

            _clientLog.Info($"User not found! {loginViewModel.Email}");
            return View("Index", loginViewModel);
        }
    }
}