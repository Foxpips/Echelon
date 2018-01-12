using AutoMapper;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.AutoMapper.Profiles;
using NUnit.Framework;

namespace Echelon.Tests.Mapping
{
    public class MapperTests
    {
        [Test]
        public void Test_UserProfile_MappingSuccess()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });


            var tempUserEntity = new TempUserEntity { Id = "TestId", Email = "Test@Test.com", Password = "TestPassword", DisplayName = "TestDisplayName" };
            var userEntity = Mapper.Instance.Map<UserEntity>(tempUserEntity);

            Assert.That(userEntity.DisplayName.Equals(tempUserEntity.DisplayName));
        }
    }
}