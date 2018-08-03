using Echelon.Misc.Attributes;

namespace Echelon.Data.Entities.Users
{
    [Name("ResetUsers")]
    public sealed class ResetPasswordEntity : EntityBase
    {
        public string Email { get; set; }
    }
}