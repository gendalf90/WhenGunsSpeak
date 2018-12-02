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
    }
}
