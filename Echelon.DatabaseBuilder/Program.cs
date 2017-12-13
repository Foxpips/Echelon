using System;
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
                await Console.Out.WriteLineAsync("Starting DatabaseBuilder...");
                await CreateEmailTemplates();

                await Console.Out.WriteLineAsync("Stopping DatabaseBuilder");
            }).GetAwaiter().GetResult();
        }

        private static async Task CreateEmailTemplates()
        {
            await Console.Out.WriteLineAsync("Creating Email Temapltes..");

            _dataService = new RavenDataService(new ClientLogger());

            var forgottenPassword =
                new EmailTemplateEntity
                {
                    Subject = "Forgotten Password",
                    Body = "Forgot your password? \r\n No Problem. Just click the link below to reset it \r\n {{Link}}",
                    Type = EmailTemplateEnum.ForgottenPassword
                };

            var accountConfirmation =
                new EmailTemplateEntity
                {
                    Subject = "Thanks for Registering",
                    Body = "Welcome {{Username}}, \r\n Thanks for signing up! \r\n Please click the link below to confirm your account! {{Body}}",
                    Type = EmailTemplateEnum.AccountConfirmation
                };

            await _dataService.Create(forgottenPassword);
            await _dataService.Create(accountConfirmation);

            await Console.Out.WriteLineAsync("Finished");
        }
    }
}