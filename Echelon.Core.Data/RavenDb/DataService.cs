using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Echelon.Core.Attributes;
using Echelon.Core.Interfaces.Data;
using Raven.Client;

namespace Echelon.Core.Data.RavenDb
{
    public class DataService : IDataService
    {
        private readonly IDocumentStore _database = DocumentStoreProvider.Database;

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
            await Open(session => session.StoreAsync(entity, entity.GetType().GetCustomAttribute<IdAttribute>().Id));
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
                var types = await session.LoadAsync<TType>(typeof(TType).GetCustomAttribute<IdAttribute>().Id);
                action(types);
            });
        }

        public async Task Delete<TType>()
        {
            await Open(
                async session =>
                {
                    await Task.Factory.StartNew(
                        () => session.Delete(typeof(TType).GetCustomAttribute<IdAttribute>().Id));
                });
        }
    }
}