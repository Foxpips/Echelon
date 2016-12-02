using System.Collections.Generic;

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