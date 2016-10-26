using System;
using System.Collections.Generic;
using System.Linq;
using Echelon.Core.Interfaces.Data;
using Raven.Client;

namespace Echelon.Core.Data.RavenDb
{
    public class DataService : IDataService
    {
        private readonly IDocumentStore _database = DocumentStoreProvider.Database;

        private void Open(Action<IDocumentSession> action)
        {
            using (var session = _database.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }

        public void Create<TType>(TType entity)
        {
            using (var session = _database.OpenSession())
            {
                session.Store(entity);
                session.SaveChanges();
            }
        }

        public IEnumerable<TType> Read<TType>(Func<TType, bool> query)
        {
            IEnumerable<TType> enumerable = null;
            Open(session => { enumerable = session.Query<TType>().Where(query).ToList(); });

            return enumerable;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete<TType>(string documentName, Func<TType, bool> query)
        {
            Open(session =>
            {
                var activeDocuments = session.Advanced.LoadStartingWith<TType>(documentName, "*", 0, 128);
                var documentsToDelete = activeDocuments.Where(query);

                foreach (var type in documentsToDelete)
                {
                    var documentId = session.Advanced.GetDocumentId(type);
                    session.Delete(documentId);
                }
            });
        }
    }
}