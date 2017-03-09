using System;
using MongoDB.Driver;

namespace Echelon.Data.MongoDb
{
    internal class DocumentStoreProvider
    {
        public static IMongoDatabase Database => DocumentStore.Value;

        private static readonly Lazy<IMongoDatabase> DocumentStore = new Lazy<IMongoDatabase>(CreateStore);

        private static IMongoDatabase CreateStore()
        {
            return new MongoClient("mongodb://localhost").GetDatabase("EchelonAppDb");
        }
    }
}