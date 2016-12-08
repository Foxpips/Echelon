using System.Collections.Generic;

namespace Echelon.Objects.Entities.Categories
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