﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Echelon.Infrastructure.Attributes
{
    public class RequireApiHttpsAttribute : IAuthenticationFilter
    {
        public bool AllowMultiple => true;

        public Task AuthenticateAsync(HttpAuthenticationContext context,
            CancellationToken cancellationToken)
        {
            if (context.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                context.ActionContext.Response = new HttpResponseMessage(
                    HttpStatusCode.Forbidden);
            }
            return Task.FromResult<object>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}