using System;

namespace Utils
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> next = null, Action completed = null, Action<Exception> error = null)
        {
            return observable.Subscribe(new ActionObserver<T>(next, completed, error));
        }
    }

    public class ActionObserver<T> : IObserver<T>
    {
        private Action completed;
        private Action<Exception> error;
        private Action<T> next;

        public ActionObserver(Action<T> next = null, Action completed = null, Action<Exception> error = null)
        {
            this.next = next;
            this.completed = completed;
            this.error = error;
        }

        public void OnCompleted()
        {
            completed?.Invoke();
        }

        public void OnError(Exception e)
        {
            error?.Invoke(e);
        }

        public void OnNext(T value)
        {
            next?.Invoke(value);
        }
    }
}
