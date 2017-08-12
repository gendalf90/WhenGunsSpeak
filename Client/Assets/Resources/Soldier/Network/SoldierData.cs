using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Soldier.Network
{
    [BinaryObjectType("D708B477-F902-4988-814D-BBD82FCD185D")]
    class SoldierData : IBinaryObject
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