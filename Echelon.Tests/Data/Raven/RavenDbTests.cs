using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Entities.Email;
using Echelon.Core.Entities.Users;
using Echelon.Core.Extensions;
using Echelon.Core.Infrastructure.Services.Email.Components;
using Echelon.Data.RavenDb;
using NUnit.Framework;

namespace Echelon.Tests.Data.Raven
{
    public class RavenDbTests
    {
        private RavenDataService _dataService;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _dataService = new RavenDataService();

//                        var userEntity = new UserEntity
//                        {
//                            Email = "Test@gmail.com",
//                            UserName = "Test",
//                            Password = HashHelper.CreateHash("password1")
//                        };
//            
//                        await _dataService.Create(userEntity);
//            
//                        var userEntity2 = new UserEntity
//                        {
//                            Email = "Test2@gmail.com",
//                            UserName = "Test2",
//                            Password = HashHelper.CreateHash("password1")
//                        };

//                        await _dataService.Create(userEntity2);
        }

        [Test]
        public async Task Connect_Read_DataBase_Success()
        {
            var users = await _dataService.Read<UserEntity>();
            Assert.NotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [Test]
        public async Task Read_User_Database()
        {
            var users =
                await
                    _dataService.Query<UserEntity>(
                        entities => entities.Where(userEntity => userEntity.Email == "Test@gmail.com"));
            Console.WriteLine(users.First().Email);
        }

        [Test]
        public async Task Remove_Add_User_Success()
        {
            var loginEntity = new UserEntity
            {
                Email = "Pete@gmail.com",
                Password = HashHelper.CreateHash("Peterson1"),
                UserName = "Pete"
            };

            await _dataService.Update<UserEntity>(entity => { entity.UserName = "updated@gmail.com"; }, "test@gmail.com");
            var loginEntities = await _dataService.Read<UserEntity>();
            Assert.That(loginEntities.Any(x => x.Email.Equals(loginEntity.Email)));
        }

        [Test]
        public async Task Delete_Document_Success()
        {
            await _dataService.Delete<UsersEntity>();
            await _dataService.Create(new UsersEntity());
        }

        [Test]
        public async Task CreateEmail_Templates_Success()
        {
            var emailTemplates = new EmailTemplatesEntity
            {
                Templates =
                    new List<EmailTemplateEntity>
                    {
                        new EmailTemplateEntity
                        {
                            Body = "Body Test",
                            Subject = "Subject Test",
                            Type = EmailTemplateEnum.ForgottenPassword
                        }
                    }
            };

            await _dataService.Create(emailTemplates);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _dataService.Update<UserEntity>(entity => { });
        }
    }
}