using System.Threading.Tasks;

namespace Connection.Udp
{
    interface IUdpConnection
    {
        Task SendAsync(MessageData data);
    }
}
