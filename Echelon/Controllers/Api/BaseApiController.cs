using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

namespace Echelon.Controllers.Api
{
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
}