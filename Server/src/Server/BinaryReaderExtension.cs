using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Server
{
    public static class BinaryReaderExtension
    {
        private const int PortLength = 4;
        private const int BufferSize = 1024;

        public static byte[] ReadToEnd(this BinaryReader reader)
        {
            using (var tempStream = new MemoryStream())
            {
                reader.BaseStream.CopyTo(tempStream, BufferSize);
                return tempStream.ToArray();
            }
        }

        public static IPEndPoint ReadIPEndPoint(this BinaryReader reader)
        {
            var length = reader.ReadInt32();
            var port = reader.ReadInt32();
            var addressBytes = reader.ReadBytes(length - PortLength);
            var address = new IPAddress(addressBytes);
            return new IPEndPoint(address, port);
        }
    }
}
