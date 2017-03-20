using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;

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
    }
}