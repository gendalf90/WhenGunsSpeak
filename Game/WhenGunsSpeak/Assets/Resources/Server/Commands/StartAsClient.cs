using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class StartAsClientCommand
    {
        public StartAsClientCommand(string connectToSession)
        {
            ConnectToSession = connectToSession;
        }

        public string ConnectToSession { get; private set; }
    }
}
