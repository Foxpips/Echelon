using System.Web.Mvc;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Secure()
        {
            ViewBag.Message = "Secure page.";
            return View();
        }
    }
}