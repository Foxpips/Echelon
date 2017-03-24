using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Data;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;
using MassTransit;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ProfileController : Controller
    {
        private readonly IBus _bus;
        private readonly IDataService _dataService;

        public ProfileController(IBus bus, IDataService dataService)
        {
            _dataService = dataService;
            _bus = bus;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ProfileViewModel profileViewModel)
        {
            var file = profileViewModel.File;
            if (file != null && file.ContentLength > 0 && file.FileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(file.FileName));
                await _bus.Publish(new LogInfoCommand { Content = $"Uploading : {file.FileName}" });

                if (ModelState.IsValid)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        file.SaveAs(Path.Combine(path, fileName));
                    }
                }

                ModelState.AddModelError("photo", @"Invalid type. Only the following types (jpg, jpeg, png) are supported.");
            }

            await _dataService.Update<UserEntity>(x =>
            {
                x.UserNameEnabled = profileViewModel.UserNameEnabled;
                x.UserName = profileViewModel.UserName;
                x.FirstName = profileViewModel.UserName;
                x.LastName = profileViewModel.LastName;
            },
                Request.GetOwinContext().Authentication.User.Identity.Name);

            return View();
        }
    }
}