using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Soldier.Network
{
    class ServerDataItem
    {
        public string Session { get; set; }

        public Vector2 LookAt { get; set; }

        public bool IsMoveRight { get; set; }

        public bool IsMoveLeft { get; set; }

        public bool IsJump { get; set; }

        public Vector2 Position { get; set; }
    }

    [BinaryObjectType("D708B477-F902-4988-814D-BBD82FCD185D")]
    class ServerData : IBinaryObject
    {
        public Dictionary<string, ServerDataItem> Items { get; set; }

        public void FromBytes(byte[] bytes)
        {
            
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }
}