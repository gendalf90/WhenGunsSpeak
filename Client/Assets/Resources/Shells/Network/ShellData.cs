using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    [BinaryObjectType("D21F6730-E739-4E65-9297-F506CBD6BAC2")]
    class ShellData : IBinaryObject
    {
        public Guid Guid { get; set; }
        public Vector2 Position { get; set; }

        public void FromBytes(byte[] bytes)
        {
            using (var reader = new BinaryDataReader(bytes))
            {
                Guid = reader.ReadGuid();
                Position = reader.ReadVector();
            }
        }

        public byte[] ToBytes()
        {
            return new BinaryDataBuilder().Write(Guid)
                                          .Write(Position)
                                          .Build();
        }
    }
}