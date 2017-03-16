﻿using System.Threading.Tasks;
using Echelon.Core.Entities.Users;
using Microsoft.Owin.Security;

namespace Echelon.Core.Infrastructure.Services.Login
{
    public interface ILoginService : IService
    {
        Task<bool> LogUserIn(UserEntity userEntity, IAuthenticationManager authenticationManager);
        Task<bool> LogUserOut(IAuthenticationManager authenticationManager);
        Task<bool> CreateAndLoguserIn(UserEntity userEntity, IAuthenticationManager authenticationManager);
    }
}