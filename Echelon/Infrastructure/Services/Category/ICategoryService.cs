using System.Threading.Tasks;
using Echelon.Entities.Categories;

namespace Echelon.Infrastructure.Services.Category
{
    public interface ICategoryService
    {
        Task<CategoriesEntity> GetCategories();
    }
}