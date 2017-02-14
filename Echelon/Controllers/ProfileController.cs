using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Echelon.Core.Infrastructure.MassTransit.Commands;
using Echelon.Models.ViewModels;
using MassTransit;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ProfileController : Controller
    {
        private readonly IBus _bus;

        public ProfileController(IBus bus)
        {
            _bus = bus;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveDetails(ProfileViewModel profileViewModel)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0 && file.FileName != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(file.FileName));
                    await _bus.Publish(new LogInfoCommand { Content = $"Uploading : {file.FileName}" });
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Index");
        }
    }
}