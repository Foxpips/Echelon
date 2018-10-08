using System.Web.Mvc;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ChatController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}