namespace Echelon.Models.ViewModels
{
    public class Category : ICategory
    {
        public string Name { get; set; }
    }

    public interface ICategory
    {
        string Name { get; set; }
    }
}