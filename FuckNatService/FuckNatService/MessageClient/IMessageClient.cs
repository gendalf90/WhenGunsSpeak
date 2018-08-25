using System;
using System.Threading.Tasks;

namespace FuckNatService.MessageClient
{
    interface IMessageClient : IDisposable
    {
        Task<MessageDto> ReceiveAsync();

        Task SendAsync(MessageDto data);
    }
}
