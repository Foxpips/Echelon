using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities.Users;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Echelon.Infrastructure.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IDataService _dataservice;

        public LoginService(IDataService dataservice)
        {
            _dataservice = dataservice;
        }

        public async Task<bool> CheckUserExists(LoginEntity loginEntity)
        {
            var usersEntity = await _dataservice.Read<UsersEntity>();
            return usersEntity.Users.Any(
                    user =>
                        user.Email.Equals(loginEntity.Email) && user.Password == loginEntity.Password);
        }

        public async Task<bool> LogUserIn(LoginEntity loginEntity, IAuthenticationManager authenticationManager)
        {
            if (await CheckUserExists(loginEntity))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginEntity.Email),
                    new Claim(ClaimTypes.Name, loginEntity.UserName ?? loginEntity.Email)
                };

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Email,
                    ClaimTypes.Role);

                LogUserOut(authenticationManager);

                authenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = loginEntity.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    AllowRefresh = true
                }, identity);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identity);

                return true;
            }
            return false;
        }

        private static void LogUserOut(IAuthenticationManager authenticationManager)
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<bool> CreateAndLoguserIn(LoginEntity loginEntity, IAuthenticationManager authenticationManager)
        {
            await _dataservice.Update<UsersEntity>(usersEntity =>
            {
                if (!usersEntity.Users.Any(user => user.Email.Equals(loginEntity.Email)))
                {
                    usersEntity.Users.Add(loginEntity);
                }
            });

            return await LogUserIn(loginEntity, authenticationManager);
        }
    }
}