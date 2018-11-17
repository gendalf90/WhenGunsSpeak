using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class SendToServerCommand
    {
        public SendToServerCommand(IPacket data)
        {
            Data = data;
        }

        public IPacket Data { get; private set; }
    }
}
