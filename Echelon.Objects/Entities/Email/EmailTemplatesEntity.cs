using System.Collections.Generic;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Email
{
    [Id("EmailTemplatesTable")]
    public class EmailTemplatesEntity
    {
        public IEnumerable<EmailTemplateEntity> Templates { get; set; }
    }
}