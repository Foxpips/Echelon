using System.Collections.Generic;
using Echelon.Data;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Email
{
    [Id("EmailTemplatesTable")]
    public class EmailTemplatesEntity : EntityBase
    {
        public IEnumerable<EmailTemplateEntity> Templates { get; set; }
    }
}