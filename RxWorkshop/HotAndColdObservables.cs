using RxWorkshop.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace RxWorkshop
{
    public class HotAndColdObservables
    {
        public static void HotIsEager_ColdIsLazy_InTheEnumerableObservableDuality()
        {
            void ReadFirstValue(IEnumerable<int> list)
            {
                foreach (var i in list)
                {
                    Console.WriteLine("Read out first value of {0}", i);
                    break;
                }
            }

            IEnumerable<int> EagerEvaluation()
            {
                var result = new List<int>();

                Console.WriteLine("About to return 1 eagerly");
                result.Add(1);

                //The code below is executed but not used
                Console.WriteLine("About to return 2 eagerly");
                result.Add(2);

                return result;
            }

            IEnumerable<int> LazyEvaluation()
            {
                Console.WriteLine("About to return 1 lazily");
                yield return 1;

                //Execution stops here
                Console.WriteLine("About to return 2 lazily");
                yield return 2;
            }

            ReadFirstValue(EagerEvaluation());

            Console.ReadLine();
            ReadFirstValue(LazyEvaluation());
        }

        public static void Publish_ConnectShouldBeCalledAfterAllSubscribersSubscribe()
        {
            var period = TimeSpan.FromSeconds(1);
            var observable = Observable.Interval(period).Take(4).Publish();
            observable.Connect();
            observable.Subscribe(
                i =>
                {
                    Get.CurrentThread();
                    Console.WriteLine($"Frist: {i}");
                });

            Thread.Sleep(period);

            observable.Subscribe(i =>
            {
                Get.CurrentThread();
                Console.WriteLine($"Sucund: {i}");
            });

            //observable.Connect();
        }

        public static void Publish_CallingDisposeTogglesTheSequenceOff()
        {
            var period = TimeSpan.FromSeconds(1);
            var observable = Observable.Interval(period).Publish();
            observable.Subscribe(i => Console.WriteLine($"Subscription : {i}"));

            var exit = false;
            while (!exit)
            {
                Console.WriteLine("\nPress ENTER to connect, ESC to exit.");
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        using (observable.Connect())
                        {//--Connects here--
                            Console.WriteLine("Press ANY KEY to dispose of connection.");
                            Console.ReadKey();
                        } //--Disconnects here--
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            }
        }

        public static void Publish_SequenceDoesNotEndWhenLastSubscriberDisconnects()
        {
            var period = TimeSpan.FromSeconds(1);
            var observable = Observable.Interval(period)
                .Do(l => Console.WriteLine($"Publishing {l}")) //Side effect to show it is running
                .Publish();
            observable.Connect();
            Console.WriteLine("Press any key to subscribe");
            Console.ReadKey();
            var subscription = observable.Subscribe(i => Console.WriteLine($"Subscription : {i}"));
            Console.WriteLine("Press any key to unsubscribe.");
            Console.ReadKey();
            subscription.Dispose();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static void Publish_RefCount_WillDisposeWhenNoMoreSubscribers_ButWillAlsoConnectOnlyOnFirstSubscriber()
        {
            var period = TimeSpan.FromSeconds(1);
            var observable = Observable.Interval(period)
                .Do(l => Console.WriteLine($"Publishing {l}")) //Side effect to show it is running
                .Publish()
                .RefCount();

            Console.WriteLine("Press any key to subscribe");
            Console.ReadKey();
            var subscription = observable.Subscribe(i => Console.WriteLine($"Subscription : {i}"));
            Console.WriteLine("Press any key to unsubscribe.");
            Console.ReadKey();
            subscription.Dispose();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

            //The Publish/RefCount pair is extremely useful for taking a cold observable and sharing it as a hot observable sequence for subsequent observers
        }

        public static void PublishLast_IsAHotVersionLastAsync()
        {
            var period = TimeSpan.FromSeconds(1);
            var observable = Observable.Interval(period)
                .Take(5)
                .Do(l => Console.WriteLine($"Publishing {l}")) //side effect to show it is running
                .PublishLast();
            observable.Connect();
            Console.WriteLine("Press any key to subscribe");
            Console.ReadKey();
            var subscription1 = observable.Subscribe(i => Console.WriteLine($"subscription1 : {i}"));
            var subscription2 = observable.Subscribe(i => Console.WriteLine($"subscription2 : {i}"));
            Console.WriteLine("Press any key to unsubscribe.");
            Console.ReadKey();
            subscription1.Dispose();
            subscription2.Dispose();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static void Replay_WrapsAHotObservableInAReplayableOne()
        {
            var period = TimeSpan.FromSeconds(1);
            var hot = Observable.Interval(period)
                .Take(3)
                .Do(l => Console.WriteLine($"Publishing {l}")) //side effect to show it is running
                .Publish();
            hot.Connect();
            Thread.Sleep(period); //Run hot and ensure a value is lost.
            var observable = hot.Replay();
            observable.Connect();
            observable.Subscribe(i => Console.WriteLine($"first subscription : {i}"));
            Thread.Sleep(period);
            observable.Subscribe(i => Console.WriteLine($"second subscription : {i}"));
            Console.ReadKey();
            observable.Subscribe(i => Console.WriteLine($"third subscription : {i}"));
            Console.ReadKey();
        }

        public static void Multicast_AllowsYouToWrapColdSequencesInSubjectBehaviors()
        {
            //.Publish() = .Multicast(new Subject<T>)
            //.PublishLast() = .Multicast(new AsyncSubject<T>)
            //.Replay() = .Multicast(new ReplaySubject<T>)

            var period = TimeSpan.FromSeconds(1);
            var asyncSubject = new AsyncSubject<long>();

            var hot = Observable.Interval(period)
                .Take(3)
                .Do(l => Console.WriteLine($"Publishing {l}")) //side effect to show it is running
                .Multicast(asyncSubject);
            hot.Connect();
            Thread.Sleep(period); //Run hot and ensure a value is lost.
            var observable = hot.Replay();
            observable.Connect();
            observable.Subscribe(i => Console.WriteLine($"first subscription : {i}"));
            Thread.Sleep(period);
            observable.Subscribe(i => Console.WriteLine($"second subscription : {i}"));
            Console.ReadKey();
            observable.Subscribe(i => Console.WriteLine($"third subscription : {i}"));
            Console.ReadKey();
        }
    }
}
