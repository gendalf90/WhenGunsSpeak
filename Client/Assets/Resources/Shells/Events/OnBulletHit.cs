using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    //public enum Type
    //{
    //    Standard,
    //    Expansive,
    //    AntiArmor
    //}

    public class OnBulletHitEvent
    {
        public OnBulletHitEvent(Guid guid, int targetInstanceId)
        {
            Guid = guid;
            TargetInstanceId = targetInstanceId;
        }

        public Guid Guid { get; private set; }

        public int TargetInstanceId { get; private set; }

        //public float Energy { get; private set; }
    }
}