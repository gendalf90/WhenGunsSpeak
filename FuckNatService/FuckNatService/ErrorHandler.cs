using Datagrammer;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FuckNatService
{
    class ErrorHandler : IErrorHandler
    {
        private readonly ILogger<ErrorHandler> logger;

        public ErrorHandler(ILogger<ErrorHandler> logger)
        {
            this.logger = logger;
        }

        public Task HandleAsync(IContext context, Exception e)
        {
            logger.LogError(e, "Request error");
            return Task.CompletedTask;
        }
    }
}
