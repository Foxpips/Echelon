using System.Threading.Tasks;
using Echelon.Core.Logging.Loggers;
using Microsoft.Owin;

namespace Echelon.Infrastructure.Middleware
{
    public class CustomLoggingMiddleware : OwinMiddleware
    {
        private readonly IClientLogger _logger;

        public CustomLoggingMiddleware(OwinMiddleware next, IClientLogger logger) : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            _logger.Info($"{context.Request.Scheme} {context.Request.Method}: {context.Request.Path}");
            await Next.Invoke(context);
        }
    }
}