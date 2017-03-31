using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Echelon.Core.Infrastructure.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IDataService _dataservice;

        public LoginService(IDataService dataservice)
        {
            _dataservice = dataservice;
        }

        public async Task<bool> CheckUserExists(UserEntity userEntity)
        {
            var userEntities = await _dataservice.Read<UserEntity>();
            return userEntities.Any(
                user =>
                    user.Email.Equals(userEntity.Email) && user.Password == userEntity.Password);
        }

        public async Task<bool> LogUserIn(UserEntity userEntity, IAuthenticationManager authenticationManager)
        {
            if (await CheckUserExists(userEntity))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim(ClaimTypes.Name, userEntity.UserName ?? userEntity.Email)
                };

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.Email,
                    ClaimTypes.Role);

                await LogUserOut(authenticationManager);

                authenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = userEntity.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    AllowRefresh = true
                }, identity);

                Thread.CurrentPrincipal = new ClaimsPrincipal(identity);
                return true;
            }
            return false;
        }

        public async Task<bool> LogUserOut(IAuthenticationManager authenticationManager)
        {
            await Task.Run(() =>
            {
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            });
            return true;
        }

        public async Task<bool> CreateAndLoguserIn(UserEntity userEntity, IAuthenticationManager authenticationManager)
        {
            var currentUsers = await _dataservice.Query<UserEntity>(x => x.Where(y => y.Email.Equals(userEntity.Email)));
            if (!currentUsers.Any())
            {
                await _dataservice.Create(userEntity);
            }

            return await LogUserIn(userEntity, authenticationManager);
        }

        public async Task SetUserAvatar(string email, string avatarUrl)
        {
            var avatarEntities = await _dataservice.Query<AvatarEntity>(x => x.Where(z => z.Email.Equals(email)));
            if (!avatarEntities.Any())
            {
                await _dataservice.Create(new AvatarEntity {Email = email, AvatarUrl = avatarUrl});
            }
        }
    }
}