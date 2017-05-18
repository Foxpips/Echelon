using System.Linq;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;
using Raven.Client;
using Raven.Client.Indexes;

namespace Echelon.Data.Indexes.Raven
{
    public class UserAvatarTransform : AbstractTransformerCreationTask<UserEntity>, IDbStartup
    {
        public UserAvatarTransform()
        {
            TransformResults = avatarEntities => avatarEntities.Select(userEntity => new
            {
                userEntity.Email,
                userEntity.UniqueIdentifier,
                userEntity.DisplayName,
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