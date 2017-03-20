﻿using Echelon.Data;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Users
{
    [Name("Users")]
    public class UserEntity : EntityBase
    {
        public override string Id => Email;

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AvatarUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}