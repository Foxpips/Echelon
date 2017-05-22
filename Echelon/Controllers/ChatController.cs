﻿using System.Web.Mvc;

namespace Echelon.Controllers
{
    [Authorize]
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

            return RedirectToAction("Index", "Login");
        }
    }
}