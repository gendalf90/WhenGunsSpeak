using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Legs
{
    public class AnimationCommand
    {
        public AnimationCommand(string session, AnimationType type)
        {
            Session = session;
            Type = type;
        }

        public string Session { get; private set; }

        public AnimationType Type { get; private set; }
    }
}