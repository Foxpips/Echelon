using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
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
            var file = profileViewModel.File;
            var email = Request.GetOwinContext().Authentication.User.Identity.Name;

            await UploadAvatar(file, email);
            await UpdateProfile(profileViewModel, email);

            return View();
        }

        private async Task UpdateProfile(ProfileViewModel profileViewModel, string email)
        {
            await _dataService.Update<UserEntity>(user =>
            {
                user.UserNameEnabled = profileViewModel.UserNameEnabled;
                user.UserName = profileViewModel.UserName;
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
                        var uploadedFileDestination = $"{Guid.NewGuid()}-{file.FileName}";
                        var filePath = Path.Combine(Server.MapPath("~/UserAvatars/"), Path.GetFileName(uploadedFileDestination));
                        file.SaveAs(filePath);

                        var avatarUrl = SiteSettings.AvatarImagesPath + uploadedFileDestination;
                        var userEntities = await _dataService.Query<UserEntity>(entities => entities.Where(x => x.Email.Equals(email)));
                        await _dataService.Update<AvatarEntity>(x => x.AvatarUrl = avatarUrl, userEntities.SingleOrDefault()?.AvatarId);
                    }
                }
            }
        }
    }
}