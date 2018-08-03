using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Data.Entities;

namespace Echelon.Data
{
    public interface IDataService
    {
        Task Create<TType>(TType entity) where TType : EntityBase;
        Task<IList<TType>> Read<TType>();
        Task<TType> Load<TType>(string id);
        Task Update<TType>(Action<TType> action, string id) where TType : EntityBase;
        Task Delete<TType>(string id) where TType : EntityBase;
        Task<IList<TType>> Query<TType>(Func<IQueryable<TType>, IQueryable<TType>> action);
        Task<TType> Single<TType>(Func<IQueryable<TType>, IQueryable<TType>> action);
        Task DeleteDocuments<TType>();
        Task<TType> TransformUserAvatars<TType>(string id);
        Task<bool> Exists<TType>(string id);
    }
}