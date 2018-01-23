using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using Echelon.Core.Logging.Loggers;
using Echelon.Data;
using Echelon.Data.Entities.Transforms;

namespace Echelon.Controllers.Api
{
    public class AvatarController : BaseApiController
    {
        private readonly IDataService _dataservice;
        private IClientLogger _logger;

        public AvatarController(IDataService dataservice, IClientLogger logger)
        {
            _logger = logger;
            _dataservice = dataservice;
        }

        [HttpGet]
        [Authorize]
        public async Task<IHttpActionResult> GetAvatar(string email)
        {
            var user = await _dataservice.TransformUserAvatars<UserAvatarEntity>(email);

            throw new StackOverflowException();

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

    public class BaseApiController : ApiController
    {
        [Route("HttpError")]
        [HttpGet]
        public HttpResponseMessage HttpError()
        {
            return Request.CreateResponse(HttpStatusCode.Forbidden, "You cannot access this method at this time.");
        }

        protected override BadRequestResult BadRequest()
        {
            
            return base.BadRequest();
        }

        protected override BadRequestErrorMessageResult BadRequest(string message)
        {
            return base.BadRequest(message);
        }

        protected override InvalidModelStateResult BadRequest(ModelStateDictionary modelState)
        {
            return base.BadRequest(modelState);
        }

        protected override InternalServerErrorResult InternalServerError()
        {
            return base.InternalServerError();
        }

        protected override ExceptionResult InternalServerError(Exception exception)
        {
            return base.InternalServerError(exception);
        }

        protected override NotFoundResult NotFound()
        {
            return base.NotFound();
        }
    }

    public class EmailList
    {
        public List<string> Emails { get; set; }
    }
}