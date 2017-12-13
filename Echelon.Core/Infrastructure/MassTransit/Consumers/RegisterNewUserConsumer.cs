using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands.Register;
using Echelon.Core.Infrastructure.Services.Email;
using Echelon.Misc.Enums;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class RegisterNewUserConsumer : IConsumer<RegisterNewUserCommand>
    {
        private readonly IEmailSenderService _emailSenderService;

        public RegisterNewUserConsumer(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task Consume(ConsumeContext<RegisterNewUserCommand> context)
        {
            await
                _emailSenderService.Send(context.Message.Email,
                    EmailTemplateEnum.AccountConfirmation, new {name = "Test"});
        }
    }
}