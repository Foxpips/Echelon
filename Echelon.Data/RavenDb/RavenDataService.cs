using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Echelon.Misc.Attributes;
using Raven.Client;

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

        public async Task Create<TType>(TType entity)
        {
            await Open(session => session.StoreAsync(entity, GetId<TType>()));
        }

        public async Task<TType> Read<TType>()
        {
            IEnumerable<TType> enumerable = null;
            await Open(async session => { enumerable = await session.Query<TType>().ToListAsync(); });
            return enumerable.SingleOrDefault();
        }

        public async Task Update<TType>(Action<TType> action)
        {
            await Open(async session =>
            {
                var type = await session.LoadAsync<TType>(GetId<TType>());
                action(type);
            });
        }

        public async Task Delete<TType>()
        {
            await Open(
                async session =>
                {
                    await Task.Factory.StartNew(
                        () => session.Delete(GetId<TType>()));
                });
        }
    }
}