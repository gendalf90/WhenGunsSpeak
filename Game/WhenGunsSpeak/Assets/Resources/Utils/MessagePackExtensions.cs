using MessagePack;
using System;

namespace Utils
{
    public static class MessagePackExtensions
    {
        private static readonly Guid TypeMarker = new Guid("852B4665-C90D-4A45-87CE-2667CECAE375");

        public static byte[] SerializeByMessagePack<T>(this T value) where T: class
        {
            var objectSource = MessagePackSerializer.Serialize(value);
            return objectSource.AddGuidAtBegin(TypeMarker);
        }

        public static T DeserializeByMessagePack<T>(this byte[] bytes) where T: class
        {
            byte[] body;

            if(bytes.TryGetNotEmptyBodyIfGuidAtBeginIs(TypeMarker, out body))
            {
                return MessagePackSerializer.Deserialize<T>(body);
            }

            return null;
        }
    }
}
