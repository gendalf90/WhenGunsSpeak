using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Ping : IPacket
    {
        public Ping(Guid initiator, Guid responder)
        {
            Initiator = initiator;
            Responder = responder;
        }

        public Guid Initiator { get; private set; }

        public Guid Responder { get; private set; }

        public byte[] GetBytes()
        {
            var ping = new { Action = "ping", Initiator = Initiator.ToMinString(), Responder = Responder.ToMinString() };
            return new BinaryDataBuilder().WriteAsJson(ping).Build();
        }
    }
}
