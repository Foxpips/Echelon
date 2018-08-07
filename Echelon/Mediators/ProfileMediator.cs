using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Echelon.Core.Extensions.MassTransit;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.Settings;
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
            var userEntity = await _dataService.Load<UserEntity>(email);
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
                await _bus.SendMessage(new LogInfoCommand { Content = $"Uploading : {file.FileName}" }, QueueSettings.General);

                if (file.ContentLength > 0)
                {
                    var user = await _dataService.TransformUserAvatars<UserAvatarEntity>(email);
                    bool? hasExistingAvatar = user.AvatarUrl?.Contains(SiteSettings.AvatarImagesPath);

                    if (hasExistingAvatar == true)
                    {
                        file.SaveAs(Path.Combine(server, Path.GetFileName(user.AvatarUrl)));
                    }
                    else
                    {
                        await CreateNewAvatar(file, email, server);
                    }
                }
            }
        }

        private async Task CreateNewAvatar(HttpPostedFileBase file, string email, string server)
        {
            var avatarFileName = $"{Guid.NewGuid()}-{DateTime.Now.Millisecond}.png";
            var avatarUrl = SiteSettings.AvatarImagesPath + avatarFileName;

            file.SaveAs(Path.Combine(server, avatarFileName));

            var userEntity = await _dataService.Load<UserEntity>(email);
            var avatarEntity = new AvatarEntity { AvatarUrl = avatarUrl };

            await _dataService.Create(avatarEntity);
            await _dataService.Update<UserEntity>(currentUser => currentUser.AvatarId = avatarEntity.Id, userEntity.Id);
        }
    }
}