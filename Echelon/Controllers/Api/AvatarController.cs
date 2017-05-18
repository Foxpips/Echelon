using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Echelon.Data;
using Echelon.Data.Entities.Transforms;

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
            var user = await _dataservice.TransformUserAvatars<UserAvatarEntity>(email);

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
            var urllist = new List<object>();
            foreach (var email in list.Emails)
            {
                var avatarEntity = await _dataservice.TransformUserAvatars<UserAvatarEntity>(email);

                if (avatarEntity != null)
                {
                    var item = new
                    {
                        url = avatarEntity.AvatarUrl ?? "https://localhost/Echelon/Content/Images/missing-image.png",
                        username = avatarEntity.DisplayName
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