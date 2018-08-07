namespace Echelon.Core.Infrastructure.MassTransit.Commands.ResetPassword
{
    public class ResetPasswordCommand : IBusCommand
    {
        public string Email { get; set; }
        public string ResetLink { get; set; }
    }
}