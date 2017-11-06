using System;
using System.Web.Mvc;

namespace Echelon.Controllers
{
    //[RequireHttps]
    public class ErrorController : Controller
    {
        public ActionResult Index(Guid? errorId)
        {
            ViewBag.ErrorMessage = $"Oops something went wrong! ErrorId: {errorId}";
            return View();
        }

        [Authorize]
        public ActionResult Account()
        {
            ViewBag.ErrorMessage = "Oops something went wrong!";
            return View();
        }
    }
}