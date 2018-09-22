using System;

namespace Connection.Common
{
    interface IObserverComposite<T> : IObserver<T>
    {
        void Add(IObserver<T> observer);

        void Remove(IObserver<T> observer);
    }
}
