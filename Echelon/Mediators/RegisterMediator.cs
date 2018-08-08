using System;
using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Extensions.MassTransit;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.MassTransit.Commands.Register;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Settings;
using Echelon.Core.Logging.Loggers;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;
using Echelon.Misc.Enums;
using Echelon.Models.ViewModels;
using MassTransit;

namespace Echelon.Mediators
{
    public class RegisterMediator : IMediator
    {
        private readonly IBus _bus;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;
        private readonly IDataService _dataService;
        private readonly IClientLogger _logger;

        public RegisterMediator(IBus bus, ILoginService loginService, IMapper mapper, IDataService dataService,
            IClientLogger logger)
        {
            _logger = logger;
            _dataService = dataService;
            _bus = bus;
            _loginService = loginService;
            _mapper = mapper;
        }

        public async Task<bool> CompleteRegistration(string tempUserId)
        {
            try
            {
                var tempUserEntity = await _dataService.Load<TempUserEntity>(tempUserId);
                var avatarEntity = new AvatarEntity { FileType = FileTypeEnum.Png, AvatarUrl = "https://localhost/Echelon/Content/Images/missing-image.png" };
                var userEntity = _mapper.Map<UserEntity>(tempUserEntity, options => options.AfterMap((source, dest) => ((UserEntity) dest).AvatarId = avatarEntity.Id));

                await _dataService.Create(avatarEntity);
                await _dataService.Create(userEntity);
                await _dataService.Delete<TempUserEntity>(tempUserId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error completing customer {tempUserId} registration" + ex.Message);
            }

            return true;
        }

        public async Task<RegistrationEnum> Register(RegisterViewModel registerViewModel, string registerUrl)
        {
            await _bus.SendMessage(new LogInfoCommand
            {
                Content = $"Attempting to register new user with email: {registerViewModel.Email}"
            }, QueueSettings.General);

            try
            {
                var tempUserEntity = _mapper.Map<TempUserEntity>(registerViewModel);

                if (await _loginService.IsRegistered(tempUserEntity.Email))
                {
                    return RegistrationEnum.AlreadyRegistered;
                }

                await _bus.SendMessage(new RegisterNewUserCommand
                {
                    User = tempUserEntity,
                    RegisterUrl = $"{registerUrl}/{tempUserEntity.Id}"
                }, QueueSettings.Registration);

                return RegistrationEnum.Success;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ex.Message}: {registerViewModel.Email}");
                return RegistrationEnum.Failure;
            }
        }
    }
}