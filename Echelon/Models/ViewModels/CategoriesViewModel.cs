using System.Collections.Generic;
using Echelon.Models.BusinessModels;

namespace Echelon.Models.ViewModels
{
    public class CategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}