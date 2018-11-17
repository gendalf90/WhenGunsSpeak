using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    public static class IPacketExtension
    {
        public static IPacket ToPacket(this byte[] bytes)
        {
            return new InternalBytesPacket(bytes);
        }

        public static BinaryDataReader ToReader(this IPacket packet)
        {
            return new BinaryDataReader(packet.GetBytes());
        }

        public static IPacket BuildPacket(this BinaryDataBuilder builder)
        {
            return builder.Build().ToPacket();
        }

        public static T AsBinaryObject<T>(this IPacket packet) where T : class, IBinaryObject, new()
        {
            return packet.GetBytes().AsBinaryObject<T>();
        }

        public static IEnumerable<T> OfBinaryObjects<T>(this IEnumerable<IPacket> packets) where T : class, IBinaryObject, new()
        {
            return packets.Select(packet => packet.AsBinaryObject<T>()).Where(result => result != null);
        }

        public static IPacket CreateBinaryObjectPacket<T>(this T data) where T : class, IBinaryObject, new()
        {
            return new InternalBinaryObjectPacket<T>(data);
        }

        class InternalBytesPacket : IPacket
        {
            private readonly byte[] bytes;

            public InternalBytesPacket(byte[] bytes)
            {
                this.bytes = bytes;
            }

            public byte[] GetBytes()
            {
                return (byte[])bytes.Clone();
            }
        }

        class InternalBinaryObjectPacket<T> : IPacket where T : class, IBinaryObject, new()
        {
            private readonly T binaryObject;

            public InternalBinaryObjectPacket(T binaryObject)
            {
                this.binaryObject = binaryObject;
            }

            public byte[] GetBytes()
            {
                return binaryObject.CreateBinaryObjectBytes();
            }
        }
    }
}