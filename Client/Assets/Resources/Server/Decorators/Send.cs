using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class SendDecorator : IPacket
    {
        private string fromSession;
        private IEnumerable<string> toSessions;
        private IPacket basePacket;

        public SendDecorator(IPacket basePacket, string fromSession, params string[] toSessions)
            : this(basePacket, fromSession, toSessions.AsEnumerable())
        {
        }

        public SendDecorator(IPacket basePacket, string fromSession, IEnumerable<string> toSessions)
        {
            this.fromSession = fromSession;
            this.basePacket = basePacket;
            this.toSessions = toSessions;
        }

        public byte[] GetBytes()
        {
            var sendTo = new { Action = "send", From = fromSession, To = toSessions.ToArray() };
            return new BinaryDataBuilder().WriteAsJson(sendTo)
                                          .Write(basePacket.GetBytes())
                                          .Build();
        }
    }
}
