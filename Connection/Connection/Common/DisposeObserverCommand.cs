using System;

namespace Connection.Common
{
    class DisposeObserverCommand<T> : IDisposable
    {
        private readonly IObserver<T> observer;
        private readonly IObserverComposite<T> composite;

        public DisposeObserverCommand(IObserverComposite<T> composite, IObserver<T> observer)
        {
            this.composite = composite;
            this.observer = observer;
        }

        public void Dispose()
        {
            composite.Remove(observer);
        }
    }
}
