using System.Threading.Tasks;
using Raven.Client.Indexes;

namespace Echelon.Data
{
    public interface IRavenDataService : IDataService
    {
        Task<TTransformedType> GetIndex<TTransformer, TTransformedType>(string id)
            where TTransformer : AbstractTransformerCreationTask, new();
    }
}