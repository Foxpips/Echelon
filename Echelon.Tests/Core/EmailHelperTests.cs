using System.Threading.Tasks;
using Echelon.Core.Entities.Email;
using Echelon.Core.Infrastructure.Services.Email;
using Echelon.Core.Infrastructure.Services.Email.Components;
using Echelon.Data.RavenDb;
using NUnit.Framework;

namespace Echelon.Tests.Core
{
    public class EmailHelperTests
    {
        private EmailSenderService _emailSenderService;
        private readonly EmailTokenHelper _emailTokenHelper = new EmailTokenHelper();

        [SetUp]
        public void SetUp()
        {
            _emailSenderService = new EmailSenderService(new DataService(), _emailTokenHelper);
        }

        [Test]
        public async Task Email_Send_Success()
        {
            await
                _emailSenderService.Send("simonpmarkey@gmail.com", "Simon", "Test", EmailTemplateEnum.ForgottenPassword,
                    new {name = "Test"});
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
    }
}