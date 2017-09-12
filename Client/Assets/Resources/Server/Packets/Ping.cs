using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Ping : IPacket
    {
        public Ping(string fromSession, string toSession)
        {
            FromSession = fromSession;
            ToSession = toSession;
        }

        public string FromSession { get; private set; }

        public string ToSession { get; private set; }

        public byte[] GetBytes()
        {
            var ping = new { Action = "ping", From = FromSession, To = ToSession };
            return new BinaryDataBuilder().WriteAsJson(ping).Build();
        }
    }
}
