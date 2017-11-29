using Echelon.Core.Infrastructure.MassTransit.Extensions;

namespace Echelon.Core.Infrastructure.MassTransit.Commands.Register
{
    public class RegisterNewUserCommand : IBusCommand
    {
        public string RegisterUrl { get; set; }

        public string Email { get; set; }
    }
}