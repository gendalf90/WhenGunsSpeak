using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Server
{
    //сюда навешать сжатие и шифрацию через декораторы над ipacket
    class Udp : MonoBehaviour
    {
        private IPEndPoint[] addresses;
        private readonly object sendingSync;
        private readonly object receivedSync;
        private Queue<IPacket> sending;
        private List<IPacket> received;
        private int currentAddressIndex = -1;
        private volatile bool isRunning;
        private volatile bool isPaused;
        private UdpClient client;

        public Udp()
        {
            sendingSync = new object();
            receivedSync = new object();
            received = new List<IPacket>();
            sending = new Queue<IPacket>();
        }

        private void Start()
        {
            Run();
            StartBackground(ReceiveProcessing);
            StartBackground(SendProcessing);
        }

        private void OnEnable()
        {
            isPaused = false;
        }

        private void OnDisable()
        {
            isPaused = true;
        }

        private void Run()
        {
            client = new UdpClient();
            isRunning = true;
        }

        private void Stop()
        {
            isRunning = false;
            client.Close();
        }

        private void StartBackground(ThreadStart action)
        {
            var thread = new Thread(action) { IsBackground = true };
            thread.Start();
        }

        private void ReceiveProcessing()
        {
            while(isRunning)
            {
                if(isPaused)
                {
                    continue;
                }

                try
                {
                    InternalReceive();
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private void InternalReceive()
        {
            IPEndPoint endPoint = null;
            var bytes = client.Receive(ref endPoint);

            lock(receivedSync)
            {
                received.Add(bytes.ToPacket());
            }
        }

        private void SendProcessing()
        {
            while (isRunning)
            {
                if(isPaused)
                {
                    continue;
                }

                try
                {
                    InternalSend();
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        private void InternalSend()
        {
            IPacket packet;
            
            lock(sendingSync)
            {
                if (sending.Count == 0)
                {
                    return;
                }

                packet = sending.Dequeue();
            }

            var endPoint = GetNextAddress();
            var bytes = packet.GetBytes();
            client.Send(bytes, bytes.Length, endPoint);
        }

        public IPacket[] Receive()
        {
            lock (receivedSync)
            {
                var result = received.ToArray();
                received.Clear();
                return result;
            }
        }

        public void Send(IPacket packet)
        {
            lock(sendingSync)
            {
                sending.Enqueue(packet);
            }
        }

        private IPEndPoint GetNextAddress()
        {
            return addresses[Interlocked.Increment(ref currentAddressIndex) % addresses.Length];
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}
