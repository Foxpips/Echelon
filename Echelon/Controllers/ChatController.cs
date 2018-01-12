using System.Web.Mvc;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ChatController : BaseController
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Login");
        }

        public ActionResult ChatHub()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Login");
        }
    }
}