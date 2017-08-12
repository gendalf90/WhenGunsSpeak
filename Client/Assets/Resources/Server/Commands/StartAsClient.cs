using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class StartAsClientCommand
    {
        public Guid Guid { get; private set; }

        public Guid ConnectTo { get; private set; }
    }
}
