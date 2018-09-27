using System;

namespace RoomsService.Logs
{
    public interface IRoomLogger
    {
        void Information(Exception e, string message);

        void Information(string message);
    }
}
