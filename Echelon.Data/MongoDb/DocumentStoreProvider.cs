using System;
using MongoDB.Driver;
using static System.Configuration.ConfigurationManager;

namespace Echelon.Data.MongoDb
{
    internal class DocumentStoreProvider
    {
        public static IMongoDatabase Database => DocumentStore.Value;

        private static readonly Lazy<IMongoDatabase> DocumentStore = new Lazy<IMongoDatabase>(CreateStore);

        private static IMongoDatabase CreateStore()
        {
            return
                new MongoClient(ConnectionStrings["MongoDbConnection"].ConnectionString).GetDatabase(
                    AppSettings["Database"]);
        }
    }
}