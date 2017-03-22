using System.Threading.Tasks;
using Echelon.Data.Entities.Categories;

namespace Echelon.Core.Infrastructure.Services.Category
{
    public interface ICategoryService : IService
    {
        Task<CategoriesEntity> GetCategories();
    }
}