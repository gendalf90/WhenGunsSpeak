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
        private static readonly Guid ResponseMessageKey = new Guid("C1FDA972-DF95-4680-8CF2-00581EB2C5E4");

        private byte[] securityKeyBytes;
        private byte[] responseMessageKeyBytes;
        private byte[] sessionKeyBytes;
        private byte[] endPointBytes;

        private IPEndPoint endPoint;

        public Request(string securityKey)
        {
            securityKeyBytes = Encoding.UTF8.GetBytes(securityKey);
            responseMessageKeyBytes = ResponseMessageKey.ToByteArray();
        }

        public async Task LoadAsync(IMessageClient client)
        {
            var messageData = await client.ReceiveAsync();
            endPoint = messageData.EndPoint;
            var endPointString = messageData.EndPoint.ToString();
            endPointBytes = Encoding.UTF8.GetBytes(endPointString);
            sessionKeyBytes = GetSessionKeyIfExist(messageData.Bytes);
        }

        private byte[] GetSessionKeyIfExist(byte[] bytes)
        {
            if (bytes.Length < SessionKeyLength)
            {
                return null;
            }

            var sessionKeyBytes = new byte[SessionKeyLength];
            bytes.CopyTo(sessionKeyBytes, 0);
            return sessionKeyBytes;
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
            get => sessionKeyBytes != null && endPointBytes != null;
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
                stream.Write(responseMessageKeyBytes, 0, responseMessageKeyBytes.Length);
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
