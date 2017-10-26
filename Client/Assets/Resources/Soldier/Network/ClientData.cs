using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Server;

namespace Soldier.Network
{
    [BinaryObjectType("07931797-2F59-4A74-9123-4F15DACDC464")]
    class ClientData : IBinaryObject
    {
        public string Session { get; set; }

        public Vector2 LookAt { get; set; }

        public bool IsMoveRight { get; set; }

        public bool IsMoveLeft { get; set; }

        public bool IsJump { get; set; }

        public void FromBytes(byte[] bytes)
        {
            using (var reader = new BinaryDataReader(bytes))
            {
                Session = reader.ReadString();
                LookAt = reader.ReadVector();
                IsMoveRight = reader.ReadBoolean();
                IsMoveLeft = reader.ReadBoolean();
                IsJump = reader.ReadBoolean();
            }
        }

        public byte[] ToBytes()
        {
            return new BinaryDataBuilder().Write(Session)
                                          .Write(LookAt)
                                          .Write(IsMoveRight)
                                          .Write(IsMoveLeft)
                                          .Write(IsJump)
                                          .Build();
        }
    }
}