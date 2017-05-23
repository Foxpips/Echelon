using System.Web.Mvc;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ErrorController : Controller
    {
        public ActionResult Account()
        {
            ViewBag.ErrorMessage = "Oops something went wrong!";
            return View();
        }
    }
}