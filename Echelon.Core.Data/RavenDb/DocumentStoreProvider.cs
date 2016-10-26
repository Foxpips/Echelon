using System;
using Raven.Abstractions.Util;
using Raven.Client;
using Raven.Client.Document;

namespace Echelon.Core.Data.RavenDb
{
    internal class DocumentStoreProvider
    {
        public static IDocumentStore Database => DocumentStore.Value;

        private static readonly Lazy<IDocumentStore> DocumentStore = new Lazy<IDocumentStore>(CreateStore);

        private static IDocumentStore CreateStore()
        {
            return new DocumentStore
            {
                ConnectionStringName = "DatabaseConnection",
                DefaultDatabase = ConfigurationManager.GetAppSetting("DefaultDatabase")
            }.Initialize();
        }
    }
}