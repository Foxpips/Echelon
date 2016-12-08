using System.Threading.Tasks;
using Echelon.Core.Interfaces.Data;
using Echelon.Objects.Entities.Categories;

namespace Echelon.Objects.Infrastructure.Services.Category
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