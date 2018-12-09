using BinaryProcessing;
using System;

namespace Utils
{
    public static class BinaryExtensions
    {
        private const int GuidLengthInBytes = 16;

        public static bool HasGuidAtBegin(this byte[] source, Guid value)
        {
            if(source.Length < GuidLengthInBytes)
            {
                return false;
            }

            var binarySource = new Binary(source);
            var sourceGuidBytes = binarySource * GuidLengthInBytes;
            var valueGuidBytes = value.ToByteArray();
            return sourceGuidBytes == valueGuidBytes;
        }

        public static byte[] AddGuidAtBegin(this byte[] body, Guid value)
        {
            var guidBinary = new Binary(value.ToByteArray());
            var bodyBinary = new Binary(body);
            var result = guidBinary + bodyBinary;
            return result.ToBytes();
        }

        public static bool TryGetNotEmptyBodyIfGuidAtBeginIs(this byte[] source, Guid value, out byte[] body)
        {
            body = null;

            if(!source.HasGuidAtBegin(value))
            {
                return false;
            }

            var binaryBody = new Binary(source) / GuidLengthInBytes;

            if(binaryBody.IsEmpty)
            {
                return false;
            }

            body = binaryBody.ToBytes();
            return true;
        }
    }
}
