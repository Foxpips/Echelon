using Echelon.Core.Infrastructure.Services.Email;
using Echelon.Core.Infrastructure.Services.Email.Components;

namespace Echelon.Core.Entities.Email
{
    public class EmailTemplateEntity
    {
        public EmailTemplateEnum Type { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}