using System;

namespace Server
{
    class AboutMeEvent
    {
        public AboutMeEvent(Guid myId)
        {
            MyId = myId;
        }

        public Guid MyId { get; private set; }
    }
}
