using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Soldier.Network
{
    [BinaryObjectType("07931797-2F59-4A74-9123-4F15DACDC464")]
    class PlayerData : IBinaryObject
    {
        public Guid Guid { get; set; }

        public void FromBytes(byte[] bytes)
        {

        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}