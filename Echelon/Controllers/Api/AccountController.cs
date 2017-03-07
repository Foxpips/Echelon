using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
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

        [HttpGet]
        [Authorize]
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

        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> PrintPerson(EmailList list)
        {
            Console.WriteLine(@"test");
            var urllist = new List<object>();
            foreach (var email in list.Emails)
            {
                var loginEntities = await _dataservice.Read<UsersEntity>();

                var userEntity = loginEntities.Users.Single(x => x.Email == email);
                var item = new
                {
                    url = userEntity.AvatarUrl,
                    email = userEntity.UserName
                };

                urllist.Add(item);
            }

            if (!urllist.Any())
            {
                return NotFound();
            }
            return Ok(urllist);
        }
    }

    public class EmailList
    {
        public List<string> Emails { get; set; }
    }
}