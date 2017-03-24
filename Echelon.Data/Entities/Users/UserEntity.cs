using Echelon.Misc.Attributes;

namespace Echelon.Data.Entities.Users
{
    [Name("Users")]
    public class UserEntity : EntityBase
    {
        public override string Id => Email;

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool UserNameEnabled { get; set; }

        public string Password { get; set; }

        public string AvatarUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}