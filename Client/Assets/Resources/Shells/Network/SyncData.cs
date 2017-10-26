using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells.Network
{
    class ShellData
    {
        public string Type { get; set; }

        public Guid Guid { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }

    [BinaryObjectType("359d0ebf-1b28-49bb-b96d-909c87fbafe0")]
    class SyncData : IBinaryObject
    {
        public ShellData[] Shells { get; set; }

        public void FromBytes(byte[] bytes)
        {
            using(var reader = new BinaryDataReader(bytes))
            {
                Shells = new ShellData[reader.ReadInt()];

                for(int i = 0; i < Shells.Length; i++)
                {
                    Shells[i] = new ShellData
                    {
                        Type = reader.ReadString(),
                        Guid = reader.ReadGuid(),
                        Position = reader.ReadVector(),
                        Rotation = reader.ReadFloat()
                    };
                }
            }
        }

        public byte[] ToBytes()
        {
            var builder = new BinaryDataBuilder().Write(Shells.Length);

            foreach(var data in Shells)
            {
                builder.Write(data.Type)
                       .Write(data.Guid)
                       .Write(data.Position)
                       .Write(data.Rotation);
            }

            return builder.Build();
        }
    }
}