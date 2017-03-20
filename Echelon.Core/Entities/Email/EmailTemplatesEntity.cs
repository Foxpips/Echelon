using System.Collections.Generic;
using Echelon.Data;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Email
{
    [Name("EmailTemplates")]
    public class EmailTemplatesEntity : EntityBase
    {
        public override string Id => "EmailTemplatesId";

        public IEnumerable<EmailTemplateEntity> Templates { get; set; }
    }
}