using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;
using RxWorkshop.Extensions;
using RxWorkshop.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using CollectionAssert = Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert;

namespace RxWorkshop
{
    public static class Testing
    {
        public static void AdvanceTo_WillExecuteAllQueuedActions_UpToAnAbsoluteTime_SpecifiedInTicks()
        {
            var scheduler = new TestScheduler();
            scheduler.Schedule(() => Console.WriteLine("I was scheduled immediately"));
            scheduler.Schedule(TimeSpan.FromTicks(154), () => Console.WriteLine("I was scheduled for 154 ticks after start"));
            scheduler.Schedule(TimeSpan.FromTicks(226), () => Console.WriteLine("I came in last at 226 ticks"));

            Console.WriteLine("\tAdvance internal clock to 1 tick");
            scheduler.AdvanceTo(1);
            Console.WriteLine("\tAdvance internal clock to 150 ticks");
            scheduler.AdvanceTo(150);
            Console.WriteLine("\tAdvance internal clock to 160 ticks");
            scheduler.AdvanceTo(160);
            Console.WriteLine("\tAdvance internal clock to 226 ticks");
            scheduler.AdvanceTo(226);
        }

        public static void AdvanceBy_WillMoveTheInternalClockAhead_ByARelativeAmount_SpecifiedInTicks()
        {
            var scheduler = new TestScheduler();
            scheduler.Schedule(() => Console.WriteLine("I was scheduled immediately"));
            scheduler.Schedule(TimeSpan.FromTicks(233), () => Console.WriteLine("I came in last at 233 ticks"));
            scheduler.Schedule(TimeSpan.FromTicks(89), () => Console.WriteLine("\nNB: The order for scheduling is taken from the timeline, not the order of executing the scheduling statments!\nI was scheduled for 89 ticks after start"));

            Console.WriteLine("\tAdvance internal clock by 50 ticks, now at 50");
            scheduler.AdvanceBy(50);
            Console.WriteLine("\tAdvance internal clock by 50 ticks, now at 100");
            scheduler.AdvanceBy(50);
            Console.WriteLine("\tAdvance internal clock by 100 ticks, now at 200");
            scheduler.AdvanceBy(100);
            Console.WriteLine("\tAdvance internal clock by 35 ticks, now at 235");
            scheduler.AdvanceBy(35);
        }

        public static void Start_WillRunAllActions_StopingAtTheLastOne()
        {
            var scheduler = new TestScheduler();
            scheduler.Schedule(() => Console.WriteLine("I was scheduled immediately"));
            scheduler.Schedule(TimeSpan.FromTicks(17), () => Console.WriteLine("I came in last at 17 ticks"));
            scheduler.Schedule(TimeSpan.FromTicks(4878), () => Console.WriteLine("I was scheduled for 4878 ticks after start"));

            Console.WriteLine("Starting");
            scheduler.Start();
            Console.WriteLine($"Stopping, with clock @ {scheduler.Clock} ticks");
        }

        public static void Start_WillRunActionsScheduledAfterItsCall_OnlyWithASubsequentCall()
        {
            var scheduler = new TestScheduler();
            scheduler.Schedule(() => Console.WriteLine("I was scheduled immediately"));
            scheduler.Schedule(TimeSpan.FromTicks(287), () => Console.WriteLine("I came in last at 287 ticks"));
            scheduler.Schedule(TimeSpan.FromTicks(369), () => Console.WriteLine("I was scheduled for 369 ticks after start"));

            Console.WriteLine("Starting");
            scheduler.Start();
            Console.WriteLine($"Stopping, with clock @ {scheduler.Clock} ticks");

            scheduler.Schedule(() => Console.WriteLine("Only gonna see me if you call Start() again!"));
            //scheduler.Start();
        }

