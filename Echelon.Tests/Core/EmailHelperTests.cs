using System.Threading.Tasks;
using Echelon.Core.Entities.Email;
using Echelon.Core.Features.Email;
using Echelon.Data.RavenDb;
using NUnit.Framework;

namespace Echelon.Tests.Core
{
    public class EmailHelperTests
    {
        private EmailSender _emailSender;
        private readonly EmailTokenHelper _emailTokenHelper = new EmailTokenHelper();

        [SetUp]
        public void SetUp()
        {
            _emailSender = new EmailSender(new DataService(), _emailTokenHelper);
        }

        [Ignore("Need to mock private send method as dont want to spam emails each time")]
        [Test]
        public async Task Email_Send_Success()
        {
            await
                _emailSender.Send("simonpmarkey@gmail.com", "Simon", "Test", EmailTemplateEnum.ForgottenPassword,
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