﻿using System.Web.Mvc;

namespace Echelon.Controllers
{
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