using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Echelon.Misc.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Echelon.Data.MongoDb
{
    public class MongoDataService : IDataService
    {
        protected static IMongoDatabase Database = DocumentStoreProvider.Database;

        private static async Task Open<TType>(Func<IMongoCollection<TType>, Task> action)
        {
            var c = await Database.ListCollectionsAsync();
            var b = await c.AnyAsync();

            await action(Database.GetCollection<TType>(typeof(TType).GetCustomAttribute<IdAttribute>().Id));
        }

        public async Task Create<TType>(TType entity)
        {
            await Open<TType>(async x => await x.InsertOneAsync(entity));
        }

        public async Task<TType> Read<TType>()
        {
            var types = await ReadAll<TType>();
            return types.SingleOrDefault();
        }

        public async Task<List<TType>> ReadAll<TType>()
        {
            List<TType> all = null;
            await Open<TType>(async collection => all = await collection.Find(Builders<TType>.Filter.Empty).ToListAsync());
            return all;
        }

        public async Task<IEnumerable<TType>> Query<TType>(Expression<Func<TType, bool>> q)
        {
            IEnumerable<TType> query = null;
            await Open<TType>(async collection => query = await collection.Find(Builders<TType>.Filter.Where(q)).ToListAsync());
            return query;
        }

        public async Task Update<TType>(Action<TType> action)
        {
            var read = await Read<TType>();
            action(read);

            await Open<TType>(
                    async collection =>
                    {
                        await collection.ReplaceOneAsync(type => type.Equals(read), read, new UpdateOptions { IsUpsert = true });
                    });
        }

        public Task Delete<TType>()
        {
            throw new NotImplementedException();
        }
    }

    [Id("Users")]
    [BsonIgnoreExtraElements]
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}