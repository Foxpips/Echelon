using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Entities.Users;
using Echelon.Core.Infrastructure.MassTransit.Commands;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Logging.Loggers;
using Echelon.Models.ViewModels;
using MassTransit;
using Microsoft.Owin;

namespace Echelon.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;
        private readonly IBus _bus;
        private IMapper _mapper;

        public LoginController(ILoginService loginService, IOwinContext owinContext, IBus bus, IMapper mapper)
        {
            _mapper = mapper;
            _bus = bus;
            _owinContext = owinContext;
            _loginService = loginService;
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
            await _bus.Publish(new LogInfoCommand { Content = $"Attempting to login with email: {loginViewModel.Email}" });

            var loginEntity = _mapper.Map<UserEntity>(loginViewModel);
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

                return RedirectToActionPermanent("Index", "Login");
            }
            return RedirectToAction("Account", "Error");
        }
    }
}