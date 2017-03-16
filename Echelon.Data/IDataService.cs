using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echelon.Data
{
    public interface IDataService
    {
        Task Create<TType>(TType entity) where TType : EntityBase;
        Task<List<TType>> Read<TType>();
        Task Update<TType>(Action<TType> action, string id);
        Task Delete<TType>(string id);
        Task<IList<TType>> Query<TType>(Func<IQueryable<TType>, IQueryable<TType>> action);
    }
}