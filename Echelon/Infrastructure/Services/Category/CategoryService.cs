using System.Threading.Tasks;
using Echelon.Core.Interfaces.Data;
using Echelon.Entities.Categories;

namespace Echelon.Infrastructure.Services.Category
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
            return await _dataService.Read<CategoriesEntity>();
        }
    }
}