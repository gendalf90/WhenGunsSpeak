using BinaryProcessing;
using MessagePack;
using System;

namespace Utils
{
    public static class MessagePackExtensions
    {
        private const int TypeMarkerSize = 16;
        private static readonly Guid TypeMarker = new Guid("852B4665-C90D-4A45-87CE-2667CECAE375");

        public static byte[] SerializeByMessagePack<T>(this T value) where T: class
        {
            var typeMarkerSource = new Binary(TypeMarker.ToByteArray());
            var objectSource = MessagePackSerializer.Serialize(value);
            var result = typeMarkerSource + objectSource;
            return result.ToBytes();
        }

        public static T DeserializeByMessagePack<T>(this byte[] bytes) where T: class
        {
            if(bytes.Length <= TypeMarkerSize)
            {
                return null;
            }

            var binary = new Binary(bytes);
            var typeMarkerSource = binary * TypeMarkerSize;
            var isItMessagePack = typeMarkerSource == TypeMarker.ToByteArray();
            
            if(!isItMessagePack)
            {
                return null;
            }

            var objectSource = binary / TypeMarkerSize;
            return MessagePackSerializer.Deserialize<T>(objectSource.ToBytes());
        }
    }
}
