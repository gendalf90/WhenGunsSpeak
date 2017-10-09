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
        private ConcurrentQueue<IPacket> sending;
        private ConcurrentQueue<IPacket> received;
        private int currentAddressIndex = -1;
        private volatile bool isRunning;
        private volatile bool isPaused;
        private UdpClient client;

        public Udp()
        {
            received = new ConcurrentQueue<IPacket>();
            sending = new ConcurrentQueue<IPacket>();
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
            received.Enqueue(bytes.ToPacket());
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
            IPacket packet = null;

            if (!sending.TryDequeue(out packet))
            {
                return;
            }

            var endPoint = GetNextAddress();
            var bytes = packet.GetBytes();
            client.Send(bytes, bytes.Length, endPoint);
        }

        public IPacket[] Receive()
        {
            var result = new IPacket[received.Count];

            for (int i = 0; i < result.Length; i++)
            {
                received.TryDequeue(out result[i]);
            }

            return result;
        }

        public void Send(IPacket packet)
        {
            sending.Enqueue(packet);
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
