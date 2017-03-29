namespace Echelon.Data.Entities.Avatar
{
    public class AvatarEntity : EntityBase
    {
        public string Email { get; set; }

        public byte[] ImageBytes { get; set; }

        public string ImageName { get; set; }

        public string FileType { get; set; }
    }
}