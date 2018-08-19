using System;

namespace RoomsService.Logs
{
    interface IRoomLogger
    {
        void Information(Exception e, string message);

        void Information(string message);
    }
}
