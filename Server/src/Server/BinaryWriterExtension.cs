using System.IO;
using System.Net;

namespace Server
{
    public static class BinaryWriterExtension
    {
        private const int PortLength = 4;

        public static void Write(this BinaryWriter writer, IPEndPoint ipEndPoint)
        {
            IPAddress address = ipEndPoint.Address;
            byte[] addressBytes = address.GetAddressBytes();
            writer.Write(addressBytes.Length + PortLength);
            writer.Write(ipEndPoint.Port);
            writer.Write(addressBytes);
        }
    }
}
