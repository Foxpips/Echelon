using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Transforms;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.Settings;
using Echelon.Models.ViewModels;
using MassTransit;

namespace Echelon.Mediators
{
    public class ProfileMediator : IMediator
    {
        private readonly IBus _bus;
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public ProfileMediator(IBus bus, IDataService dataService, IMapper mapper)
        {
            _mapper = mapper;
            _dataService = dataService;
            _bus = bus;
        }

        internal async Task<ProfileViewModel> GetUser(string email)
        {
            var userEntity = await _dataService.Single<UserEntity>(entities => entities.Where(user => user.Email.Equals(email)));
            return _mapper.Map<ProfileViewModel>(userEntity);
        }

        internal async Task UpdateUser(ProfileViewModel profileViewModel, string email)
        {
            await _dataService.Update<UserEntity>(user =>
            {
                user.DisplayName = profileViewModel.DisplayName;
                user.FirstName = profileViewModel.FirstName;
                user.LastName = profileViewModel.LastName;
            }, email);
        }

        internal async Task UploadUserAvatar(HttpPostedFileBase file, string email, string server)
        {
            if (file != null && file.ContentLength > 0 && file.FileName != null)
            {
                await _bus.Publish(new LogInfoCommand { Content = $"Uploading : {file.FileName}" });
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

                        var avatarFilePath = Path.Combine(server, avatarFileName);
                        file.SaveAs(avatarFilePath);

                        var userEntity = await _dataService.Single<UserEntity>(entities => entities.Where(x => x.Email.Equals(email)));
                        await _dataService.Update<AvatarEntity>(x => x.AvatarUrl = avatarUrl, userEntity.AvatarId);
                    }
            }
        }
    }
}