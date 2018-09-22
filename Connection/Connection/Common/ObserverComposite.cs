using System;

namespace Connection.Common
{
    class ObserverComposite<T> : IObserverComposite<T>
    {
        public void Add(IObserver<T> observer)
        {
            OnCompletedEvent += observer.OnCompleted;
            OnErrorEvent += observer.OnError;
            OnNextEvent += observer.OnNext;
        }

        public void OnCompleted()
        {
            OnCompletedEvent?.Invoke();
        }

        public void OnError(Exception error)
        {
            OnErrorEvent?.Invoke(error);
        }

        public void OnNext(T value)
        {
            OnNextEvent?.Invoke(value);
        }

        public void Remove(IObserver<T> observer)
        {
            OnCompletedEvent -= observer.OnCompleted;
            OnErrorEvent -= observer.OnError;
            OnNextEvent -= observer.OnNext;
        }

        private event Action OnCompletedEvent;

        private event Action<Exception> OnErrorEvent;

        private event Action<T> OnNextEvent;
    }
}
