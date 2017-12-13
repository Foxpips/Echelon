using System.Threading.Tasks;
using Echelon.Misc.Enums;

namespace Echelon.Core.Infrastructure.Services.Email
{
    public interface IEmailSenderService : IService
    {
        Task Send(string recipientEmail, EmailTemplateEnum emailTemplateEnum, object tokens);
    }
}