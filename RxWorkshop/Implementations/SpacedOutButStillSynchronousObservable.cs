using System;
using System.Reactive.Disposables;
using System.Threading;

namespace RxWorkshop.Implementations
{
    public class SpacedOutButStillSynchronousObservable : IObservable<int>
    {
        public IDisposable Subscribe(IObserver<int> observer)
        {
            observer.OnNext(1);
            Thread.Sleep(1000);
            observer.OnNext(2);
            Thread.Sleep(1500);
            observer.OnNext(3);
            Thread.Sleep(2000);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
