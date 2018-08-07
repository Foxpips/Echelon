using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Extensions.MassTransit;
using Echelon.Core.Helpers;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.MassTransit.Commands.ResetPassword;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Settings;
using Echelon.Data;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.AutoMapper.Profiles;
using Echelon.Models.ViewModels;
using MassTransit;

namespace Echelon.Mediators
{
    public class ResetPasswordMediator : IMediator
    {
        private readonly IBus _bus;
        private readonly IDataService _dataService;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;

        public ResetPasswordMediator(IBus bus, IDataService dataService, ILoginService loginService, IMapper mapper)
        {
            _bus = bus;
            _dataService = dataService;
            _loginService = loginService;
            _mapper = mapper;
        }

        public async Task<bool> ResetPassword(string email, string resetLink)
        {
            var userExists = await _loginService.IsRegistered(email);
            if (userExists)
            {
                var resetUserEntity = new ResetPasswordEntity { Email = email };
                await _dataService.Create(resetUserEntity);

                await _bus.SendMessage(new ResetPasswordCommand
                {
                    Email = email,
                    ResetLink = $"{resetLink}/{resetUserEntity.Id}"
                }, QueueSettings.ResetPassword);
                return true;
            }
            return false;
        }

        public async Task<ResetPasswordViewModel> GetUserToResetById(string id)
        {
            var resetUserEntity = await _dataService.Load<ResetPasswordEntity>(id);
            if (resetUserEntity != null)
            {
                var userEntity = await _dataService.Load<UserEntity>(resetUserEntity.Email);
                return _mapper.Map<ResetPasswordViewModel>(userEntity);
            }
            return null;
        }

        public async Task UpdateUser(ResetPasswordViewModel resetViewModel)
        {
            var resetUserEntity = await _dataService.Load<ResetPasswordEntity>(resetViewModel.Id);
            await _dataService.Update<UserEntity>(x => x.Password = HashHelper.CreateHash(resetViewModel.Password), resetUserEntity.Email);
            await _dataService.Delete<ResetPasswordEntity>(resetViewModel.Id);
        }
    }
}