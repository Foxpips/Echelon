using System.Threading.Tasks;
using Echelon.Entities;
using Echelon.Entities.Users;

namespace Echelon.Infrastructure.Services.Login
{
    public interface ILoginService
    {
        Task<bool> CheckUserExists(LoginEntity loginEntity);
        void LogUserIn();
    }
}