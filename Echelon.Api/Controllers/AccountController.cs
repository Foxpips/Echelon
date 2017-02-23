using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Echelon.Core.Entities.Users;
using Echelon.Data;
using Echelon.Data.RavenDb;

namespace Echelon.Api.Controllers
{
    public class AvatarController : ApiController
    {
        private readonly IDataService _dataservice = new DataService();

        //public AccountController(IDataService dataservice)
        //{
        //    _dataservice = dataservice;
        //}

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