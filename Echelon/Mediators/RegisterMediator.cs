using System;
using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.Exceptions;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.MassTransit.Commands.Register;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;
using MassTransit;
using static System.Configuration.ConfigurationManager;

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
                var sendEndpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/echelon_queue"));
                await sendEndpoint.Send(new LogInfoCommand
                {
                    Content = $"Attempting to register new user with email: {registerViewModel.Email}"
                });

                try
                {
                    var id = Guid.NewGuid().ToString();
                    var tempUserEntity = _mapper.Map<TempUserEntity>(registerViewModel);
                    tempUserEntity.Id = id;

                    await _loginService.CreateTempUser(tempUserEntity);
                    await sendEndpoint.Send(new RegisterNewUserCommand
                    {
                        RegisterUrl = $"{registerUrl}/{id}",
                        Email = tempUserEntity.Email
                    });
                }
                catch (UserAlreadyExistsException ex)
                {
                    await sendEndpoint.Send(new LogInfoCommand
                    {
                        Content = $"{ex.Message}: {registerViewModel.Email}"
                    });

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