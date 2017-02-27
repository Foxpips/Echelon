using System.Web.Mvc;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ChatController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Login", "Login");
        }
    }
}