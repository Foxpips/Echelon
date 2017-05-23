using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Mediators;

namespace Echelon.Controllers
{
    [Authorize]
    [RequireHttps]
    public class TokenController : Controller
    {
        private readonly TokenMediator _tokenMediator;


        public TokenController(TokenMediator tokenMediator)
        {
            _tokenMediator = tokenMediator;
        }

        [HttpPost]
        public async Task<ActionResult> Index(string device, string channel)
        {
            return Json(await _tokenMediator.CreateToken(device, channel),
                JsonRequestBehavior.AllowGet);
        }
    }
}