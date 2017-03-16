using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Entities.Users;
using Echelon.Core.Extensions;
using Echelon.Data.MongoDb;
using NUnit.Framework;

namespace Echelon.Tests.Data.Mongo
{
    public class MongoDbTests
    {
        private MongoDataService _dataService;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _dataService = new MongoDataService();

            var userEntity = new UserEntity
            {
                Email = "Test@gmail.com",
                UserName = "Test",
                Password = HashHelper.CreateHash("password1")
            };

            await _dataService.Create(userEntity);

            var userEntity2 = new UserEntity
            {
                Email = "Test2@gmail.com",
                UserName = "Test2",
                Password = HashHelper.CreateHash("password1")
            };

            await _dataService.Create(userEntity2);
        }

        [Test]
        public async Task Connect_ReadRows_DataBase_Success()
        {
            var users = await _dataService.Read<UserEntity>();
            Assert.NotNull(users);
            Assert.IsTrue(users.Count > 0);
        }


        [Test]
        public async Task Connect_Read_DataBase_Success()
        {
            var exists = await _dataService.Query<UsersEntity>(user => user.Users.First().Email.Equals("simonpmarkey@gmail.com"));
            if (exists == null)
            {
                await _dataService.Create(new UserEntity { Email = "simonpmarkey@gmail.com", UserName = "Simon" });
            }

            var read = await _dataService.Query<User>(user => user.Name.Equals("Simon"));
            Assert.NotNull(read);
            Console.WriteLine(read.First().Email);
        }

        [Test]
        public async Task Connect_ReadAll_DataBase_Success()
        {
            var read = await _dataService.Read<UsersEntity>();
            Assert.NotNull(read);
            foreach (var user in read)
            {
                Console.WriteLine(user.Users.First().Email);
            }
        }

        [Test]
        public async Task Update_User_Result()
        {
            await _dataService.Update<UsersEntity>(entity =>
            {
                entity.Users.Remove(entity.Users.SingleOrDefault(x => x.Email.Equals("Pete@gmail.com")));
                entity.Users.Add(new UserEntity { Email = "Pete@gmail.com", Password = HashHelper.CreateHash("Peterson1"), UserName = "Pete" });
            });

            var query = await _dataService.Query<User>(x => x.Email == "Pete@gmail.com");
        }
    }
}