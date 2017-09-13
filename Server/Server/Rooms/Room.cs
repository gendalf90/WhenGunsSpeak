using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Rooms
{
    class Room
    {
        public Room(string session, string description)
        {
            Session = session;
            Description = description;
        }

        public string Session { get; private set; }

        public string Description { get; private set; }
    }
}