        public static void Stop_WillPreventFurtherScheduledActions_FromTakingPlace()
        {
            var scheduler = new TestScheduler();
            scheduler.Schedule(() => Console.WriteLine("I was scheduled immediately"));
            scheduler.Schedule(TimeSpan.FromTicks(162), () => Console.WriteLine("I followed suit at 162 ticks"));
            scheduler.Schedule(TimeSpan.FromTicks(336), () => { Console.WriteLine("Stopping at 336 ticks");
                scheduler.Stop();
            });
            scheduler.Schedule(TimeSpan.FromTicks(487), () => Console.WriteLine("Ain't gonna see this unless you Start() again."));

            Console.WriteLine("Starting");
            scheduler.Start();
            Console.WriteLine($"Stopping, with clock @ {scheduler.Clock} ticks");

            //scheduler.Start();
        }

        public static void Collisions_AreHandled_InTheOrderTheyWereScheduled()
        {
            const long SamePointInTime = 4376;

            var scheduler = new TestScheduler();
            scheduler.Schedule(TimeSpan.FromTicks(SamePointInTime), () => Console.WriteLine("Frist"));
            scheduler.Schedule(TimeSpan.FromTicks(SamePointInTime), () => Console.WriteLine("Sucund"));
            scheduler.Schedule(TimeSpan.FromTicks(SamePointInTime), () => Console.WriteLine("Tird"));

            Console.WriteLine("Starting");
            scheduler.Start();
            Console.WriteLine($"Stopping, with clock @ {scheduler.Clock} ticks");
        }

        public static void InjectingTestScheduler_AndManipulatingVirtualTime_HappensInstantly()
        {
            Benchmarker.Benchmark(() =>
            {
                var expectedValues = new long[] { 0, 1, 2, 3, 4 };
                var actualValues = new List<long>();
                var scheduler = new TestScheduler();

                var subscription = Observable
                    .Interval(TimeSpan.FromSeconds(1), scheduler)
                    .Take(5)
                    .Subscribe(actualValues.Add);
                scheduler.Start();

                CollectionAssert.AreEqual(expectedValues, actualValues);
                Console.WriteLine("Collection was as expected.");
            });
        }

        public class Model
        {
            public virtual IObservable<decimal> GetPriceStream(Action<string> notify)
            {
                    return Observable.Create<decimal>(
                        obs =>
                        {
                            obs.OnNext(135.3523m);
                            notify("One");
                            obs.OnNext(136.2467m);
                            notify("Two");
                            obs.OnNext(135.9987m);
                            notify("Three");

                            //Force timeout to occur by commenting out the OnCompleted (simulating an Observable.Never)
                            obs.OnCompleted();

                            return System.Reactive.Disposables.Disposable.Create(() => notify($"Done with the stream on {Thread.CurrentThread.ManagedThreadId}"));
                        });
            }

            public virtual ObservableCollection<decimal> Prices { get; } = new ObservableCollection<decimal>();

            public bool IsConnected { get; private set; }

            public void Show(IScheduler uiScheduler, IScheduler threadPoolScheduler, Action<string> notify)
            {
                GetPriceStream(notify)
                    .ObserveOn(uiScheduler)
                    .Timeout(TimeSpan.FromSeconds(10), threadPoolScheduler)
                    .Subscribe(
                        priceFromStream =>
                        {
                            Prices.Add(priceFromStream);
                            notify($"Added {priceFromStream} on {Thread.CurrentThread.ManagedThreadId}");
                        },
                        ex =>
                        {
                            if (ex is TimeoutException)
                            {
                                notify($"Timed out on {Thread.CurrentThread.ManagedThreadId}");
                                IsConnected = false;
                            }
                        });

                IsConnected = true;
            }
        }

        public static void Testing_HappyFlow_InjectingControllingSchedulers(Form form, Action<string> notify)
        {
            var model = new Model();
            var uiScheduler = new TestScheduler();
            var threadPoolScheduler = new TestScheduler();
            model.Show(uiScheduler, threadPoolScheduler, notify);

            var halfASecondLater = TimeSpan.FromMilliseconds(500).Ticks;
            var twelveSecondsLater = TimeSpan.FromSeconds(12).Ticks;

            threadPoolScheduler.AdvanceTo(halfASecondLater);
            uiScheduler.AdvanceTo(halfASecondLater);

            Assert.That(model.IsConnected, Is.True);

            threadPoolScheduler.AdvanceTo(twelveSecondsLater);
            uiScheduler.AdvanceTo(twelveSecondsLater);

            Assert.That(model.IsConnected, Is.True);
        }

