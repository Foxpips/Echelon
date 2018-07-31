using System.ComponentModel.DataAnnotations;

namespace Echelon.Models.ViewModels
{
    public class ForgottenPasswordModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}