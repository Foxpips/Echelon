using System.Collections.Generic;
using System.Threading.Tasks;
using Echelon.Core.Logging.Loggers;
using Echelon.Data.DataProviders.RavenDb;
using Echelon.Data.Entities.Email;
using Echelon.Misc.Enums;

namespace Echelon.DatabaseBuilder
{
    public class Program
    {
        private static RavenDataService _dataService;

        private static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                _dataService = new RavenDataService(new ClientLogger());

                var emailTemplates = new EmailTemplatesEntity
                {
                    Templates =
                        new List<EmailTemplateEntity>
                        {
                            new EmailTemplateEntity
                            {
                                Body = "{{Body}}",
                                Subject = "{{Subject}}",
                                Type = EmailTemplateEnum.ForgottenPassword
                            }
                        }
                };

                await _dataService.Create(emailTemplates);
            }).GetAwaiter().GetResult();
        }
    }
}