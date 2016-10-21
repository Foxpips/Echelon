using System.Web.Mvc;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}