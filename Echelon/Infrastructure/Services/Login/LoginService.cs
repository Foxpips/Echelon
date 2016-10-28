using System;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities.Users;

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
            return usersEntity.Users.Any(user => user.Email.Equals(loginEntity.Email));

        }

        public async Task LogUserIn(LoginEntity loginEntity)
        {
            if (await CheckUserExists(loginEntity))
            {
                
            }
        }
    }
}