using System;
using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.MassTransit.Commands.Register;
using Echelon.Core.Infrastructure.MassTransit.Extensions;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Infrastructure.Settings;
using Echelon.Core.Logging.Loggers;
using Echelon.Data;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.Settings;
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
                await _dataService.Create(_mapper.Map<UserEntity>(tempUserEntity));
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
                tempUserEntity.Id = Guid.NewGuid().ToString();

                if (await _loginService.IsRegistered(tempUserEntity.Email))
                {
                    return RegistrationEnum.AlreadyRegistered;
                }

                await _bus.SendMessage(new RegisterNewUserCommand
                {
                    User = tempUserEntity,
                    RegisterUrl = $"{registerUrl}/{tempUserEntity.Id}",
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