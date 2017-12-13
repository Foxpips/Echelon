using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.Services.Email.Components;
using Echelon.Data;
using Echelon.Data.Entities.Email;
using Echelon.Misc.Enums;

namespace Echelon.Core.Infrastructure.Services.Email
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IDataService _dataService;
        private readonly ITokenHelper _tokenHelper;

        public EmailSenderService(IDataService dataService, ITokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
            _dataService = dataService;
        }

        public async Task Send(string recipientEmail, EmailTemplateEnum emailTemplateEnum, object tokens)
        {
            var fromAddress = new MailAddress(EmailSettings.EmailAccount);
            var emailTemplate = await _dataService.Single<EmailTemplateEntity>(entities => entities.Where(userEntity => userEntity.Type == emailTemplateEnum));
            
            SendMessage(recipientEmail, _tokenHelper.Replace(tokens, emailTemplate.Subject), _tokenHelper.Replace(tokens, emailTemplate.Body), fromAddress);
        }

        private static void SendMessage(string recipientEmail, string subject, string body,
            MailAddress fromAddress)
        {
            using (var message = new MailMessage(fromAddress, new MailAddress(recipientEmail))
            {
                Subject = subject,
                Body = body
            })
            {
                CreateEmailClient(fromAddress).Send(message);
            }
        }

        private static SmtpClient CreateEmailClient(MailAddress fromAddress)
        {
            return new SmtpClient
            {
                EnableSsl = true,
                Port = EmailSettings.Port,
                Host = EmailSettings.SmtpHost,
                UseDefaultCredentials = false,
                Timeout = EmailSettings.Timeout,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, EmailSettings.Password)
            };
        }
    }
}