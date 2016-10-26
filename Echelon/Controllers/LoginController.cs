using System.Web.Mvc;
using Echelon.Core.Logging.Loggers;
using Echelon.Models.ViewModels;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(LoginViewModel loginViewModel)
        {
            var clientLogger = new ClientLogger();
            clientLogger.Info(loginViewModel.Email);
            clientLogger.Info(loginViewModel.Password);

            return View();
        }
    }
}