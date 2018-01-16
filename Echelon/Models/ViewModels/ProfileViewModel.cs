using System.ComponentModel.DataAnnotations;
using System.Web;
using Echelon.Misc.Attributes;
using Echelon.Misc.Enums;

namespace Echelon.Models.ViewModels
{
    public class ProfileViewModel
    {
        [FileSize(250000)]
        [FileTypes(FileTypeEnum.Jpeg, FileTypeEnum.Jpg, FileTypeEnum.Png, FileTypeEnum.Gif)]
        public HttpPostedFileBase File { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }
    }
}