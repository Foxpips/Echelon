namespace Echelon.Models.BusinessModels
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