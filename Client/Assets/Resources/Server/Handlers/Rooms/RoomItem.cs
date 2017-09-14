using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class RoomItem
    {
        public RoomItem(string session, string description)
        {
            Session = session;
            Description = description;
        }

        public string Session { get; private set; }

        public string Description { get; private set; }
    }
}
