using System.Linq;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;
using Raven.Client.Indexes;

namespace Echelon.Data.Indexes
{
    public class UsersAvatars : AbstractTransformerCreationTask<AvatarEntity>
    {
        public UsersAvatars()
        {
            TransformResults = avatarEntities => avatarEntities.Select(avatar => new
            {
                avatar.Email,
                LoadDocument<UserEntity>(avatar.Email).UserName,
                avatar.AvatarUrl
            });
        }
    }

    public class AvatarUserEntity
    {
        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public string Email { get; set; }
    }
}