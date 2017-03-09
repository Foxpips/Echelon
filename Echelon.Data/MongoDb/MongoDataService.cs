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

        public async Task Create<TType>(TType entity)
        {
            await
                Database.GetCollection<TType>(entity.GetType().GetCustomAttribute<IdAttribute>().Id)
                    .InsertOneAsync(entity);
        }

        Task<TType> IDataService.Read<TType>()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TType>> ReadAll<TType>()
        {
            List<TType> all = null;
            await OpenConnection<TType>(async x => all = await x.Find(Builders<TType>.Filter.Empty).ToListAsync());
            return all;
        }

        public async Task<TType> Query<TType>(Expression<Func<TType, bool>> q)
        {
            IEnumerable<TType> z = null;
            await OpenConnection<TType>(async x => z = await x.Find(Builders<TType>.Filter.Where(q)).ToListAsync());
            return z.FirstOrDefault();
        }

        public async Task OpenConnection<TType>(Func<IMongoCollection<TType>, Task> action)
        {
            await action(Database.GetCollection<TType>(typeof(TType).GetCustomAttribute<IdAttribute>().Id));
        }

        public Task Update<TType>(Action<TType> action)
        {
            throw new NotImplementedException();
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