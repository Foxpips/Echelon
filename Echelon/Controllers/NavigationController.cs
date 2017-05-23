using System.Web.Mvc;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class NavigationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}