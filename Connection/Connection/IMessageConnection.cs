using System;
using System.Threading.Tasks;

namespace Connection
{
    public interface IMessageConnection : IObservable<MessageData>, 
                                          IObservable<MyIPData>, 
                                          IDisposable
    {
        Task SendAsync(MessageData data);
    }
}
