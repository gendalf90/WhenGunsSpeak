using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Connection
    {
        private SimpleTimer timeoutTimer;

        public Guid Guid { get; private set; }

        public bool IsTimeout
        {
            get
            {
                return timeoutTimer.ItIsTime;
            }
        }

        public void Ping()
        {
            timeoutTimer.Restart();
        }

        public static Connection StartConnection(Guid guid, float timeoutSeconds)
        {
            return new Connection
            {
                Guid = guid,
                timeoutTimer = SimpleTimer.StartNew(timeoutSeconds)
            };
        }
    }
}
