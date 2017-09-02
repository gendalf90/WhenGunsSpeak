using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Legs
{
    public class StartAnimationCommand
    {
        public StartAnimationCommand(Guid guid, AnimationType type)
        {
            Guid = guid;
            Type = type;
        }

        public Guid Guid { get; private set; }

        public AnimationType Type { get; private set; }
    }
}