using System.Web.Mvc;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class NavigationController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}