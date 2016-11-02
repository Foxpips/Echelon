using System.Threading.Tasks;
using Echelon.Entities.Users;
using Microsoft.Owin.Security;

namespace Echelon.Infrastructure.Services.Login
{
    public interface ILoginService
    {
        Task<bool> CheckUserExists(LoginEntity loginEntity);
        Task<bool> LogUserIn(LoginEntity loginEntity, IAuthenticationManager authenticationManager);
        Task<bool> CreateAndLoguserIn(LoginEntity loginEntity, IAuthenticationManager authenticationManager);
    }
}