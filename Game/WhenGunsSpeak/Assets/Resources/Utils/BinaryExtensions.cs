using BinaryProcessing;
using System;

namespace Utils
{
    public static class BinaryExtensions
    {
        private const int GuidLengthInBytes = 16;

        public static bool HasGuidAtBegin(this Binary source, Guid value)
        {
            if(source.Length < GuidLengthInBytes)
            {
                return false;
            }

            var sourceGuidBytes = source * GuidLengthInBytes;
            var valueGuidBytes = value.ToByteArray();
            return sourceGuidBytes == valueGuidBytes;
        }

        public static Binary AddGuidAtBegin(this Binary body, Guid value)
        {
            return value.ToByteArray() + body;
        }

        public static bool TryGetNotEmptyBodyIfGuidAtBeginIs(this Binary source, Guid value, out Binary result)
        {
            result = null;

            if(!source.HasGuidAtBegin(value))
            {
                return false;
            }

            var body = source / GuidLengthInBytes;

            if(body.IsEmpty)
            {
                return false;
            }

            result = body;
            return true;
        }
    }
}
