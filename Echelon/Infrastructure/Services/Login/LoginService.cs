using System.Linq;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities;

namespace Echelon.Infrastructure.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IDataService _service;

        public LoginService(IDataService service)
        {
            _service = service;
        }

        public bool CheckUserExists(LoginEntity loginEntity)
        {
            return _service.Read<LoginEntity>(
                entity => entity.Email.Equals(loginEntity.Email) && entity.Password.Equals(loginEntity.Password))
                .SingleOrDefault() != null;
        }

        public void LogUserIn()
        {
            throw new System.NotImplementedException();
        }
    }
}