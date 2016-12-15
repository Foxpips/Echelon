﻿using System.Collections.Generic;
using Echelon.Misc.Attributes;

namespace Echelon.Core.Entities.Categories
{
    [Id("CategoriesTable")]
    public class CategoriesEntity
    {
        public IEnumerable<CategoryEntity> Categories { get; set; }

        public CategoriesEntity()
        {
            Categories = new List<CategoryEntity>();
        }
    }
}