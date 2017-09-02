using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Ground
{
    public class GroundEvent
    {
        public GroundEvent(Guid guid, bool status)
        {
            Guid = guid;
            IsGrounded = status;
        }

        public Guid Guid { get; private set; }

        public bool IsGrounded { get; private set; }
    }
}