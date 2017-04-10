namespace Echelon.Data.Entities.Avatar
{
    public class AvatarEntity : EntityBase
    {
        public string ImageName => "Avatar";

        public string FileType { get; set; }

        public string AvatarUrl { get; set; }
    }
}