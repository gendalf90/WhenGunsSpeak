using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Rooms : IPacket
    {
        public byte[] GetBytes()
        {
            var data = new { Action = "rooms" };
            return new BinaryDataBuilder().WriteAsJson(data).Build();
        }
    }
}
