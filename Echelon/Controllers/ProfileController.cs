using System;
using System.IO;
using System.Linq;
using System.Net;
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

        [System.Web.Http.HttpGet]
        [System.Web.Http.Authorize]
        public async Task<ActionResult> GetAvatarImage(string email)
        {
            var avatarEntities = await _dataService.Read<AvatarEntity>();
            var avatarFile = avatarEntities.Single(x => x.Email.Equals("simonpmarkey@gmail.com"));
            var fileContentResult = File(avatarFile.ImageBytes, "image/png", "avatar.png");
            //            return fileContentResult;

            using (var webClient = new WebClient())
            {
                var foo = await webClient.DownloadDataTaskAsync(new Uri("https://lh5.googleusercontent.com/-h85Scfoq274/AAAAAAAAAAI/AAAAAAAAABQ/rFY9Ow0Nn4M/photo.jpg"));
                fileContentResult = File(foo, "image/jpg", "avatar.jpg");
            }
            return fileContentResult;
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
                        //file.SaveAs(Path.Combine(Server.MapPath("~/Images/"), Path.GetFileName(file.FileName)));

                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            var fileData = binaryReader.ReadBytes(file.ContentLength);
                            await
                                _dataService.Create(new AvatarEntity
                                {
                                    Email = email,
                                    ImageName = file.FileName,
                                    FileType = file.ContentType,
                                    ImageBytes = fileData
                                });
                        }
                    }
                }
            }
        }
    }
}