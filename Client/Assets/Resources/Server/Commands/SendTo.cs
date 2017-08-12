using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Server
{
    public class SendToCommand
    {
        public SendToCommand(IPacket data, IEnumerable<Guid> to)
        {
            To = to;
            Data = data;
        }

        public SendToCommand(IPacket data, params Guid[] to)
            : this(data, to.AsEnumerable())
        {
        }

        public IEnumerable<Guid> To { get; private set; }

        public IPacket Data { get; private set; }
    }
}
