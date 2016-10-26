using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;

namespace Echelon.Core.Data
{
    public class DataService
    {
        private DocumentStore _database;
        private readonly string _dataconnection = "http://localhost:8080/";

        public DataService()
        {
            Connect();
        }

        private void Connect()
        {
            _database = new DocumentStore
            {
                Url = _dataconnection,
                DefaultDatabase = "Echelon"
            };

            _database.Initialize();
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

        private void Open(Action<IDocumentSession> action)
        {
            using (var session = _database.OpenSession())
            {
                action(session);
                session.SaveChanges();
            }
        }
    }
}