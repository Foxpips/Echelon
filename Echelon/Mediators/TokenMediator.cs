using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Echelon.Data;
using Echelon.Data.Entities.Transforms;
using Echelon.Data.Entities.Users;
using Microsoft.Owin;

namespace Echelon.Mediators
{
    public class TokenMediator : IMediator
    {
        private readonly IDataService _dataservice;
        private readonly IOwinContext _owinContext;

        public TokenMediator(IDataService dataservice, IOwinContext owinContext)
        {
            _owinContext = owinContext;
            _dataservice = dataservice;
        }

        public async Task<object> CreateToken(string device, string channel)
        {
            // Create a random identity for the client
            var user = _owinContext.Authentication.User;
            var userAvatar = await _dataservice.TransformUserAvatars<UserAvatarEntity>(user.Identity.Name);
            var identity = new { username = userAvatar.DisplayName, uniqueId = userAvatar.UniqueIdentifier, avatarUrl = userAvatar.AvatarUrl };

            return new
            {
                identity
            };
        }
    }
}