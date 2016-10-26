using System.Linq;
using Echelon.Core.Data;
using Echelon.Entities;
using NUnit.Framework;

namespace Echelon.Tests.Data
{
    public class DataServiceTests
    {
        private DataService _dataService;

        [SetUp]
        public void SetUp()
        {
            _dataService = new DataService();
        }

        [Test]
        public void Connect_DataBase_Success()
        {
            var enumerable = _dataService.Read<LoginEntity>(item => item.Email.Equals("simonpmarkey@gmail.com")).ToList();

            if (enumerable.Count == 0)
            {
                _dataService.Create(new LoginEntity { Password = "password1", Email = "simonpmarkey@gmail.com" });
            }

            _dataService.Delete<LoginEntity>("LoginEntities", item => item.Email.Equals("simonpmarkey@gmail.com"));
        }
    }
}