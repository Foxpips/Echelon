using System;
using System.Configuration;
using MongoDB.Driver;

namespace Echelon.Data.DataProviders.MongoDb
{
    internal class DocumentStoreProvider : BaseDbStoreProvider
    {
        public static IMongoDatabase Database => DocumentStore.Value;

        private static readonly Lazy<IMongoDatabase> DocumentStore = new Lazy<IMongoDatabase>(CreateStore);

        private static IMongoDatabase CreateStore()
        {
            var mongoDatabase =
                new MongoClient(ConfigurationManager.ConnectionStrings["MongoDbConnection"].ConnectionString)
                    .GetDatabase(
                        ConfigurationManager.AppSettings["Database"]);

            ConfigureDatabase(mongoDatabase);
            return mongoDatabase;
        }
    }
}