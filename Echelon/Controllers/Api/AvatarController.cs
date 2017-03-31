using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Echelon.Data;
using Echelon.Data.Entities.Avatar;
using Echelon.Data.Entities.Users;

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
            var loginEntities = await _dataservice.Read<AvatarEntity>();
            var user = loginEntities.Single(x => x.Email.Equals(email));

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
                var avatarEntities = await _dataservice.Query<AvatarEntity>(x => x.Where(z => z.Email.Equals(email)));
                var avatarEntity = avatarEntities.SingleOrDefault();

                if (avatarEntity != null)
                {
                    var item = new
                    {
                        url = avatarEntity.AvatarUrl ?? "https://localhost/Echelon/Content/Images/missing-image.png",
                        username = avatarEntity.Email
                    };

                    urllist.Add(item);
                }
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