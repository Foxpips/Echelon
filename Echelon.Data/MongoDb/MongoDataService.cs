using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Echelon.Misc.Attributes;
using MongoDB.Driver;

namespace Echelon.Data.MongoDb
{
    public class MongoDataService : IDataService
    {
        protected static IMongoDatabase Database = DocumentStoreProvider.Database;

        private static string GetName<TType>()
        {
            return typeof(TType).GetCustomAttribute<NameAttribute>()?.Name;
        }

        private static async Task Open<TType>(Func<IMongoCollection<TType>, Task> action)
        {
            await action(Database.GetCollection<TType>(GetName<TType>()));
        }

        public async Task Create<TType>(TType entity) where TType : EntityBase
        {
            await Open<TType>(async x => await x.InsertOneAsync(entity));
        }

        public async Task<IList<TType>> Read<TType>()
        {
            List<TType> all = null;
            await Open<TType>(async collection => all = await collection.Find(Builders<TType>.Filter.Empty).ToListAsync());
            return all;
        }

        public async Task Delete<TType>(string id) where TType : EntityBase
        {
            await Open<TType>(async collection =>
            {
                var filter = Builders<TType>.Filter.Eq(s => s.Id, id);
                await collection.DeleteOneAsync(filter);
            });
        }

        public async Task DeleteDocuments<TType>()
        {
            await Open<TType>(async collection =>
            {
                await collection.DeleteManyAsync(Builders<TType>.Filter.Empty);
            });
        }

        public async Task<IList<TType>> Query<TType>(Func<IQueryable<TType>, IQueryable<TType>> action)
        {
            var collection = Database.GetCollection<TType>(GetName<TType>());
            var queryable = action(collection.AsQueryable()) as IAsyncCursorSource<TType>;
            return await queryable.ToListAsync();

        }

        public async Task Update<TType>(Action<TType> action, string id) where TType : EntityBase
        {
            await Open<TType>(async collection =>
            {
                var filter = Builders<TType>.Filter.Eq(s => s.Id, id);
                var findAsync = await collection.Find(filter).SingleOrDefaultAsync();
                action(findAsync);
                await collection.ReplaceOneAsync(filter, findAsync);
            });
        }
    }
}