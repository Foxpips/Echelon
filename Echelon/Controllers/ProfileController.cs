using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Infrastructure.MassTransit.Commands.File;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Transforms;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.Settings;
using Echelon.Models.ViewModels;
using MassTransit;

namespace Echelon.Controllers
{
    [RequireHttps]
    public class ProfileController : Controller
    {
        private readonly IBus _bus;
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public ProfileController(IBus bus, IDataService dataService, IMapper mapper)
        {
            _mapper = mapper;
            _dataService = dataService;
            _bus = bus;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var email = Request.GetOwinContext().Authentication.User.Identity.Name;
            var userEntity = await _dataService.Single<UserEntity>(entities => entities.Where(user => user.Email.Equals(email)));

            return View(_mapper.Map<ProfileViewModel>(userEntity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ProfileViewModel profileViewModel)
        {
            var email = Request.GetOwinContext().Authentication.User.Identity.Name;
            await UpdateProfile(profileViewModel, email);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadAvatar(ProfileViewModel profileViewModel)
        {
            var file = profileViewModel.File;
            var email = Request.GetOwinContext().Authentication.User.Identity.Name;

            await UploadAvatar(file, email);

            return RedirectToAction("Index");
        }

        private async Task UpdateProfile(ProfileViewModel profileViewModel, string email)
        {
            await _dataService.Update<UserEntity>(user =>
            {
                user.DisplayName = profileViewModel.DisplayName;
                user.FirstName = profileViewModel.FirstName;
                user.LastName = profileViewModel.LastName;
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
                        var user = await _dataService.TransformUserAvatars<UserAvatarEntity>(email);

                        var avatarFileName = $"{Guid.NewGuid()}-{DateTime.Now.Millisecond}.png";
                        var avatarUrl = SiteSettings.AvatarImagesPath + avatarFileName;

                        if (user.AvatarUrl.Contains(SiteSettings.AvatarImagesPath))
                        {
                            avatarFileName = Path.GetFileName(user.AvatarUrl);
                            avatarUrl = user.AvatarUrl;
                        }

                        var avatarFilePath = Path.Combine(Server.MapPath("~/UserAvatars/"), avatarFileName);
                        file.SaveAs(avatarFilePath);

                        var userEntity = await _dataService.Single<UserEntity>(entities => entities.Where(x => x.Email.Equals(email)));

//                        await _bus.Publish(new DeleteFileCommand { FilePath = Path.Combine(Server.MapPath("~/UserAvatars/"), Path.GetFileName(user.AvatarUrl)) });
                        await _dataService.Update<AvatarEntity>(x => x.AvatarUrl = avatarUrl, userEntity.AvatarId);
                    }
                }
            }
        }
    }
}