namespace Echelon.Core.Infrastructure.MassTransit.Commands.Register
{
    public class RegisterNewUserCommand
    {
        public string RegisterUrl { get; set; }

        public string Email { get; set; }
    }
}