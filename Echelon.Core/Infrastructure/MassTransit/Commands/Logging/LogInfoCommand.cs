namespace Echelon.Core.Infrastructure.MassTransit.Commands.Logging
{
    public class LogInfoCommand : IBusCommand
    {
        public string Content { get; set; }
    }
}