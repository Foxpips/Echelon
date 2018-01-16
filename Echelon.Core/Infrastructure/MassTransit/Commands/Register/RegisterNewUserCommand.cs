using Echelon.Core.Infrastructure.MassTransit.Extensions;
using Echelon.Data.Entities.Users;

namespace Echelon.Core.Infrastructure.MassTransit.Commands.Register
{
    public class RegisterNewUserCommand : IBusCommand
    {
        public string RegisterUrl { get; set; }

        public TempUserEntity User { get; set; }
    }
}