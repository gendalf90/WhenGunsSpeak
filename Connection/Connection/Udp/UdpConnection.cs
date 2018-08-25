using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connection.Udp
{
    class UdpConnection : IHostedService, IUdpConnection
    {
        public readonly UdpClient udpClient;

        public UdpConnection()
        {

        }




        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
