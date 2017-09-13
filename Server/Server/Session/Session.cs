using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Server
{
    class Session
    {
        private readonly string address;
        private readonly int port;

        public Session(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var value = Encoding.ASCII.GetString(bytes);
            var segments = value.Split(';');
            address = IPAddress.Parse(segments[0]).ToString();
            port = int.Parse(segments[1]);
        }

        public Session(IPEndPoint iPEndPoint)
        {
            address = iPEndPoint.Address.ToString();
            port = iPEndPoint.Port;
        }

        public IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(address), port);
        }

        public override string ToString()
        {
            var value = $"{address};{port}";
            var bytes = Encoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
    }
}
