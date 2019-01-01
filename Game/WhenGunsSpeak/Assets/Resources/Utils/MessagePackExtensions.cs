using BinaryProcessing;
using MessagePack;
using System;

namespace Utils
{
    public static class MessagePackExtensions
    {
        private static readonly Guid TypeMarker = Guid.Parse("852B4665-C90D-4A45-87CE-2667CECAE375");

        public static Binary SerializeByMessagePack<T>(this T value) where T: class
        {
            var markerBytes = new Binary(TypeMarker.ToByteArray());
            return markerBytes + MessagePackSerializer.Serialize(value);
        }

        public static T DeserializeByMessagePack<T>(this Binary bytes) where T: class
        {
            Binary body;

            if(bytes.TryGetNotEmptyBodyIfGuidAtBeginIs(TypeMarker, out body))
            {
                return MessagePackSerializer.Deserialize<T>(body.ToBytes());
            }

            return null;
        }
    }
}
