using System;
using System.Collections.Generic;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Users
{
    [Id("UsersTable")]
    public class UsersEntity : EntityBase
    {
        public IList<UserEntity> Users { get; set; }

        public UsersEntity()
        {
            Users = new List<UserEntity>();
        }
    }

    public class EntityBase
    {
        public EntityBase()
        {
            Id = Guid.NewGuid();
        }

        public object Id { get; set; }
    }
}