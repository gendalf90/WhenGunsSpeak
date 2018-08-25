using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Connection.Udp
{
    class Message
    {
        private const int NatFuckingSignKeyLength = 20;
        private const int NatFuckingSessionKeyLength = 16;
        private const int AddressLength = 8;
        private const int PortLength = 4;
        private const int IVLength = 32;

        private IObserver<MessageData> messageObserver;
        private IObserver<MyIPData> myIPObserver;

        private IPEndPoint natFuckerAddress;
        private byte[] natFuckingSessionKey;
        private byte[] securityKeyBytes;
        private MessageDto currentMessageData;

        public async Task ReceiveAsync(IMessageClient messageClient)
        {
            await ReceiveMessageDataAsync(messageClient);

            if (IsNatFuckingResponse())
            {
                NotifyAboutNatFuckingResponse();
            }
            else if(IsMessage())
            {
                NotifyAboutMessage();
            }
        }

        private async Task ReceiveMessageDataAsync(IMessageClient messageClient)
        {
            currentMessageData = await messageClient.ReceiveAsync();
        }

        private bool IsMessage()
        {
            return currentMessageData.Bytes.Length > IVLength;
        }

        private void NotifyAboutMessage()
        {
            messageObserver.OnNext(new MessageData
            {
                Bytes = DecryptMessage(currentMessageData.Bytes),
                IP = currentMessageData.IP
            });
        }

        private byte[] DecryptMessage(byte[] bytes)
        {
            using (var algorithm = new AesManaged())
            using (var messageStream = new MemoryStream(bytes))
            {
                var ivBytes = new byte[IVLength];
                messageStream.Read(ivBytes, 0, ivBytes.Length);
                var decryptor = algorithm.CreateDecryptor(securityKeyBytes, ivBytes);

                using (var cryptoStream = new CryptoStream(messageStream, decryptor, CryptoStreamMode.Read))
                using (var resultStream = new MemoryStream())
                {
                    cryptoStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        }

        private void NotifyAboutNatFuckingResponse()
        {
            var myRealIP = GetMyRealIPFromResponse(currentMessageData.Bytes);

            myIPObserver.OnNext(new MyIPData
            {
                IP = myRealIP
            });
        }

        private bool IsNatFuckingResponse()
        {
            var bytes = currentMessageData.Bytes;

            if(bytes.Length != NatFuckingSessionKeyLength + NatFuckingSignKeyLength + AddressLength + PortLength)
            {
                return false;
            }

            var responseSessionKey = new byte[NatFuckingSessionKeyLength];
            Array.Copy(bytes, NatFuckingSignKeyLength, responseSessionKey, 0, responseSessionKey.Length);

            if (!responseSessionKey.SequenceEqual(natFuckingSessionKey))
            {
                return false;
            }

            var responseSignKey = new byte[NatFuckingSignKeyLength];
            Array.Copy(bytes, 0, responseSignKey, 0, responseSignKey.Length);
            var responseBody = new byte[bytes.Length - responseSignKey.Length];
            Array.Copy(bytes, responseSignKey.Length, responseBody, 0, responseBody.Length);
            var validSignKey = CreateSignKeyOfData(responseBody);
            return validSignKey.SequenceEqual(responseSignKey);
        }

        private byte[] CreateSignKeyOfData(byte[] bytes)
        {
            using (var hmac = new HMACSHA1(securityKeyBytes))
            {
                return hmac.ComputeHash(bytes);
            }
        }

        private IPEndPoint GetMyRealIPFromResponse(byte[] bytes)
        {
            var address = BitConverter.ToInt64(bytes, NatFuckingSessionKeyLength + NatFuckingSignKeyLength);
            var port = BitConverter.ToInt32(bytes, NatFuckingSessionKeyLength + NatFuckingSignKeyLength + AddressLength);
            return new IPEndPoint(address, port);
        }

        public async Task SendAsync(IMessageClient messageClient, MessageData data)
        {
            await messageClient.SendAsync(new MessageDto
            {
                Bytes = EncryptMessage(data.Bytes),
                IP = data.IP
            });
        }

        private byte[] EncryptMessage(byte[] bytes)
        {
            using (var algorithm = new AesManaged())
            using (var messageStream = new MemoryStream(bytes))
            {
                var ivBytes = algorithm.IV;
                messageStream.Write(ivBytes, 0, ivBytes.Length);
                var decryptor = algorithm.CreateEncryptor(securityKeyBytes, ivBytes);

                using (var cryptoStream = new CryptoStream(messageStream, decryptor, CryptoStreamMode.Write))
                using (var resultStream = new MemoryStream())
                {
                    cryptoStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
        }

        public async Task SendFuckNatRequestAsync(IMessageClient messageClient)
        {
            await messageClient.SendAsync(new MessageDto
            {
                Bytes = natFuckingSessionKey,
                IP = natFuckerAddress
            });
        }
    }
}
