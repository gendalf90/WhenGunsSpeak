using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnReceiveFromClientEvent
    {
        public OnReceiveFromClientEvent(string session, IEnumerable<IPacket> data)
        {
            Data = data;
            Session = session;
        }

        public string Session { get; private set; }

        public IEnumerable<IPacket> Data { get; private set; }
    }
}