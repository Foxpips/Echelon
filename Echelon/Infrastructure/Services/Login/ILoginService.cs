using Echelon.Entities;

namespace Echelon.Infrastructure.Services.Login
{
    public interface ILoginService
    {
        bool CheckUserExists(LoginEntity loginEntity);
        void LogUserIn();
    }
}