using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Room : IPacket
    {
        public Room(string session, string description)
        {
            Session = session;
            Description = description;
        }

        public string Session { get; private set; }

        public string Description { get; private set; }

        public byte[] GetBytes()
        {
            var registration = new { Action = "room", Session = Session, Description = Description };
            return new BinaryDataBuilder().WriteAsJson(registration).Build();
        }
    }
}
