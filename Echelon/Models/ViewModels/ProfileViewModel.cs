using System.ComponentModel.DataAnnotations;
using System.Web;
using Echelon.Misc.Attributes;

namespace Echelon.Models.ViewModels
{
    public class ProfileViewModel
    {
        [FileSize(250000)]
        [FileTypes(FileTypes.Jpeg, FileTypes.Jpg, FileTypes.Png, FileTypes.Gif)]
        public HttpPostedFileBase File { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string DisplayName { get; set; }
    }
}