using System;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Document;

namespace Echelon.Data.DataProviders.RavenDb
{
    internal class DocumentStoreProvider : BaseDbStoreProvider
    {
        public static IDocumentStore Database => DocumentStore.Value;

        private static readonly Lazy<IDocumentStore> DocumentStore = new Lazy<IDocumentStore>(CreateStore);

        private static IDocumentStore CreateStore()
        {
            var documentStore = new DocumentStore
            {
                ConnectionStringName = "RavenDbConnection",
                DefaultDatabase = ConfigurationManager.GetAppSetting("Database")
            }.Initialize();

            ConfigureDatabase(documentStore);
            return documentStore;
        }
    }
}