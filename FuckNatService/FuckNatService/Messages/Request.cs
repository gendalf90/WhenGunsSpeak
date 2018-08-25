using FuckNatService.MessageClient;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FuckNatService.Messages
{
    class Request : IRequest
    {
        private const int SessionKeyLength = 16;
        private const int AddressLength = 8;
        private const int PortLenght = 4;

        private byte[] securityKeyBytes;
        private byte[] sessionKeyBytes;
        private byte[] endPointBytes;

        private IPEndPoint endPoint;

        public Request(string securityKey)
        {
            securityKeyBytes = Encoding.UTF8.GetBytes(securityKey);
        }

        public async Task LoadAsync(IMessageClient client)
        {
            var messageData = await client.ReceiveAsync();
            endPoint = messageData.EndPoint;
            endPointBytes = CreateEndPointBytes(endPoint);
            sessionKeyBytes = messageData.Bytes;
        }

        private byte[] CreateEndPointBytes(IPEndPoint endPoint)
        {
            var result = new byte[AddressLength + PortLenght];
            var addressBytes = endPoint.Address.GetAddressBytes();
            addressBytes.CopyTo(result, 0);
            var portBytes = BitConverter.GetBytes(endPoint.Port);
            portBytes.CopyTo(result, AddressLength);
            return result;
        }

        public async Task SendResponseIfValidAsync(IMessageClient client)
        {
            if(!IsReadyToResponse)
            {
                return;
            }

            var body = CreateResponseBody();
            var sign = CreateSignKeyOfData(body);
            var signedBody = CreateSignedResponse(sign, body);
            var messageData = new MessageDto { Bytes = signedBody, EndPoint = endPoint };
            await client.SendAsync(messageData);
        }

        private bool IsReadyToResponse
        {
            get => sessionKeyBytes != null && securityKeyBytes.Length == SessionKeyLength && endPoint != null;
        }

        private byte[] CreateSignKeyOfData(byte[] bytes)
        {
            using (var hmac = new HMACSHA1(securityKeyBytes))
            {
                return hmac.ComputeHash(bytes);
            }
        }

        private byte[] CreateResponseBody()
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(sessionKeyBytes, 0, sessionKeyBytes.Length);
                stream.Write(endPointBytes, 0, endPointBytes.Length);
                return stream.ToArray();
            }
        }

        private byte[] CreateSignedResponse(byte[] sign, byte[] body)
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(sign, 0, sign.Length);
                stream.Write(body, 0, body.Length);
                return stream.ToArray();
            }
        }
    }
}
