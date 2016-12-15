using System.Threading.Tasks;
using Echelon.Core.Entities.Email;

namespace Echelon.Core.Features.Email
{
    public interface IEmailSender
    {
        Task Send(string recipientEmail, string recipientName, string senderName,
            EmailTemplateEnum emailTemplateEnum, object tokens);
    }
}