        public static void Testing_ExceptionFlow_InjectingControllingSchedulers(Form form, Action<string> notify)
        {
            var modelMock = new Mock<Model>();
            var prices = new ObservableCollection<decimal>();
            modelMock.SetupGet(m => m.Prices).Returns(prices);
            modelMock.Setup(m => m.GetPriceStream(notify)).Returns(Observable.Never<decimal>());

            var uiScheduler = new TestScheduler();
            var threadPoolScheduler = new TestScheduler();
            modelMock.Object.Show(uiScheduler, threadPoolScheduler, notify);

            var halfASecondLater = TimeSpan.FromMilliseconds(500).Ticks;
            var twelveSecondsLater = TimeSpan.FromSeconds(12).Ticks;

            threadPoolScheduler.AdvanceTo(halfASecondLater);
            uiScheduler.AdvanceTo(halfASecondLater);

            Assert.That(modelMock.Object.IsConnected, Is.True);

            threadPoolScheduler.AdvanceTo(twelveSecondsLater);
            uiScheduler.AdvanceTo(twelveSecondsLater);

            Assert.That(modelMock.Object.IsConnected, Is.False);
        }

        public static void TestableObserver_Start_CanBeUsedToControl_TheCreationSubscriptionAndDisposal_OfASourceObservable()
        {
            var scheduler = new TestScheduler();
            var source = Observable.Interval(TimeSpan.FromSeconds(1), scheduler).Take(4);

            source.RunOn(scheduler, disposalTick: TimeSpan.FromSeconds(5).Ticks);
            //source.RunOn(scheduler, subscriptionTick: TimeSpan.FromSeconds(2).Ticks, disposalTick: TimeSpan.FromSeconds(5).Ticks);
        }

        public static void CreateColdObservable_CanBeUsedToSimulateTheBehaviourOfAPipeline_GivenAPredictableAndControlledSource()
        {
            var scheduler = new TestScheduler();
            var source = scheduler.CreateColdObservable(
                new Recorded<Notification<long>>(10000000, Notification.CreateOnNext(0L)),
                new Recorded<Notification<long>>(20000000, Notification.CreateOnNext(1L)),
                new Recorded<Notification<long>>(30000000, Notification.CreateOnNext(2L)),
                new Recorded<Notification<long>>(40000000, Notification.CreateOnNext(3L)),
                new Recorded<Notification<long>>(40000000, Notification.CreateOnCompleted<long>())
            );

            source.RunOn(scheduler, disposalTick: TimeSpan.FromSeconds(5).Ticks);
            //source.RunOn(scheduler, subscriptionTick: TimeSpan.FromSeconds(1).Ticks, disposalTick: TimeSpan.FromSeconds(5).Ticks);
        }

        public static void CreateHotObservable_HotBehaviorInfluences_SubscriptionTimeVeryMuchSo()
        {
            var scheduler = new TestScheduler();
            var source = scheduler.CreateHotObservable(
                new Recorded<Notification<long>>(10000000, Notification.CreateOnNext(0L)),
                new Recorded<Notification<long>>(20000000, Notification.CreateOnNext(1L)),
                new Recorded<Notification<long>>(30000000, Notification.CreateOnNext(2L)),
                new Recorded<Notification<long>>(40000000, Notification.CreateOnNext(3L)),
                new Recorded<Notification<long>>(40000000, Notification.CreateOnCompleted<long>())
            );

            // Scheduling of the creation and subscription do not affect the Hot Observable
            // therefore the notifications happen 1 tick earlier than their Cold counterparts.
            source.RunOn(scheduler, disposalTick: TimeSpan.FromSeconds(5).Ticks); //One measly tick difference from Cold
            //source.RunOn(scheduler, subscriptionTick: TimeSpan.FromSeconds(1).Ticks, disposalTick: TimeSpan.FromSeconds(5).Ticks);
        }
    }
}
