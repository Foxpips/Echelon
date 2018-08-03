using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Infrastructure.Services.Email;
using Echelon.Core.Infrastructure.Services.Email.Components;
using Echelon.Core.Logging.Loggers;
using Echelon.Data.DataProviders.RavenDb;
using Echelon.Data.Entities.Email;
using Echelon.DatabaseBuilder.EmailTemplates;
using Echelon.Misc.Enums;
using NUnit.Framework;

namespace Echelon.Tests.Core
{
    public class EmailHelperTests
    {
        private EmailSenderService _emailSenderService;
        private readonly EmailTokenHelper _emailTokenHelper = new EmailTokenHelper();
        private RavenDataService _dataService;

        [SetUp]
        public async Task SetUp()
        {
            var clientLogger = new ClientLogger();
            _dataService = new RavenDataService(clientLogger);
            _emailSenderService = new EmailSenderService(new RavenDataService(clientLogger), _emailTokenHelper,
                clientLogger);
            await _dataService.Create(EmailTemplateSettings.ResetPassword);
            await _dataService.Create(EmailTemplateSettings.AccountConfirmation);
        }

        [Test]
        public async Task Email_Send_Success()
        {
            await
                _emailSenderService.Send("simonpmarkey@gmail.com", EmailTemplateEnum.AccountConfirmation,
                    new {username = "Foxpips", link = "Why hello there world!"});
        }

        [Test]
        public void EmailHelper_TokenReplace_Success()
        {
            var replace = _emailTokenHelper.Replace(new
            {
                name = "Test",
                orderRef = 1890
            },
                "Hi {{name}} how are you? your orderref is {{orderRef}}");

            Assert.True("Hi Test how are you? your orderref is 1890".Equals(replace));
        }

        [Test]
        public async Task EmailHelper_GetTemaplte_Success()
        {
            var emailTemplate =
                await _dataService.Single<EmailTemplateEntity>(entities =>
                    entities.Where(userEntity => userEntity.Type == EmailTemplateEnum.ResetPassword));

            Assert.NotNull(emailTemplate);
        }
    }
}