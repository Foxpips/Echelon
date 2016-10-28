using System.Web.Mvc;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Login");
        }

        [Authorize]
        public ActionResult Secure()
        {
            ViewBag.Message = "Secure page.";
            return View();
        }
    }
}