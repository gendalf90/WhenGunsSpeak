using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Session : IPacket
    {
        public byte[] GetBytes()
        {
            var ping = new { Action = "session" };
            return new BinaryDataBuilder().WriteAsJson(ping).Build();
        }
    }
}
