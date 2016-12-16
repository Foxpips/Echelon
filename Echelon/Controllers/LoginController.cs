using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Entities.Users;
using Echelon.Core.Infrastructure.MassTransit.Commands;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Logging.Interfaces;
using Echelon.Models.ViewModels;
using MassTransit;
using Microsoft.Owin;

namespace Echelon.Controllers
{
    public class LoginController : Controller
    {
        private readonly IClientLogger _clientLog;
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;
        private readonly IBus _bus;

        public LoginController(IClientLogger clientLog, ILoginService loginService, IOwinContext owinContext, IBus bus)
        {
            _bus = bus;
            _owinContext = owinContext;
            _loginService = loginService;
            _clientLog = clientLog;
        }

        [HttpGet]
        public ActionResult Help()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
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
            await _bus.Publish(new LogInfoCommand { Content = $"Attempting to login with email: {loginViewModel.Email}" });

            var loginEntity = Mapper.Map<UserEntity>(loginViewModel);
            if (ModelState.IsValid)
            {
                if (await _loginService.LogUserIn(loginEntity, _owinContext.Authentication))
                {
                    return RedirectToActionPermanent("Index", "Chat");
                }

                ModelState.AddModelError("", @"Email or Password is incorrect!");
            }

            await _bus.Publish(new LogInfoCommand { Content = $"User not found: {loginViewModel.Email}" });
            return View(loginViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            if (await _loginService.LogUserOut(_owinContext.Authentication))
            {
                await _bus.Publish(new LogInfoCommand { Content = $"Logging user: {_owinContext.Authentication.User.Identity.Name} out" });

                return RedirectToActionPermanent("Login", "Login");
            }
            return RedirectToAction("Account", "Error");
        }
    }
}