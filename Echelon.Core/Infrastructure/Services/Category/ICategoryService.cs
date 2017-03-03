using System.Threading.Tasks;
using Echelon.Core.Entities.Categories;

namespace Echelon.Core.Infrastructure.Services.Category
{
    public interface ICategoryService : IService
    {
        Task<CategoriesEntity> GetCategories();
    }
}