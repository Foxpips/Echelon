using System;
using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.Exceptions;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.MassTransit.Commands.Register;
using Echelon.Core.Infrastructure.MassTransit.Extensions;
using Echelon.Core.Infrastructure.Services.Login;
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

        public RegisterMediator(IBus bus, ILoginService loginService, IMapper mapper)
        {
            _bus = bus;
            _loginService = loginService;
            _mapper = mapper;
        }

        public async Task<bool> Register(RegisterViewModel registerViewModel, string registerUrl)
        {
            try
            {
                await _bus.SendMessage(new LogInfoCommand
                {
                    Content = $"Attempting to register new user with email: {registerViewModel.Email}"
                }, QueueSettings.General);

                try
                {
                    var id = Guid.NewGuid().ToString();
                    var tempUserEntity = _mapper.Map<TempUserEntity>(registerViewModel);
                    tempUserEntity.Id = id;

                    await _loginService.CreateTempUser(tempUserEntity);
                    await _bus.SendMessage(new RegisterNewUserCommand
                    {
                        RegisterUrl = $"{registerUrl}/{id}",
                        Email = tempUserEntity.Email
                    }, QueueSettings.General);
                }
                catch (UserAlreadyExistsException ex)
                {
                    await _bus.SendMessage(new LogInfoCommand
                    {
                        Content = $"{ex.Message}: {registerViewModel.Email}"
                    }, QueueSettings.General);

                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return true;
        }
    }
}