using System.Threading.Tasks;
using Echelon.Core.Infrastructure.Services.Email.Components;

namespace Echelon.Core.Infrastructure.Services.Email
{
    public interface IEmailSenderService : IService
    {
        Task Send(string recipientEmail, string recipientName, string senderName,
            EmailTemplateEnum emailTemplateEnum, object tokens);
    }
}