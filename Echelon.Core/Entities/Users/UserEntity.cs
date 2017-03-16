using Echelon.Data;

namespace Echelon.Core.Entities.Users
{
    public class UserEntity : EntityBase
    {
        public UserEntity()
        {
            Id = Email;
        }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AvatarUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}