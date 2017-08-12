using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ReceivePingEventArgs : EventArgs
    {
        public ReceivePingEventArgs(Guid from)
        {
            From = from;
        }

        public Guid From { get; private set; }
    }
}
