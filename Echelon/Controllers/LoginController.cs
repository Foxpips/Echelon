using System;
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
            _clientLog.Info($"Attempting to login email: {loginViewModel.Email}");

            var loginEntity = Mapper.Map<LoginEntity>(loginViewModel);
            if (ModelState.IsValid)
            {
                if (await _loginService.LogUserIn(loginEntity))
                {
                    return RedirectToActionPermanent("Index","Home");
                }

                ModelState.AddModelError("", @"Email or Password is incorrect!");
            }

            _clientLog.Info($"User not found! {loginViewModel.Email}");
            return View("Index", loginViewModel);
        }
    }
}