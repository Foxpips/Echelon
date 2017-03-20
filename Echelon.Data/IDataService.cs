using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echelon.Data
{
    public interface IDataService
    {
        Task Create<TType>(TType entity) where TType : EntityBase;
        Task<IList<TType>> Read<TType>();
        Task Update<TType>(Action<TType> action, string id) where TType : EntityBase;
        Task Delete<TType>(string id) where TType : EntityBase;
        Task<IList<TType>> Query<TType>(Func<IQueryable<TType>, IQueryable<TType>> action);
        Task DeleteDocuments<TType>();
    }
}