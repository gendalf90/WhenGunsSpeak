using Microsoft.Extensions.Logging;
using System;

namespace RoomsService.Logs
{
    class RoomLogger : IRoomLogger
    {
        private readonly ILogger<RoomLogger> logger;

        public RoomLogger(ILogger<RoomLogger> logger)
        {
            this.logger = logger;
        }

        public void Information(Exception e, string message)
        {
            logger.LogInformation(e, message);
        }

        public void Information(string message)
        {
            logger.LogInformation(message);
        }
    }
}
