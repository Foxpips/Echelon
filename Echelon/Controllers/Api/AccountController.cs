using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Echelon.Core.Entities.Users;
using Echelon.Data;

namespace Echelon.Controllers.Api
{
    public class AvatarController : ApiController
    {
        private readonly IDataService _dataservice;

        public AvatarController(IDataService dataservice)
        {
            _dataservice = dataservice;
        }

        [System.Web.Mvc.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> GetAvatar(string email)
        {

            var loginEntities = await _dataservice.Read<UsersEntity>();
            var user = loginEntities.Users.Single(x => x.Email.Equals(email));

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.AvatarUrl);
        }
    }
}