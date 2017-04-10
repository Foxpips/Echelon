using System.Linq;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;
using Raven.Client;
using Raven.Client.Indexes;

namespace Echelon.Data.Indexes.Raven
{
    public class UsersAvatarsTransform : AbstractTransformerCreationTask<UserEntity>, IDbStartup
    {
        public UsersAvatarsTransform()
        {
            TransformResults = avatarEntities => avatarEntities.Select(userEntity => new
            {
                userEntity.Email,
                userEntity.UserName,
                LoadDocument<AvatarEntity>(userEntity.AvatarId).AvatarUrl
            });
        }

        public void ExecuteInternal(object store)
        {
            var documentStore = store as IDocumentStore;
            if (documentStore != null)
            {
                Execute(documentStore);
            }
        }
    }
}