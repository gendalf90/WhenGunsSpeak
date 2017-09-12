using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Connection
    {
        private SimpleTimer timeoutTimer;

        public string Session { get; private set; }

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

        public static Connection StartConnection(string session, float timeoutSeconds)
        {
            return new Connection
            {
                Session = session,
                timeoutTimer = SimpleTimer.StartNew(timeoutSeconds)
            };
        }
    }
}
