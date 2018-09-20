using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Helpers;
using Echelon.Core.Logging.Loggers;
using Echelon.Data.DataProviders.RavenDb;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Email;
using Echelon.Data.Entities.Transforms;
using Echelon.Data.Entities.Users;
using Echelon.DatabaseBuilder.EmailTemplates;
using Echelon.Misc.Enums;
using NUnit.Framework;

namespace Echelon.Tests.Data.Raven
{
    public class RavenDbTests
    {
        private RavenDataService _dataService;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _dataService = new RavenDataService(new ClientLogger());
            await _dataService.Create(new UserEntity { FirstName = "TestName", Email = "Test@gmail.com" });

            var type = typeof(EmailTemplateSettings);
            foreach (var fieldInfo in type.GetProperties())
            {
                var entity = fieldInfo.GetValue(type) as EmailTemplateEntity;
                await _dataService.Create(entity);
            }
        }

        [Test]
        public async Task Connect_Read_DataBase_Success()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200));
            var users = await _dataService.Read<UserEntity>();
            Assert.NotNull(users);
            Console.WriteLine(users.Count);
            Assert.IsTrue(users.Count > 0);
        }

        [Test]
        public async Task Read_User_Database()
        {
            var users = await _dataService.Query<UserEntity>(entities => entities.Where(userEntity => userEntity.Email == "Test@gmail.com"));
            Assert.IsTrue(users.Any());
        }

        [Test]
        public async Task Remove_Add_User_Success()
        {
            await _dataService.Update<UserEntity>(entity => { entity.DisplayName = "updated@gmail.com"; }, "Test@gmail.com");
            var loginEntities = await _dataService.Query<UserEntity>(x => x.Where(y => y.DisplayName != null));
            Assert.That(loginEntities.Any(x => x.DisplayName.Equals("updated@gmail.com")));
        }

        [Test]
        public async Task Delete_Document_Success()
        {
            var userEntity = new UserEntity
            {
                Email = "deleteTest@gmail.com",
                DisplayName = "Test",
                Password = HashHelper.CreateHash("password1")
            };

            await _dataService.Create(userEntity);
            Assert.NotNull(await _dataService.Query<UserEntity>(x => x.Where(z => z.Email.Equals(userEntity.Email))));

            await _dataService.Delete<UserEntity>("deleteTest@gmail.com");
            Assert.False((await _dataService.Read<UserEntity>()).Any(x => x.Email.Equals(userEntity.Email)));
        }

        [Test]
        public async Task CreateEmail_Templates_Success()
        {
            var expected = await _dataService.Load<EmailTemplateEntity>(((int)EmailTemplateEnum.ResetPassword).ToString());
            Assert.AreEqual(EmailTemplateSettings.ResetPassword.Body, expected.Body);
        }

        [Test]
        public async Task IndexTest_Scenario_Result()
        {
            var avatarEntity = new AvatarEntity
            {
                AvatarUrl = "someurl/pic.jpg",
                FileType = FileTypeEnum.Jpg
            };
            var userEntity = new UserEntity
            {
                Email = "testuser@gmail.com",
                AvatarId = avatarEntity.Id
            };

            await _dataService.Create(avatarEntity);
            await _dataService.Create(userEntity);

            var avatarUserEntity = await _dataService.TransformUserAvatars<UserAvatarEntity>(userEntity.Id);
            Assert.AreEqual(avatarUserEntity.AvatarUrl, "someurl/pic.jpg");
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _dataService.DeleteDocuments<UserEntity>();
            await _dataService.DeleteDocuments<EmailTemplateEntity>();
            await _dataService.DeleteDocuments<AvatarEntity>();
        }
    }
}