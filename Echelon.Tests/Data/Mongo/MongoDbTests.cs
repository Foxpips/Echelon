using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Entities.Users;
using Echelon.Core.Extensions;
using Echelon.Data.MongoDb;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
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
        public async Task Connect_Read_Count_Success()
        {
            List<UsersEntity> users = await _dataService.ReadAll<UsersEntity>();
            foreach (var usersEntity in users)
            {
                Console.WriteLine(usersEntity.Users.First().Email);
            }
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
            var read = await _dataService.ReadAll<UsersEntity>();
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