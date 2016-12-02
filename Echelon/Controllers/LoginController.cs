using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Logging.Interfaces;
using Echelon.Entities.Users;
using Echelon.Infrastructure.Services.Login;
using Echelon.Models.ViewModels;
using Microsoft.Owin;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class LoginController : Controller
    {
        private readonly IClientLogger _clientLog;
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;

        public LoginController(IClientLogger clientLog, ILoginService loginService, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _loginService = loginService;
            _clientLog = clientLog;
        }

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
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            _clientLog.Info($"Attempting to login email: {loginViewModel.Email}");

            var loginEntity = Mapper.Map<UserEntity>(loginViewModel);
            if (ModelState.IsValid)
            {
                if (await _loginService.LogUserIn(loginEntity, _owinContext.Authentication))
                {
                    return RedirectToActionPermanent("Index", "Chat");
                }

                ModelState.AddModelError("", @"Email or Password is incorrect!");
            }

            _clientLog.Info($"User not found! {loginViewModel.Email}");
            return View("Index", loginViewModel);
        }
    }
}