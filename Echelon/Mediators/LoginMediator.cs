using System.Threading.Tasks;
using AutoMapper;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Data.Entities.Users;
using Echelon.Models.ViewModels;
using MassTransit;
using Microsoft.Owin;

namespace Echelon.Mediators
{
    public class LoginMediator : IMediator
    {

        private readonly ILoginService _loginService;
        private readonly IOwinContext _owinContext;
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public LoginMediator(ILoginService loginService, IOwinContext owinContext, IBus bus, IMapper mapper)
        {
            _loginService = loginService;
            _owinContext = owinContext;
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<bool> Login(LoginViewModel loginViewModel)
        {
            await _bus.Publish(new LogInfoCommand { Content = $"Attempting to login with email: {loginViewModel.Email}" });
            if (await _loginService.LogUserIn(_mapper.Map<UserEntity>(loginViewModel), _owinContext.Authentication))
            {
                return true;
            }
            await _bus.Publish(new LogInfoCommand { Content = $"User not found: {loginViewModel.Email}" });
            return false;
        }

        public async Task<bool> Logout()
        {
            await _bus.Publish(new LogInfoCommand { Content = $"Logging user: {_owinContext.Authentication.User.Identity.Name} out" });
            if (await _loginService.LogUserOut(_owinContext.Authentication))
            {
                return true;
            }
            await _bus.Publish(new LogInfoCommand { Content = $"Error could not log user : {_owinContext.Authentication.User.Identity.Name} out" });
            return false;
        }
    }
}