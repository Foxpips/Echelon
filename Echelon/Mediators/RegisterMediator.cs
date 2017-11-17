using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.Exceptions;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;
using MassTransit;
using Microsoft.Owin;

namespace Echelon.Mediators
{
    public class RegisterMediator : IMediator
    {
        private readonly IBus _bus;
        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;
        private readonly IMapper _mapper;

        public RegisterMediator(IBus bus, ILoginService loginService, IOwinContext owinContext, IMapper mapper)
        {
            _bus = bus;
            _loginService = loginService;
            _owinContext = owinContext;
            _mapper = mapper;
        }

        public async Task<bool> Register(RegisterViewModel registerViewModel)
        {
            await _bus.Publish(new LogInfoCommand
            {
                Content = $"Attempting to register new user with email: {registerViewModel.Email}"
            });

            try
            {
                await
                    _loginService.CreateAndLoguserIn(_mapper.Map<UserEntity>(registerViewModel), string.Empty,
                        _owinContext.Authentication);
            }
            catch (UserAlreadyExistsException ex)
            {
                await _bus.Publish(new LogInfoCommand
                {
                    Content = $"{ex.Message}: {registerViewModel.Email}"
                });

                return false;
            }

            return true;
        }
    }
}