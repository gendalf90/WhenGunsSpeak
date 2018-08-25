using System;
using System.Threading.Tasks;

namespace Connection.Udp
{
    interface IMessageClient : IDisposable
    {
        Task<MessageDto> ReceiveAsync();

        Task SendAsync(MessageDto data);
    }
}
