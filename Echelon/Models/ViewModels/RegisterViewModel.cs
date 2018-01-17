using System.ComponentModel.DataAnnotations;

namespace Echelon.Models.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string DisplayName { get; set; }
    }
}