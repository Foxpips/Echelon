using Echelon.Misc.Attributes;
using Echelon.Misc.Enums;

namespace Echelon.Data.Entities.Email
{
    [Name("Emails")]
    public class EmailTemplateEntity : EntityBase
    {
        public override string Id => ((int) Type).ToString();

        public EmailTemplateEnum Type { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}