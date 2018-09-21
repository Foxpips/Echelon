using System;
using System.Threading.Tasks;
using Echelon.Core.Helpers;
using Echelon.Core.Logging.Loggers;
using Echelon.Data.DataProviders.RavenDb;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Email;
using Echelon.Data.Entities.Users;
using Echelon.DatabaseBuilder.EmailTemplates;
using Echelon.Misc.Enums;

namespace Echelon.DatabaseBuilder
{
    public class Program
    {
        private static RavenDataService _dataService;

        private static void Main()
        {
            Task.Run(async () =>
            {
                await Console.Out.WriteLineAsync("Starting DatabaseBuilder...");
                await CreateEmailTemplates();

                await Console.Out.WriteLineAsync("Stopping DatabaseBuilder");
            }).GetAwaiter().GetResult();
        }

        private static async Task CreateEmailTemplates()
        {
            await Console.Out.WriteLineAsync("Creating Email Temapltes..");
            _dataService = new RavenDataService(new ClientLogger());

            await CreateSampleUsers();

            var type = typeof(EmailTemplateSettings);
            foreach (var fieldInfo in type.GetProperties())
            {
                var entity = fieldInfo.GetValue(type) as EmailTemplateEntity;
                await _dataService.Create(entity);
            }

            await Console.Out.WriteLineAsync("Finished");
        }

        private static async Task CreateSampleUsers()
        {
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

            var avatarEntity = new AvatarEntity {AvatarUrl = "someurl/pic.jpg", FileType = FileTypeEnum.Jpeg };
            await _dataService.Create(avatarEntity);

            userEntity2.AvatarId = avatarEntity.Id;
            await _dataService.Create(userEntity2);
        }
    }
}