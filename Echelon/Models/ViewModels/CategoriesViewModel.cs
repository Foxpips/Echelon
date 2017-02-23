using System.Collections.Generic;
using Echelon.Models.BusinessModels;

namespace Echelon.Models.ViewModels
{
    public class CategoriesViewModel : ICategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }

    public interface ICategoriesViewModel
    {
        IEnumerable<Category> Categories { get; set; }
    }
}