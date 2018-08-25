using FuckNatService.MessageClient;
using System.Threading.Tasks;

namespace FuckNatService.Messages
{
    interface IRequest
    {
        Task LoadAsync(IMessageClient client);

        Task SendResponseIfValidAsync(IMessageClient client);
    }
}
