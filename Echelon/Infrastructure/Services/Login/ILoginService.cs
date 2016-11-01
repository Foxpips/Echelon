using System.Threading.Tasks;
using Echelon.Entities.Users;

namespace Echelon.Infrastructure.Services.Login
{
    public interface ILoginService
    {
        Task<bool> CheckUserExists(LoginEntity loginEntity);
        Task<bool> LogUserIn(LoginEntity loginEntity);
    }
}