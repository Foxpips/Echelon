using System.Collections.Generic;
using Echelon.Misc.Attributes;

namespace Echelon.Data.Entities.Email
{
    [Name("EmailTemplates")]
    public class EmailTemplatesEntity : EntityBase
    {
        public override string Id => "EmailTemplatesId";

        public IEnumerable<EmailTemplateEntity> Templates { get; set; }
    }
}