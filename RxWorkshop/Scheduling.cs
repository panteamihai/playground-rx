using RxWorkshop.Extensions;
using RxWorkshop.Helpers;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

using Disposable = System.Reactive.Disposables.Disposable;

namespace RxWorkshop
{
    public static class Scheduling
    {
        public static void RxIsFreeThreaded_ButSingleThreadedByDefault()
        {
            //Free-threaded means that you are not restricted to which thread you choose to do your work. (i.e. MTA)
            //The alternative is a Single Threaded Apartment (STA) model where you must interact with the system on a given thread (i.e. the UI interactions)

            //Subscribing or calling OnNext will NOT introduce multi-threading to your sequence.

            Console.WriteLine($"Starting on threadId:[{Thread.CurrentThread.ManagedThreadId}]");
            var subject = new Subject<object>();
            subject.Subscribe(o => Console.WriteLine($"Received {o} on threadId:[{Thread.CurrentThread.ManagedThreadId}]"));

            ParameterizedThreadStart notify = obj =>
            {
                Console.WriteLine($"OnNext({obj}) on threadId:[{Thread.CurrentThread.ManagedThreadId}]");
                subject.OnNext(obj);
            };

            notify(1);
            new Thread(notify).Start(2);
            new Thread(notify).Start(3);
        }

