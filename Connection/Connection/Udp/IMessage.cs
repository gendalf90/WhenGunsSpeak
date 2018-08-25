using System.Threading.Tasks;

namespace Connection.Udp
{
    interface IMessage
    {
        Task ReceiveMessageDataAsync(IMessageClient messageClient);

        Task SendAsync(IMessageClient messageClient, MessageData data);

        Task SendFuckNatRequestAsync(IMessageClient messageClient);
    }
}
