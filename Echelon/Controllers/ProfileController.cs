using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
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
            var email = Request.GetOwinContext().Authentication.User.Identity.Name;

            await UploadAvatar(file, email);
            await UpdateProfile(profileViewModel, email);

            return View();
        }

        private async Task UpdateProfile(ProfileViewModel profileViewModel, string email)
        {
            await _dataService.Update<UserEntity>(x =>
            {
                x.UserNameEnabled = profileViewModel.UserNameEnabled;
                x.UserName = profileViewModel.UserName;
                x.FirstName = profileViewModel.FirstName;
                x.LastName = profileViewModel.LastName;
            },
                email);
        }

        private async Task UploadAvatar(HttpPostedFileBase file, string email)
        {
            if (file != null && file.ContentLength > 0 && file.FileName != null)
            {
                await _bus.Publish(new LogInfoCommand { Content = $"Uploading : {file.FileName}" });

                if (ModelState.IsValid)
                {
                    if (file.ContentLength > 0)
                    {
                        var filePath = Path.Combine(Server.MapPath("~/Users/Profiles/Images/"),
                            Path.GetFileName($"{Guid.NewGuid()}-{file.FileName}"));
                        file.SaveAs(filePath);

                        await _dataService.Create(new AvatarEntity
                        {
                            Email = email,
                            ImageName = file.FileName,
                            FileType = file.ContentType,
                            AvatarUrl = filePath
                        });
                    }
                }
            }
        }
    }
}