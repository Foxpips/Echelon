using System;
using System.Threading.Tasks;
using Echelon.Data.MongoDb;
using NUnit.Framework;

namespace Echelon.Tests.Data.Mongo
{
    public class MongoDbTests
    {
        private MongoDataService _dataService;

        [SetUp]
        public void SetUp()
        {
            _dataService = new MongoDataService();
        }

        [Test]
        public async Task Connect_Read_DataBase_Success()
        {
            await _dataService.Create(new User { Email = "simonpmarkey@gmail.com", Name = "Simon" });
            var read = await _dataService.Query<User>(user => user.Name.Equals("Derek"));
            Assert.NotNull(read);
            Console.WriteLine(read.Email);
        }

        [Test]
        public async Task Connect_ReadAll_DataBase_Success()
        {
            var read = await _dataService.ReadAll<User>();
            Assert.NotNull(read);
            foreach (var user in read)
            {
                Console.WriteLine(user.Email);
            }
        }
    }
}