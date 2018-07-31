using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands.Logging;
using Echelon.Core.Infrastructure.Services.Email;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Logging.Loggers;
using Echelon.Misc.Enums;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class ResetPasswordConsumer : BaseConsumer<ResetPasswordCommand> 
    {
        private readonly IEmailSenderService _emailSenderService;

        public ResetPasswordConsumer(IEmailSenderService emailSenderService, IClientLogger clientLogger) : base(clientLogger)
        {
            _emailSenderService = emailSenderService;
        }

        protected override async Task ConsumeInternal(ConsumeContext<ResetPasswordCommand> context)
        {
            var message = context.Message;

            await _emailSenderService.Send(message.Email, EmailTemplateEnum.ResetPassword,
                new
                {
                    resetLink = "TODO"
                });
        }
    }
}