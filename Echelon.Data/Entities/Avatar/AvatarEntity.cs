using Echelon.Misc.Enums;

namespace Echelon.Data.Entities.Avatar
{
    public class AvatarEntity : EntityBase
    {
        public string ImageName => "Avatar";

        public FileTypeEnum FileType { get; set; }

        public string AvatarUrl { get; set; }
    }
}