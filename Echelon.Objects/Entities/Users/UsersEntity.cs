using System.Collections.Generic;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Users
{
    [Id("UsersTable")]
    public class UsersEntity
    {
        public IList<UserEntity> Users { get; set; }

        public UsersEntity()
        {
            Users = new List<UserEntity>();
        }
    }
}