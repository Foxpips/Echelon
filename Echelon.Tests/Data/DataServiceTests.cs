﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Data.RavenDb;
using Echelon.Entities.Users;
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

            await _dataService.Create(new UsersEntity
            {
                Users = new List<LoginEntity>
                {
                    new LoginEntity("simonpmarkey@gmail.com", "password1"),
                    new LoginEntity("test@Cindy.com", "test2"),
                    new LoginEntity("test@test.com", "test1")
                }
            });
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
            var loginEntity = new LoginEntity("Pete", "Peterson");
            await _dataService.Update<UsersEntity>(entity =>
            {
                entity.Users.Remove(entity.Users.SingleOrDefault(x => x.Email.Equals("Pete")));
                entity.Users.Add(loginEntity);
            });

            var loginEntities = _dataService.Read<UsersEntity>().Result.Users;
            Assert.That(loginEntities.Any(x => x.Email.Equals(loginEntity.Email)));
        }

        [Test]
        public async Task Delete_Document_Success()
        {
            await _dataService.Delete<UsersEntity>();
        }
    }
}