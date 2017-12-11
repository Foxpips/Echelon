using System.Collections.Generic;
using System.Threading.Tasks;
using Echelon.Data;
using Echelon.Data.Entities.Categories;

namespace Echelon.Core.Infrastructure.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IDataService _dataService;

        public CategoryService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IList<CategoryEntity>> GetCategories()
        {
            var categoriesEntities = await _dataService.Read<CategoryEntity>();
            return categoriesEntities;
        }
    }
}