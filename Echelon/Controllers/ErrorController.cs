using System;
using System.Web.Mvc;

namespace Echelon.Controllers
{
    //[RequireHttps]
    public class ErrorController : BaseController
    {
        public ActionResult Index(Guid? errorId)
        {
            ViewBag.ErrorMessage = $"Oops something went wrong! <br/> ErrorId: {errorId}";
            return View();
        }

        public ActionResult NotFound()
        {
            ViewBag.ErrorMessage = "Oops that page does not exist!";
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