using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Extensions;
using Echelon.Data.RavenDb;
using Echelon.Objects.Entities.Users;
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
            usersEntity.Users.Add(new UserEntity { Email = "Test@gmail.com", UserName = "Test", Password = HashHelper.CreateHash("password1") });

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
            var loginEntity = new UserEntity { Email = "Pete@gmail.com", Password = HashHelper.CreateHash("Peterson1") };
            await _dataService.Update<UsersEntity>(entity =>
            {
                entity.Users.Remove(entity.Users.SingleOrDefault(x => x.Email.Equals("Pete@gmail.com")));
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