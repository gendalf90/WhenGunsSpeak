using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Ground
{
    public class GroundEvent
    {
        public GroundEvent(string session, bool status)
        {
            Session = session;
            IsGrounded = status;
        }

        public string Session { get; private set; }

        public bool IsGrounded { get; private set; }
    }
}