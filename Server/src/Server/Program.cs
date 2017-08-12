using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        private static IPEndPoint localEndPoint;
        private static ConcurrentDictionary<IPEndPoint, DateTime> clients;
        private static Timer removeClientTimer;
        private static TimeSpan ipTimeout;
        private static UdpClient udpClient;

        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddCommandLine(args)
                                                          .SetBasePath(Directory.GetCurrentDirectory())
                                                          .AddJsonFile("settings.json")
                                                          .Build();

            localEndPoint = new IPEndPoint(IPAddress.Any, configuration.GetValue<int>("Port"));
            clients = new ConcurrentDictionary<IPEndPoint, DateTime>();
            var ipTimeouts = configuration.GetSection("IPTimeouts");
            ipTimeout = TimeSpan.FromMilliseconds(ipTimeouts.GetValue<int>("IPTimeoutInMilliseconds"));
            var ipCheckInterval = ipTimeouts.GetValue<int>("IPCheckIntervalInMilliseconds");

            removeClientTimer = new Timer(RemoveTimeoutClients, null, ipCheckInterval, ipCheckInterval);

            udpClient = new UdpClient(localEndPoint);

            Listen();

            Console.WriteLine($"Host runned on port : {localEndPoint.Port}");
            Console.WriteLine($"IP timeout : {ipTimeout.TotalMilliseconds} milliseconds");
            Console.WriteLine($"IP check interval : {ipCheckInterval} milliseconds");

            Thread.Sleep(Timeout.InfiniteTimeSpan);
        }

        private static async void Listen()
        {
            var result = await udpClient.ReceiveAsync();
            Listen();
            ProcessResultAsync(result);

            //try
            //{
            //    var result = await udpClient.ReceiveAsync();
            //    ProcessResultAsync(result);
            //}
            //catch
            //{
            //    // do nothing
            //}
            //finally
            //{
            //    Listen();
            //}
        }

        private static async void ProcessResultAsync(UdpReceiveResult receiveResult)
        {
            clients[receiveResult.RemoteEndPoint] = DateTime.UtcNow;

            using (var receiveStream = new MemoryStream(receiveResult.Buffer))
            using (var reader = new BinaryReader(receiveStream))
            using (var sendStream = new MemoryStream())
            using (var writer = new BinaryWriter(sendStream))
            {
                var messageType = reader.ReadByte();

                if (messageType == 1)
                {
                    var toIpLength = reader.ReadInt32();
                    var toIpArray = new IPEndPoint[toIpLength];

                    for (int i = 0; i < toIpArray.Length; i++)
                    {
                        toIpArray[i] = reader.ReadIPEndPoint();
                    }

                    var data = reader.ReadToEnd();

                    writer.Write(receiveResult.RemoteEndPoint);
                    writer.Write(data);

                    var sendMessage = sendStream.ToArray();

                    foreach (var toIPEndPoint in toIpArray)
                    {
                        await udpClient.SendAsync(sendMessage, sendMessage.Length, toIPEndPoint);
                    }
                }

                if (messageType == 2)
                {
                    var data = reader.ReadToEnd();

                    writer.Write(receiveResult.RemoteEndPoint);
                    writer.Write(data);

                    var sendMessage = sendStream.ToArray();

                    await udpClient.SendAsync(sendMessage, sendMessage.Length, receiveResult.RemoteEndPoint);
                }

                if (messageType == 3)
                {
                    var data = reader.ReadToEnd();

                    writer.Write(receiveResult.RemoteEndPoint);
                    writer.Write(data);

                    var sendMessage = sendStream.ToArray();

                    foreach (var toIPEndPoint in clients.Select(x => x.Key))
                    {
                        await udpClient.SendAsync(sendMessage, sendMessage.Length, toIPEndPoint);
                    }
                }
            }
        }

        private static void RemoveTimeoutClients(object state)
        {
            foreach (var client in clients)
            {
                if (DateTime.UtcNow - client.Value > ipTimeout)
                {
                    DateTime removeValue;
                    clients.TryRemove(client.Key, out removeValue);
                }
            }
        }
    }
}
