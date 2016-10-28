namespace Echelon.Entities.Users
{
    public class LoginEntity
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public LoginEntity(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}