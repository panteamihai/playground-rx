using RxWorkshop.Extensions;
using System;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace RxWorkshop
{
    public static class AdvancedErrorHandling
    {
        public static void Catch_ForSwallowingAllExceptions()
        {
            var source = Observable.Create<int>(
                o =>
                {
                    o.OnNext(58);
                    o.OnNext(208);
                    o.OnError(new Exception("Kinda general!"));

                    return Disposable.Empty;
                });
            var result = source.Catch(Observable.Empty<int>());
            result.Dump("Catch");
        }

        public static void Catch_CanBeUsedForSpecificExceptionsToo()
        {
            var source = Observable.Create<int>(
                o =>
                {
                    o.OnNext(135);
                    o.OnNext(654);
                    o.OnError(new ObjectDisposedException("this"));
                    //o.OnError(new ArgumentException("Boom"));

                    return Disposable.Empty;
                });
            var result = source.Catch<int, ObjectDisposedException>(ode => Observable.Return(0));
            result.Dump("Catch");
        }

        public static void Finally_IsInvokedIfWeTerminateNormally_Erronously_OrTheSubscriptionIsDisposedOf()
        {
            var source = Observable.Create<int>(
                o =>
                {
                    o.OnNext(1);
                    o.OnNext(2);
                    o.OnNext(3);
                    //o.OnCompleted();
                    o.OnError(new DataMisalignedException());

                    return Disposable.Empty;
                });

            var result = source.Finally(() => Console.WriteLine("Finally got here"));
            var subscription = result.Dump("Finally-ed source");
            //subscription.Dispose();
        }

        public static void Using_AllowsYouToBindTheLifetimeOfAResource_ToThatOfAnObservable()
        {
            IObservable<long> BuildComponent(IDisposable serviceDependency)
            {
                return Observable.Interval(TimeSpan.FromMilliseconds(350));
            }

            var result = Observable.Using(
                () =>
                {
                    var someServiceCreation = Disposable.Create(() => Console.WriteLine("Done some action after sequence completed"));
                    return someServiceCreation;
                },
                BuildComponent);

            result.Take(8).Dump("Using");
        }

        public static void OnErrorResumeNext_ProvidesAFallBack()
        {
            var erroredSource = Observable.Create<int>(
            o =>
            {
                o.OnNext(135);
                o.OnNext(654);
                o.OnError(new ObjectDisposedException("this"));

                return Disposable.Empty;
            });

            var anotherErroredSource = Observable.Create<int>(
                o =>
                {
                    o.OnNext(445);
                    o.OnNext(366);
                    o.OnError(new CookieException());

                    return Disposable.Empty;
                });

            var fineSource = Observable.Create<int>(
                o =>
                {
                    o.OnNext(368);

                    return Disposable.Empty;
                });

            erroredSource.OnErrorResumeNext(anotherErroredSource.OnErrorResumeNext(fineSource)).Dump("OERN");
        }

        public static void Retry_IsTricky()
        {
            var erroredSource = Observable.Create<int>(
                o =>
                {
                    o.OnNext(445);
                    o.OnNext(366);
                    o.OnError(new CookieException());

                    Thread.Sleep(700);

                    return Disposable.Empty;
                });

            erroredSource.Retry() //.Retry(2)
                         .Dump("Forever loop");
        }
    }
}
