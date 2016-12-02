using System.Threading.Tasks;
using Echelon.Entities.Users;
using Microsoft.Owin.Security;

namespace Echelon.Infrastructure.Services.Login
{
    public interface ILoginService : IService
    {
        Task<bool> LogUserIn(UserEntity userEntity, IAuthenticationManager authenticationManager);
        Task<bool> CreateAndLoguserIn(UserEntity userEntity, IAuthenticationManager authenticationManager);
    }
}