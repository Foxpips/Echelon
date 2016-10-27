using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities;
using Echelon.Entities.Users;

namespace Echelon.Infrastructure.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IDataService _service;

        public LoginService(IDataService service)
        {
            _service = service;
        }

        public async Task<bool> CheckUserExists(LoginEntity loginEntity)
        {
            var exists = true;
            return exists != null;
        }

        public void LogUserIn()
        {
            throw new System.NotImplementedException();
        }
    }
}