using System;
using Raven.Client;
using Raven.Client.Document;
using static Raven.Abstractions.Util.ConfigurationManager;

namespace Echelon.Data.RavenDb
{
    internal class DocumentStoreProvider
    {
        public static IDocumentStore Database => DocumentStore.Value;

        private static readonly Lazy<IDocumentStore> DocumentStore = new Lazy<IDocumentStore>(CreateStore);

        private static IDocumentStore CreateStore()
        {
            return new DocumentStore
            {
                ConnectionStringName = "RavenDbConnection",
                DefaultDatabase = GetAppSetting("Database")
            }.Initialize();
        }
    }
}