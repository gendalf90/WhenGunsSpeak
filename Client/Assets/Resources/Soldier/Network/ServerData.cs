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
            Items = new Dictionary<string, ServerDataItem>();

            using (var reader = new BinaryDataReader(bytes))
            {
                var count = reader.ReadInt();

                for(int i = 0; i < count; i++)
                {
                    var item = new ServerDataItem
                    {
                        Session = reader.ReadString(),
                        LookAt = reader.ReadVector(),
                        IsMoveRight = reader.ReadBoolean(),
                        IsMoveLeft = reader.ReadBoolean(),
                        IsJump = reader.ReadBoolean(),
                        Position = reader.ReadVector()
                    };

                    Items[item.Session] = item;
                }
            }
        }

        public byte[] ToBytes()
        {
            var builder = new BinaryDataBuilder().Write(Items.Count);

            foreach(var item in Items.Values)
            {
                builder.Write(item.Session)
                       .Write(item.LookAt)
                       .Write(item.IsMoveRight)
                       .Write(item.IsMoveLeft)
                       .Write(item.IsJump)
                       .Write(item.Position);
            }

            return builder.Build();
        }
    }
}