using System.Collections.Generic;
using System.Threading.Tasks;
using Echelon.Data.Entities.Categories;

namespace Echelon.Core.Infrastructure.Services.Category
{
    public interface ICategoryService : IService
    {
        Task<IList<CategoryEntity>> GetCategories();
    }
}