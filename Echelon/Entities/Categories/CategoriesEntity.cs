using System.Collections.Generic;

namespace Echelon.Entities.Categories
{
    public class CategoriesEntity
    {
        public IEnumerable<CategoryEntity> Categories { get; set; }

        public CategoriesEntity()
        {
            Categories = new List<CategoryEntity>();
        }
    }
}