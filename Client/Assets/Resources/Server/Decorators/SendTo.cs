using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class SendToDecorator : IPacket
    {
        private Guid from;
        private IEnumerable<Guid> to;
        private IPacket basePacket;

        public SendToDecorator(IPacket basePacket, Guid from, params Guid[] to)
            : this(basePacket, from, to.AsEnumerable())
        {
        }

        public SendToDecorator(IPacket basePacket, Guid from, IEnumerable<Guid> to)
        {
            this.from = from;
            this.basePacket = basePacket;
            this.to = to;
        }

        public byte[] GetBytes()
        {
            var sendTo = new { Action = "sendto", From = from.ToMinString(), To = to.Select(guid => guid.ToMinString()).ToArray() };
            return new BinaryDataBuilder().WriteAsJson(sendTo)
                                          .Write(basePacket.GetBytes())
                                          .Build();
        }
    }
}
