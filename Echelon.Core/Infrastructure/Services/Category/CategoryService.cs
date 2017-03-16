using System.Linq;
using System.Threading.Tasks;
using Echelon.Core.Entities.Categories;
using Echelon.Data;

namespace Echelon.Core.Infrastructure.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IDataService _dataService;

        public CategoryService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<CategoriesEntity> GetCategories()
        {
            var categoriesEntities = await _dataService.Read<CategoriesEntity>();
            return categoriesEntities.Single();
        }
    }
}