using System.Web.Mvc;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class NavigationController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}