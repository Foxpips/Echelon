using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Logging.Loggers;
using Echelon.Data.Entities;
using Echelon.Data.Entities.Users;
using Echelon.Data.Indexes.Raven;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Echelon.Data.DataProviders.RavenDb
{
    public class RavenDataService : IDataService
    {
        private readonly IDocumentStore _database = DocumentStoreProvider.Database;
        private readonly IClientLogger _clientLogger;

        public RavenDataService(IClientLogger clientLogger)
        {
            _clientLogger = clientLogger;
        }

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
                _clientLogger.Error($"Accessing Ravendb query error:{e.Message}");
                throw;
            }
        }

        private async Task<TReturnType> OpenAndReturn<TReturnType>(Func<IAsyncDocumentSession, Task<TReturnType>> action)
        {
            try
            {
                using (var session = _database.OpenAsyncSession())
                {
                    var returnType = await action(session);
                    await session.SaveChangesAsync();
                    return returnType;
                }
            }
            catch (Exception e)
            {
                _clientLogger.Error($"Accessing Ravendb query error:{e.Message}");
                throw;
            }
        }

        public async Task Create<TType>(TType entity) where TType : EntityBase
        {
            await Open(async session => { await session.StoreAsync(entity, entity.Id); });
        }

        public async Task<IList<TType>> Read<TType>()
        {
            return await OpenAndReturn(async session => await session.Query<TType>().ToListAsync());
        }

        public async Task<TType> Load<TType>(string id)
        {
            return await OpenAndReturn(async session => await session.LoadAsync<TType>(id));
        }

        public async Task<IList<TType>> Query<TType>(Func<IQueryable<TType>, IQueryable<TType>> action)
        {
            return await OpenAndReturn(async session =>
            {
                var types = await action(session.Query<TType>()).ToListAsync();
                await session.SaveChangesAsync();
                return types;
            });
        }

        public async Task<TType> Single<TType>(Func<IQueryable<TType>, IQueryable<TType>> action)
        {
            return await OpenAndReturn(async session =>
            {
                var types = await action(session.Query<TType>()).ToListAsync();
                await session.SaveChangesAsync();
                return types.SingleOrDefault();
            });
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

        public async Task<TType> TransformUserAvatars<TType>(string id)
        {
            return await OpenAndReturn(async session =>
            {
                var userEntities = session.Query<UserEntity>().Customize(c=>c.WaitForNonStaleResults()).Where(x => x.Email.Equals(id));
                var transformedTypes = await userEntities.TransformWith<UserAvatarTransform, TType>().ToListAsync();

                await session.SaveChangesAsync();
                return transformedTypes.SingleOrDefault();
            });
        }

        public async Task<bool> Exists<TType>(string id)
        {
            return await OpenAndReturn(async session => await session.LoadAsync<TType>(id)) != null;
        }

        public async Task<TTransformedType> GetIndex<TTransformer, TTransformedType>(string id)
            where TTransformer : AbstractTransformerCreationTask, new()
        {
            return await OpenAndReturn(async session =>
            {
                var userEntities = session.Query<UserEntity>().Where(x => x.Email.Equals(id));
                var transformedTypes = await userEntities.TransformWith<TTransformer, TTransformedType>().ToListAsync();

                await session.SaveChangesAsync();

                return transformedTypes.SingleOrDefault();
            });
        }
    }
}