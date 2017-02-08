using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Entities.Email;
using Echelon.Core.Entities.Users;
using Echelon.Core.Extensions;
using Echelon.Core.Features.Email;
using Echelon.Data.RavenDb;
using NUnit.Framework;

namespace Echelon.Tests.Data
{
    public class DataServiceTests
    {
        private DataService _dataService;

        [SetUp]
        public async Task SetUp()
        {
            _dataService = new DataService();

            var usersEntity = new UsersEntity();
            usersEntity.Users.Add(new UserEntity
            {
                Email = "Test@gmail.com",
                UserName = "Test",
                Password = HashHelper.CreateHash("password1")
            });

            await _dataService.Create(usersEntity);
        }

        [Test]
        public async Task Connect_Read_DataBase_Success()
        {
            var users = await _dataService.Read<UsersEntity>();
            Assert.NotNull(users);
            Assert.IsTrue(users.Users.Count > 0);
        }

        [Test]
        public async Task Remove_Add_User_Success()
        {
            var loginEntity = new UserEntity { Email = "Pete@gmail.com", Password = HashHelper.CreateHash("Peterson1"), UserName = "Pete" };
            await _dataService.Update<UsersEntity>(entity =>
            {
                entity.Users.Remove(entity.Users.SingleOrDefault(x => x.Email.Equals("Pete@gmail.com")));
                entity.Users.Add(loginEntity);
            });

            var loginEntities = await _dataService.Read<UsersEntity>();
            Assert.That(loginEntities.Users.Any(x => x.Email.Equals(loginEntity.Email)));
        }

        [Test]
        public async Task Delete_Document_Success()
        {
            await _dataService.Delete<UsersEntity>();
        }

        [Test]
        public async Task CreateEmail_Templates_Success()
        {
            var emailTemplates = new EmailTemplatesEntity
            {
                Templates = new List<EmailTemplateEntity> { new EmailTemplateEntity { Body = "Body Test", Subject = "Subject Test", Type = EmailTemplateEnum.ForgottenPassword } }
            };

            await _dataService.Create(emailTemplates);
        }
    }
}