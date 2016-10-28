using System.Collections.Generic;
using Echelon.Core.Attributes;

namespace Echelon.Entities.Users
{
    [Id("UsersTable")]
    public class UsersEntity
    {
        public IList<LoginEntity> Users { get; set; }

        public UsersEntity()
        {
            Users = new List<LoginEntity>();
        }
    }
}