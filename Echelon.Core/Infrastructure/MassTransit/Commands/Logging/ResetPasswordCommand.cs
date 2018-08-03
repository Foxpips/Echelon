using Echelon.Core.Infrastructure.MassTransit.Extensions;

namespace Echelon.Core.Infrastructure.MassTransit.Commands.Logging
{
    public class ResetPasswordCommand : IBusCommand
    {
        public string Email { get; set; }
        public string ResetLink { get; set; }
    }
}