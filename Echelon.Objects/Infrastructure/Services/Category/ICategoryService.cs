using System.Threading.Tasks;
using Echelon.Objects.Entities.Categories;

namespace Echelon.Objects.Infrastructure.Services.Category
{
    public interface ICategoryService
    {
        Task<CategoriesEntity> GetCategories();
    }
}