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
            return await _dataService.Single<CategoriesEntity>();
        }
    }
}