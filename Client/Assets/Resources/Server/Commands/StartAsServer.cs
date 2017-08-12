using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class StartAsServerCommand
    {
        public StartAsServerCommand(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}
