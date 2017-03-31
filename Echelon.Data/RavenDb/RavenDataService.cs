using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Data.Entities;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;
using Echelon.Data.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace Echelon.Data.RavenDb
{
    public class RavenDataService : IDataService
    {
        private readonly IDocumentStore _database = DocumentStoreProvider.Database;

        private async Task Open(Func<IAsyncDocumentSession, Task> action)
        {
            try
            {
                using (var session = _database.OpenAsyncSession())
                {
                    await action(session);
                    await session.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Create<TType>(TType entity) where TType : EntityBase
        {
            await Open(session => session.StoreAsync(entity, entity.Id));
        }

        public async Task<IList<TType>> Read<TType>()
        {
            IList<TType> enumerable = null;
            await Open(async session => { enumerable = await session.Query<TType>().ToListAsync(); });
            return enumerable;
        }

        public async Task<IList<TType>> Query<TType>(Func<IQueryable<TType>, IQueryable<TType>> action)
        {
            using (var session = _database.OpenAsyncSession())
            {
                var types = await action(session.Query<TType>()).ToListAsync();
                await session.SaveChangesAsync();
                return types;
            }
        }

        public async Task Update<TType>(Action<TType> action, string id) where TType : EntityBase
        {
            await Open(async session =>
            {
                var type = await session.LoadAsync<TType>(id);
                action(type);
            });
        }

        public async Task Delete<TType>(string id) where TType : EntityBase
        {
            await Open(async session => { await Task.Factory.StartNew(() => session.Delete(id)); });
        }

        public async Task DeleteDocuments<TType>()
        {
            await Open(async session =>
            {
                var documentCollection = await session.Query<TType>().ToListAsync();
                foreach (var document in documentCollection)
                {
                    session.Delete(document);
                }
            });
        }

        public async Task<IList<AvatarUserEntity>> GetIndex()
        {
            new UsersAvatars().Execute(_database);

            using (var session = _database.OpenAsyncSession())
            {
                var avatarEntities = session.Query<AvatarEntity>().Where(x => x.Email.Equals("simonpmarkey@gmail.com"));
                var foo = await avatarEntities.TransformWith<UsersAvatars, AvatarUserEntity>().ToListAsync();

                await session.SaveChangesAsync();

                return foo;
            }
        }

        public async Task GetInclude()
        {
            using (var session = _database.OpenAsyncSession())
            {
                var results =
                    await session.Include<UserEntity>(x => x.Email).LoadAsync<UserEntity>("simonpmarkey@gmail.com");

                // this will not require querying the server!
                var customer = await session.LoadAsync<AvatarEntity>();

                await session.SaveChangesAsync();
            }
        }
    }
}