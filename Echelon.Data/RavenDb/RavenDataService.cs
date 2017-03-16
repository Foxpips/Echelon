using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Echelon.Misc.Attributes;
using Raven.Client;
using Raven.Client.Linq;

namespace Echelon.Data.RavenDb
{
    public class RavenDataService : IDataService
    {
        private readonly IDocumentStore _database = DocumentStoreProvider.Database;

        private static string GetId<TType>()
        {
            return typeof(TType).GetCustomAttribute<IdAttribute>().Id;
        }

        private async Task Open(Func<IAsyncDocumentSession, Task> action)
        {
            using (var session = _database.OpenAsyncSession())
            {
                await action(session);
                await session.SaveChangesAsync();
            }
        }

        public async Task Create<TType>(TType entity) where TType : EntityBase
        {
            await Open(session => session.StoreAsync(entity, entity.Id));
        }

        public async Task<List<TType>> Read<TType>()
        {
            IList<TType> enumerable = null;
            await Open(async session => { enumerable = await session.Query<TType>().ToListAsync(); });
            return enumerable.ToList();
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

        public async Task Update<TType>(Action<TType> action, string id = null)
        {
            await Open(async session =>
            {
                var type = await session.LoadAsync<TType>(id ?? GetId<TType>());
                action(type);
            });
        }

        public async Task Delete<TType>(string id = null)
        {
            await Open(
                async session =>
                {
                    await Task.Factory.StartNew(() => session.Delete(id ?? GetId<TType>()));
                });
        }
    }
}