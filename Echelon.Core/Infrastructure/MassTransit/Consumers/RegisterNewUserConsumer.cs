using System.Threading.Tasks;
using Echelon.Core.Infrastructure.MassTransit.Commands.Register;
using Echelon.Core.Infrastructure.Services.Email;
using Echelon.Core.Infrastructure.Services.Login;
using Echelon.Core.Logging.Loggers;
using Echelon.Misc.Enums;
using MassTransit;

namespace Echelon.Core.Infrastructure.MassTransit.Consumers
{
    public class RegisterNewUserConsumer : BaseConsumer<RegisterNewUserCommand> 
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly ILoginService _loginService;

        public RegisterNewUserConsumer(IEmailSenderService emailSenderService, ILoginService loginService, IClientLogger clientLogger) : base(clientLogger)
        {
            _loginService = loginService;
            _emailSenderService = emailSenderService;
        }

        protected override async Task ConsumeInternal(ConsumeContext<RegisterNewUserCommand> context)
        {
            var message = context.Message;

            await _loginService.CreateTempUser(message.User);
            await _emailSenderService.Send(message.User.Email, EmailTemplateEnum.AccountConfirmation,
                new
                {
                    username = message.User.UserName,
                    link = message.RegisterUrl
                });
        }
    }
}