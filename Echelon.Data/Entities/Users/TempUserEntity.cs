using Echelon.Misc.Attributes;

namespace Echelon.Data.Entities.Users
{
    [Name("TempUsers")]
    public sealed class TempUserEntity : EntityBase
    {
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }
    }
}