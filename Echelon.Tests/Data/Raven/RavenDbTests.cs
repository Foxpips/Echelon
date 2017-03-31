﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Helpers;
using Echelon.Core.Infrastructure.Services.Email.Components;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Email;
using Echelon.Data.Entities.Users;
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

            var userEntity = new UserEntity
            {
                Email = "Test@gmail.com",
                UserName = "Test",
                Password = HashHelper.CreateHash("password1")
            };

            await _dataService.Create(userEntity);

            var userEntity2 = new UserEntity
            {
                Email = "simonpmarkey@gmail.com",
                UserName = "Test2",
                Password = HashHelper.CreateHash("password1")
            };

            await _dataService.Create(userEntity2);

            var userEntity3 = new UserEntity
            {
                Email = "smarkey@gmail.com",
                UserName = "simon",
                Password = HashHelper.CreateHash("password1")
            };

            await _dataService.Create(userEntity3);

            var emailTemplates = new EmailTemplatesEntity
            {
                Templates =
                    new List<EmailTemplateEntity>
                    {
                        new EmailTemplateEntity
                        {
                            Body = "Body Test",
                            Subject = "Subject Test",
                            Type = EmailTemplateEnum.ForgottenPassword.ToString()
                        }
                    }
            };

            await _dataService.Create(emailTemplates);
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
            await
                _dataService.Update<UserEntity>(entity => { entity.UserName = "updated@gmail.com"; }, "test@gmail.com");
            var loginEntities = await _dataService.Read<UserEntity>();
            Assert.That(loginEntities.Any(x => x.UserName.Equals("updated@gmail.com")));
        }

        [Test]
        public async Task Delete_Document_Success()
        {
            var userEntity = new UserEntity
            {
                Email = "deleteTest@gmail.com",
                UserName = "Test",
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
            var expected = await _dataService.Read<EmailTemplatesEntity>();
            Assert.AreEqual(expected.SingleOrDefault()?.Templates.First().Body, "Body Test");
        }

        [Test]
        public async Task IndexTest_Scenario_Result()
        {
            var avatarEntity = new AvatarEntity { Email = "simonpmarkey@gmail.com", AvatarUrl = "someurl/pic.jpg" };
            var avatarEntity2 = new AvatarEntity { Email = "smarkey@gmail.com", AvatarUrl = "someurl/pic2.jpg" };
            await _dataService.Create(avatarEntity);
            await _dataService.Create(avatarEntity2);

            var avatarUserEntities = await _dataService.GetIndex();
            var avatarUserEntity = avatarUserEntities.First();
            Assert.That(avatarUserEntities.Count(x => x.AvatarUrl == avatarUserEntity.AvatarUrl), Is.EqualTo(1));
            Assert.AreEqual(avatarUserEntity.AvatarUrl, avatarEntity.AvatarUrl);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _dataService.DeleteDocuments<UserEntity>();
            await _dataService.DeleteDocuments<EmailTemplatesEntity>();
            await _dataService.DeleteDocuments<AvatarEntity>();
        }
    }
}