        public static void RxIsSynchronousByDefault()
        {
            Console.WriteLine($"Starting on [{Thread.CurrentThread.ManagedThreadId}]");

            var source = Observable.Create<int>(
                o =>
                {
                    Console.WriteLine($"Invoked on [{Thread.CurrentThread.ManagedThreadId}]");
                    o.OnNext(1);
                    o.OnNext(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Console.WriteLine($"Finished on [{Thread.CurrentThread.ManagedThreadId}]");

                    return Disposable.Empty;
                });

            source.Subscribe(
                    o => Console.WriteLine($"Processing {o} on [{Thread.CurrentThread.ManagedThreadId}]"),
                    () => Console.WriteLine($"Completing on [{Thread.CurrentThread.ManagedThreadId}]"));

            Console.WriteLine($"Post-subscription processing on [{Thread.CurrentThread.ManagedThreadId}]");
        }

        public static void SubscribeOn_WillScheduleTheSubscriptionWarmUpCode_OnTheReceivedScheduler_MakingPostSubscriptionProcessingNonBlocking()
        {
            Console.WriteLine($"Starting on [{Thread.CurrentThread.ManagedThreadId}]");

            var source = Observable.Create<int>(
                o =>
                {
                    Console.WriteLine($"Invoked on [{Thread.CurrentThread.ManagedThreadId}]");
                    o.OnNext(1);
                    o.OnNext(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Console.WriteLine($"Finished on [{Thread.CurrentThread.ManagedThreadId}]");

                    return Disposable.Empty;
                });
            source
                .SubscribeOn(Scheduler.Default)
                .Subscribe(
                    o => Console.WriteLine($"Processing {o} on [{Thread.CurrentThread.ManagedThreadId}]"),
                    () => Console.WriteLine($"Completing on [{Thread.CurrentThread.ManagedThreadId}]"));

            Console.WriteLine($"Post-subscription processing on [{Thread.CurrentThread.ManagedThreadId}]");
        }

        public static void SubscribeOn_WillNotChangeTheThreadForProcessingByDefault()
        {
            Console.WriteLine($"Starting on [{Thread.CurrentThread.ManagedThreadId}]");
            var source = Observable.Create<int>(
                o =>
                {
                    ParameterizedThreadStart notify = obj =>
                    {
                        Console.WriteLine($"Emitting OnNext({obj}) on [{Thread.CurrentThread.ManagedThreadId}]");
                        o.OnNext((int)obj);
                    };

                    Console.WriteLine($"Invoked Create on [{Thread.CurrentThread.ManagedThreadId}]");
                    new Thread(notify).Start(1);
                    new Thread(notify).Start(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Console.WriteLine($"Finished Create on [{Thread.CurrentThread.ManagedThreadId}]");

                    return Disposable.Empty;
                });
            source
                .SubscribeOn(Scheduler.Default)
                .Subscribe(
                    o => Console.WriteLine($"Processing {o} on [{Thread.CurrentThread.ManagedThreadId}]"),
                    () => Console.WriteLine($"Completing on [{Thread.CurrentThread.ManagedThreadId}]"));

            Console.WriteLine($"Post-subscription processing on [{Thread.CurrentThread.ManagedThreadId}]");
        }

        public static void ObserveOn_WillScheduleProcessing_OnTheReceivedScheduler_MakingSubscriptionProcessingNonBlocking()
        {
            Console.WriteLine($"Starting on [{Thread.CurrentThread.ManagedThreadId}]");
            var source = Observable.Create<int>(
                o =>
                {
                    ParameterizedThreadStart notify = obj =>
                    {
                        Console.WriteLine($"Emitting OnNext({obj}) on [{Thread.CurrentThread.ManagedThreadId}]");
                        o.OnNext((int)obj);
                    };

                    Console.WriteLine($"Invoked Create on [{Thread.CurrentThread.ManagedThreadId}]");
                    new Thread(notify).Start(1);
                    new Thread(notify).Start(2);
                    o.OnNext(3);
                    o.OnCompleted();
                    Console.WriteLine($"Finished Create on [{Thread.CurrentThread.ManagedThreadId}]");

                    return Disposable.Empty;
                });
            source
                .SubscribeOn(Scheduler.Default)
                .ObserveOn(NewThreadScheduler.Default)
                .Subscribe(
                    o => Console.WriteLine($"Processing {o} on [{Thread.CurrentThread.ManagedThreadId}]"),
                    () => Console.WriteLine($"Completing on [{Thread.CurrentThread.ManagedThreadId}]"));

            Console.WriteLine($"Post-subscription processing on [{Thread.CurrentThread.ManagedThreadId}]");
        }

        public static void ImmediateScheduler_DoesntActuallyDoAnyScheduling_MakingStuffBlocking_DeadlockMaterial()
        {
            //Execution Context: Current Thread
            //Execution Policy: Immediate
            //Clock: Machine Time

            Console.WriteLine("Before");
            Observable.Interval(TimeSpan.FromMilliseconds(250), ImmediateScheduler.Instance).Take(6).Dump("Interval on ImmediateScheduler");
            Console.WriteLine("After");
        }

        public static void CurrentThreadScheduler_QueuesSuccessivelyScheduledActions_EffectivelyDoingOutOfOrderExecution_ButNoMoreDeadlocks()
        {
            //Execution Context: Current Thread
            //Execution Policy: FIFO (trampolined  / message queue)
            //Clock: Machine Time

            Console.WriteLine("Before");
            Observable.Interval(TimeSpan.FromMilliseconds(250), CurrentThreadScheduler.Instance).Take(6).Dump("Interval on CurrentThreadScheduler");
            Console.WriteLine("After");
        }

        public static void ScheduleTasks(IScheduler scheduler)
        {
            Action leafAction = () =>
            {
                Get.CurrentThread();
                Console.WriteLine("----leafAction.");
            };
            Action innerAction = () =>
            {
                Get.CurrentThread();
                Console.WriteLine("--innerAction start.");
                scheduler.Schedule(leafAction);
                Console.WriteLine("--innerAction end.");
            };
            Action outerAction = () =>
            {
                Get.CurrentThread();
                Console.WriteLine("outer start.");
                scheduler.Schedule(innerAction);
                Console.WriteLine("outer end.");
            };
            scheduler.Schedule(outerAction);
        }

        public static void CurrentThreadScheduler_vs_ImmediateScheduler()
        {
            ScheduleTasks(CurrentThreadScheduler.Instance);
            /*Output:
            outer start. on the specified thread
            outer end.
            --innerAction start.
            --innerAction end.
            ----leafAction.
            */

            Console.ReadLine();
            ScheduleTasks(ImmediateScheduler.Instance);
            /*Output:
            outer start.
            --innerAction start.
            ----leafAction.
            --innerAction end.
            outer end.
            */
        }

        public static void EventLoopScheduler_IsSimilarToCurrentThreadScheduler_ButYouProvideTheThread()
        {
            //Execution Context: Dedicated (i.e. provided) Thread
            //Execution Policy: FIFO (trampolined  / message queue)
            //Clock: Machine Time

            Console.WriteLine("Before");
            Observable.Using(
                () => new EventLoopScheduler(),
                els => Observable.Interval(TimeSpan.FromMilliseconds(250), els).Take(6))
                .Dump("Interval on EventLoopScheduler");
            Console.WriteLine("After");
        }

        public static void EventLoopScheduler_vs_CurrentThreadScheduler()
        {
            ScheduleTasks(new EventLoopScheduler());
            /*Output:
            outer start. on specified thread
            outer end.
            --innerAction start. on specified thread
            --innerAction end.
            ----leafAction. on specified thread
            */

            Console.ReadLine();

            ScheduleTasks(CurrentThreadScheduler.Instance);
            /*Output:
            outer start.
            outer end.
            --innerAction start.
            --innerAction end.
            ----leafAction.
            */
        }

        public static void NewThreadScheduler_BuildsNewEventLoopSchedulers_ForEachTopLevelScheduledActions()
        {
            //Execution Context: Dedicated (i.e. provided) Thread
            //Execution Policy: FIFO (trampolined  / message queue)
            //Clock: Machine Time

            Console.WriteLine("Before");
            var newThreadScheduler = new NewThreadScheduler();

            ScheduleTasksWithPassedOnScheduler(newThreadScheduler);
            /*Output:
            outer start. on thread T0
            outer end.
            --innerAction start. on thread T0
            --innerAction end.
            ----leafAction. on thread T0
            */

            Console.ReadLine();
            ScheduleTasksWithPassedOnScheduler(newThreadScheduler);
            /*Output:
            outer start. on thread T1
            outer end.
            --innerAction start. on thread T1
            --innerAction end.
            ----leafAction. on thread T1
            */

            Console.WriteLine("After");
        }

        public static void ScheduleTasksWithPassedOnScheduler(IScheduler scheduler)
        {
            Func<IScheduler, string, IDisposable> leafAction = (sch, state) =>
            {
                Get.CurrentThread();
                Console.WriteLine("----leafAction. " + state);

                return Disposable.Empty;
            };
            Func<IScheduler, string, IDisposable> innerAction = (sch, state) =>
            {
                Get.CurrentThread();
                Console.WriteLine("--innerAction start.");
                sch.Schedule(state, leafAction);
                Console.WriteLine("--innerAction end.");

                return Disposable.Empty;
            };
            Func<IScheduler, string, IDisposable> outerAction = (sch, state) =>
            {
                Get.CurrentThread();
                Console.WriteLine("outer start.");
                sch.Schedule(state, innerAction);
                Console.WriteLine("outer end.");

                return Disposable.Empty;
            };

            scheduler.Schedule("", outerAction);
        }

        public static void ThreadPoolScheduler_SchedulesStuffASAP_OnTheThreadPool()
        {
            //Execution Context: Thread Pool
            //Execution Policy: ASAP (as opposed to immediate)
            //Clock: Machine Time

            Console.WriteLine("Before");

            ScheduleTasksWithPassedOnScheduler(ThreadPoolScheduler.Instance);
            ScheduleTasksWithPassedOnScheduler(ThreadPoolScheduler.Instance);
            ScheduleTasksWithPassedOnScheduler(ThreadPoolScheduler.Instance);

            Console.WriteLine("After");
        }

        public static void TaskPoolScheduler_LikeTheThreadPoolScheduler_DoesntGuaranteeSameThreadForNestedActions()
        {
            //Execution Context: Thread Pool
            //Execution Policy: ASAP (as opposed to immediate)
            //Clock: Machine Time

            Console.WriteLine("Before");

            ScheduleTasksWithPassedOnScheduler(TaskPoolScheduler.Default);
            ScheduleTasksWithPassedOnScheduler(TaskPoolScheduler.Default);
            ScheduleTasksWithPassedOnScheduler(TaskPoolScheduler.Default);

            Console.WriteLine("After");

            //Favor this over the ThreadPoolScheduler (if available on your target platform)
        }

        public static void DispatcherScheduler_CanBeUsedToEmitAndConsume_OnTheUIThread(Form form)
        {
            //Execution Context: UI Thread
            //Execution Policy: Priority FIFO (non-blocking)
            //Clock: Machine Time

            Observable.Interval(TimeSpan.FromMilliseconds(250), new DispatcherScheduler(Dispatcher.CurrentDispatcher))
                .Take(6)
                .Subscribe(i => form.AppendToBox($"Blipp{i}"));

            form.AppendToBox("Finished");
        }
    }
}
