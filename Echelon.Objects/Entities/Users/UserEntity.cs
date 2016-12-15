namespace Echelon.Core.Entities.Users
{
    public class UserEntity
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}