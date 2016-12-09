using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Logging.Interfaces;
using Echelon.Models.ViewModels;
using Echelon.Objects.Entities.Users;
using Echelon.Objects.Infrastructure.MassTransit.Commands;
using Echelon.Objects.Infrastructure.Services.Login;
using MassTransit;
using Microsoft.Owin;

namespace Echelon.Controllers
{
    [RequireHttps]
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
            await _bus.Publish(new LogInfoCommand {Content = $"Attempting to login with email: {loginViewModel.Email}"});

//            _clientLog.Info($"Attempting to login email: {loginViewModel.Email}");

            var loginEntity = Mapper.Map<UserEntity>(loginViewModel);
            if (ModelState.IsValid)
            {
                if (await _loginService.LogUserIn(loginEntity, _owinContext.Authentication))
                {
                    return RedirectToActionPermanent("Index", "Chat");
                }

                ModelState.AddModelError("", @"Email or Password is incorrect!");
            }

            await _bus.Publish(new LogInfoCommand {Content = $"User not found: {loginViewModel.Email}"});
//            _clientLog.Info($"User not found! {loginViewModel.Email}");
            return View(loginViewModel);
        }
    }
}