using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities.Users;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Echelon.Infrastructure.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IDataService _dataservice;

        private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        public LoginService(IDataService dataservice)
        {
            _dataservice = dataservice;
        }

        public async Task<bool> CheckUserExists(LoginEntity loginEntity)
        {
            var usersEntity = await _dataservice.Read<UsersEntity>();
            return usersEntity.Users.Any(user => user.Email.Equals(loginEntity.Email));
        }

        public async Task<bool> LogUserIn(LoginEntity loginEntity)
        {
            if (await CheckUserExists(loginEntity))
            {
                var identity = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Email, loginEntity.Email) },
                    DefaultAuthenticationTypes.ApplicationCookie,
                    ClaimTypes.Email, ClaimTypes.Role);

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = loginEntity.RememberMe,
                }, identity);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identity);

                return true;
            }
            return false;
        }
    }
}