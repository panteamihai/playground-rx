using System;
using System.Reactive.Disposables;

namespace RxWorkshop.Implementations
{
    public class NaiveSubject<T> : IObservable<T>, IObserver<T>
    {
        private IObserver<T> _observer;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            _observer = observer;
            return Disposable.Empty;
        }

        public void OnNext(T value)
        {
            _observer?.OnNext(value);
        }

        public void OnError(Exception error)
        {
            _observer?.OnError(error);
        }

        public void OnCompleted()
        {
            _observer?.OnCompleted();
        }
    }
}
