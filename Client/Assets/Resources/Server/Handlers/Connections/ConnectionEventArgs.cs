using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventArgs(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}
