using Microsoft.Reactive.Testing;
using RxWorkshop.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RxWorkshop.Extensions
{
    public static class ObservableExtensions
    {
        public static IDisposable Dump<T>(this IObservable<T> source, string name)
        {
            return source.Dump(name, i => i.ToString());
        }

        public static IDisposable Dump<T, TProject>(this IObservable<T> source, string name, Func<T, TProject> projection)
        {
            return source.Subscribe(
                i => Console.WriteLine($"{name} --> {projection(i)}"),
                ex => Console.WriteLine($"{name} failed! --> {ex.Message}"),
                () => Console.WriteLine($"{name} completed @{DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture)}"));
        }

        public static IDisposable WindowedDump<T>(this IObservable<IObservable<T>> windowedSource, string name)
        {
            var disposables = new CompositeDisposable();

            //Viva las ValueTuples :)
            var mainSubscription = windowedSource.Select((window, i) => (window, i))
                .Subscribe(
                    p =>
                    {
                        Console.WriteLine("\n\tStarting new window.");
                        disposables.Add(p.window.Dump($"{name}[{p.i}]"));
                    },
                    () => Console.WriteLine("\n\tCompleted windowing."));
            disposables.Add(mainSubscription);

            return disposables;
        }

        public static IObservable<T> DecorateWithTime<T>(this IObservable<T> source)
        {
            return source.Do(i => { Get.Now(); });
        }

        public static void RunOn<T>(this IObservable<T> source, TestScheduler scheduler, long creationTick = 0, long subscriptionTick = 0, long disposalTick = 0)
        {
            var testObserver = scheduler.Start(
                () => source,
                creationTick,
                subscriptionTick,
                disposalTick);

            Console.WriteLine("Time is {0} ticks", scheduler.Clock);
            Console.WriteLine("Received {0} notifications", testObserver.Messages.Count);
            foreach (var message in testObserver.Messages)
            {
                Console.WriteLine("{0} @ {1}", message.Value, message.Time);
            }
        }

        public static IObservable<IObservable<T>> NaiveWindow<T>(this IObservable<T> source, int count)
        {
            var sharedSource = source.Publish().RefCount();
            var sharedWindowEdges = sharedSource
                .Select((value, index) => index % count)
                .Where(mod => mod == 0)
                .Publish() // Try making it cold
                .RefCount();

            return sharedSource.Window(sharedWindowEdges, _ => sharedWindowEdges);
            //return sharedSource.Window(sharedWindowEdges/*.Skip(1)*/, _ => sharedWindowEdges/*.Skip(1)*/);
        }

        public static IObservable<IObservable<T>> NaiveWindow<T>(this IObservable<T> source, int count, int skip)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException();
            if (skip <= 0)
                throw new ArgumentOutOfRangeException();

            var sharedSource = source.Publish().RefCount();
            var sharedIndexes = sharedSource
                .Select((value, index) => index)
                .Publish()
                .RefCount();

            var windowOpen = sharedIndexes.Where(i => i % skip == 0);
            var windowClose = sharedIndexes.Skip(count - 1);

            return sharedSource.Window(windowOpen, _ => windowClose);
        }


        public static IObservable<IList<T>> NaiveBuffer<T>(this IObservable<T> source, int count)
        {
            return source.Window(count)
                         .SelectMany(window =>
                                        window.Aggregate(new List<T>(), (list, item) =>
                                            {
                                                list.Add(item);
                                                return list;
                                            }));
        }

        public static IObservable<TResult> NaiveCombineLatest<TLeft, TRight, TResult>(
            this IObservable<TLeft> left, IObservable<TRight> right,
            Func<TLeft, TRight, TResult> resultSelector)
        {
            var sharedLeft = left.Publish().RefCount();
            var sharedRight = right.Publish().RefCount();

            return Observable.Join(
                sharedLeft,
                sharedRight,
                value => sharedLeft,
                value => sharedRight,
                resultSelector);
        }

        public static IObservable<TResult> NaiveJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(
            this IObservable<TLeft> left, IObservable<TRight> right,
            Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector,
            Func<TRight, IObservable<TRightDuration>> rightDurationSelector,
            Func<TLeft, TRight, TResult> resultSelector)
        {
            return Observable.GroupJoin
                (
                    left,
                    right,
                    leftDurationSelector,
                    rightDurationSelector,
                    (leftValue, rightValues) =>
                        rightValues.Select(rightValue => resultSelector(leftValue, rightValue))
                )
                .Merge();
        }
    }
}
