using RxWorkshop.Implementations;
using System;
using System.Reactive.Subjects;

namespace RxWorkshop
{
    public class KeyTypes
    {
        public static void ObservableObserverPair()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var synchronousObservable = new SynchronousObservable();

            //always get the subscription
            var subscription = synchronousObservable.Subscribe(consoleObserver);

            //can't do something in the mean time (while subscribed to a synchronous observable)
            Console.WriteLine("This is executed only after the whole subscription fiasco");

            //remeber to dispose of the subscription => See LifetimeManagement
            subscription.Dispose();
        }

        public static void UsingSynchronousObservables()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var synchronousObservable = new SynchronousObservable();
            var stillSynchronousObservable = new SpacedOutButStillSynchronousObservable();

            var subscription = synchronousObservable.Subscribe(consoleObserver);
            var subscription2 = stillSynchronousObservable.Subscribe(consoleObserver);

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void NaiveSubjectImplementation()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var naiveSubject = new NaiveSubject<int>();
            var subscription = naiveSubject.Subscribe(consoleObserver);

            naiveSubject.OnNext(4);
            naiveSubject.OnNext(5);
            naiveSubject.OnError(new Exception("Custom"));
            //This shouldn't happen in a proper implementation (no more values after OnError / OnCompleted)
            naiveSubject.OnNext(6);

            subscription.Dispose();
            //This shouldn't happen in a proper implementation either.
            naiveSubject.OnNext(7);
        }

        public static void WorkingWithAnActualSubject()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var subject = new Subject<int>();
            var subscription = subject.Subscribe(consoleObserver);

            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnError(new Exception("Custom"));
            //This won't appear because of the contract of IObservable
            subject.OnNext(6);

            subscription.Dispose();
        }

        public static void TimeOfSubscriptionActuallyMattersForSubjects()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var subject = new Subject<int>();
            var subscription = subject.Subscribe(consoleObserver);

            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);

            var consoleObserver2 = new ConsoleObserver<int>();
            var subscription2 = subject.Subscribe(consoleObserver2);

            subject.OnNext(7);
            subject.OnNext(8);

            subject.OnCompleted();

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void HowAboutACache()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var replaySubject = new ReplaySubject<int>();
            var subscription = replaySubject.Subscribe(consoleObserver);

            replaySubject.OnNext(4);
            replaySubject.OnNext(5);
            replaySubject.OnNext(6);

            var consoleObserver2 = new ConsoleObserver<int>();
            var subscription2 = replaySubject.Subscribe(consoleObserver2);

            replaySubject.OnCompleted();

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void CacheItEvenAfterCompletion()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var replaySubject = new ReplaySubject<int>();
            var subscription = replaySubject.Subscribe(consoleObserver);

            replaySubject.OnNext(4);
            replaySubject.OnNext(5);
            replaySubject.OnNext(6);
            replaySubject.OnCompleted();

            var consoleObserver2 = new ConsoleObserver<int>();
            var subscription2 = replaySubject.Subscribe(consoleObserver2);

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void TheDefaultValueForASubscriptionBeforeEmittingAValue()
        {
            var subject = new BehaviorSubject<int>(-1);

            var consoleObserver = new ConsoleObserver<int>();
            Console.WriteLine("We're gonna subscribe once");
            var subscription = subject.Subscribe(consoleObserver);

            Console.WriteLine("First value");
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);

            var consoleObserver2 = new ConsoleObserver<int>();
            Console.WriteLine("We're gonna subscribe a second time");
            var subscription2 = subject.Subscribe(consoleObserver2);

            subject.OnNext(7);
            subject.OnNext(8);

            subject.OnCompleted();

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void TheLastValueBeforeSubscription()
        {
            var subject = new BehaviorSubject<int>(-1);

            Console.WriteLine("First value");
            subject.OnNext(4);

            var consoleObserver = new ConsoleObserver<int>();
            Console.WriteLine("We're gonna subscribe once");
            var subscription = subject.Subscribe(consoleObserver);

            subject.OnNext(5);
            subject.OnNext(6);

            var consoleObserver2 = new ConsoleObserver<int>();
            Console.WriteLine("We're gonna subscribe a second time");
            var subscription2 = subject.Subscribe(consoleObserver2);

            subject.OnNext(7);
            subject.OnNext(8);

            subject.OnCompleted();

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void TheLastValueBeforeCompletion()
        {
            var subject = new AsyncSubject<int>();
            var consoleObserver = new ConsoleObserver<int>();
            var subscription = subject.Subscribe(consoleObserver);

            var observer2 = new ConsoleObserver<int>();
            var subscription2 = subject.Subscribe(observer2);

            subject.OnNext(9);
            subject.OnCompleted();

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void UnlessThereIsNone_ButAtLeastItFinishes()
        {
            var subject = new AsyncSubject<int>();
            var consoleObserver = new ConsoleObserver<int>();
            var subscription = subject.Subscribe(consoleObserver);

            var consoleObserver2 = new ConsoleObserver<int>();
            var subscription2 = subject.Subscribe(consoleObserver2);

            subject.OnCompleted();
            //This won't appear because of the contract of IObservable
            subject.OnNext(9);

            subscription.Dispose();
            subscription2.Dispose();
        }

        public static void NotCompletingAnAsyncSubject_WillNotPushAnyValueOut()
        {
            var subject = new AsyncSubject<int>();
            var consoleObserver = new ConsoleObserver<int>();
            var subscription = subject.Subscribe(consoleObserver);

            var consoleObserver2 = new ConsoleObserver<int>();
            var subscription2 = subject.Subscribe(consoleObserver2);

            subject.OnNext(9);

            subscription.Dispose();
            subscription2.Dispose();
        }
    }
}
