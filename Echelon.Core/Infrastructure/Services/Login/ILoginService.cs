using System.Threading.Tasks;
using Echelon.Data.Entities.Users;
using Microsoft.Owin.Security;

namespace Echelon.Core.Infrastructure.Services.Login
{
    public interface ILoginService : IService
    {
        Task<bool> IsRegistered(string email);
        Task<bool> LogUserIn(UserEntity userEntity, IAuthenticationManager authenticationManager);
        Task<bool> LogUserOut(IAuthenticationManager authenticationManager);
        Task CreateUser(UserEntity userEntity, string avatarUrl);
        Task CreateTempUser(TempUserEntity tempUserEntity);
    }
}