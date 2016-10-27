using System;
using System.Threading.Tasks;

namespace Echelon.Core.Interfaces.Data
{
    public interface IDataService
    {
        Task Create<TType>(TType entity);
        Task<TType> Read<TType>();
        Task Update<TType>(Action<TType> action);
        Task Delete<TType>();
    }
}