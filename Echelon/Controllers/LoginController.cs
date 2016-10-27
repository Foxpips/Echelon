using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Logging.Interfaces;
using Echelon.Entities;
using Echelon.Entities.Users;
using Echelon.Infrastructure.Services.Login;
using Echelon.Models.ViewModels;

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

            if (await _loginService.CheckUserExists(loginEntity))
            {
                _loginService.LogUserIn();

                return RedirectToAction("Index", "Home");
            }

            _clientLog.Info($"User not found! {loginViewModel.Email}");
            return View("Index");
        }
    }
}