using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Connection.Udp
{
    interface IUdpConnection : IHostedService
    {
        Task SendAsync(MessageData data);
    }
}
