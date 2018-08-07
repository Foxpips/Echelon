using System.Web;
using Echelon.Misc.Attributes;
using Echelon.Misc.Enums;

namespace Echelon.Models.ViewModels
{
    public class AvatarViewModel
    {
        [FileSize(250000)]
        [FileTypes(FileTypeEnum.Jpeg, FileTypeEnum.Jpg, FileTypeEnum.Png, FileTypeEnum.Gif)]
        public HttpPostedFileBase File { get; set; }
    }
}