using System;
using System.Collections.Generic;
using System.Linq;
using Echelon.Data.DataProviders.MongoDb;

namespace Echelon.Data.DataProviders
{
    internal class BaseDbStoreProvider
    {
        protected static void ConfigureDatabase<TDbType>(TDbType documentStore)
        {
            var startupType = typeof(IDbStartup);

            var startupTypes = typeof(DocumentStoreProvider).Assembly.GetExportedTypes()
                .Where(type => startupType.IsAssignableFrom(type)).ToList();

            foreach (var type in startupTypes.Except(new List<Type> { startupType }))
            {
                var instance = Activator.CreateInstance(type) as IDbStartup;
                instance?.ExecuteInternal(documentStore);
            }
        }
    }
}