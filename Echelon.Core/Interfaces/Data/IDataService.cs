using System;
using System.Collections.Generic;

namespace Echelon.Core.Interfaces.Data
{
    public interface IDataService
    {
        void Create<TType>(TType entity);
        IEnumerable<TType> Read<TType>(Func<TType, bool> query);
        void Update();
        void Delete<TType>(string documentName, Func<TType, bool> query);
    }
}