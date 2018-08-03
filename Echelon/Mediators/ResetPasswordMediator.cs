using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.MassTransit.Extensions;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Settings;
using Echelon.Data;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.AutoMapper.Profiles;
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

        public async Task<ResetPasswordViewModel> GetUserToReset(string id)
        {
            var resetUserEntity = await _dataService.Load<ResetPasswordEntity>(id);
            if (resetUserEntity != null)
            {
                var userEntity = await _dataService.Load<UserEntity>(resetUserEntity.Email);
                return _mapper.Map<ResetPasswordViewModel>(userEntity);
            }
            return null;
        }

        public Task UpdateUser(object profileViewModel)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}