using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace RxWorkshop
{
    public class KeyTypes
    {
        static void Main()
        {
            //ObservableObserverPair();
            //UsingSynchronousObservables();
            //NaiveSubjectImplementation();
            //WorkingWithAnActualSubject();
            //TimeOfSubscriptionActuallyMattersForSubjects();
            //HowAboutACache();
            //CacheItEvenAfterCompletion();
            //TheDefaultValueForASubscriptionBeforeEmittingAValue();
            //TheLastValueBeforeSubscription();
            //TheLastValueBeforeCompletion();
            //UnlessThereIsNone_ButAtLeastItFinishes();
            //LikeIfItWereInfinite();

            Console.Read();
        }

        private static void ObservableObserverPair()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var synchronousObservable = new SynchronousObservable();

            //always get the subscription
            var subscription = synchronousObservable.Subscribe(consoleObserver);

            //can't do something in the mean time (while subscribed to a synchronous observable)
            Console.WriteLine("This is executed only after the whole subscription fiasco");
        }

        private static void UsingSynchronousObservables()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var synchronousObservable = new SynchronousObservable();
            var stillSynchronousObservable = new SpacedOutButStillSynchronousObservable();

            var subscription = synchronousObservable.Subscribe(consoleObserver);
            var subscriptionSO = stillSynchronousObservable.Subscribe(consoleObserver);

            subscription.Dispose();
            subscriptionSO.Dispose();
        }

        private static void NaiveSubjectImplementation()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var naiveSubject = new NaiveSubject<int>();
            var subscription = naiveSubject.Subscribe(consoleObserver);

            naiveSubject.OnNext(4);
            naiveSubject.OnNext(5);
            naiveSubject.OnError(new Exception("Custom"));
            //This shouldn't happen in a proper implementation (no more values after OnError / OnCompleted)
            naiveSubject.OnNext(6);
        }

        private static void WorkingWithAnActualSubject()
        {
            var consoleObserver = new ConsoleObserver<int>();
            var subject = new Subject<int>();
            var subscription = subject.Subscribe(consoleObserver);

            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnError(new Exception("Custom"));
            //This won't appear because of the contract of IObservable
            subject.OnNext(6);
        }

        private static void TimeOfSubscriptionActuallyMattersForSubjects()
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
        }

        private static void HowAboutACache()
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
        }

        private static void CacheItEvenAfterCompletion()
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
        }

        private static void TheDefaultValueForASubscriptionBeforeEmittingAValue()
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
        }

        private static void TheLastValueBeforeSubscription()
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
        }

        private static void TheLastValueBeforeCompletion()
        {
            var subject = new AsyncSubject<int>();
            var consoleObserver = new ConsoleObserver<int>();
            var subscription = subject.Subscribe(consoleObserver);

            var observer2 = new ConsoleObserver<int>();
            var subscriptionSubject2 = subject.Subscribe(observer2);

            subject.OnNext(9);
            subject.OnCompleted();
        }

        private static void UnlessThereIsNone_ButAtLeastItFinishes()
        {
            var subject = new AsyncSubject<int>();
            var consoleObserver = new ConsoleObserver<int>();
            var subscription = subject.Subscribe(consoleObserver);

            var observer2 = new ConsoleObserver<int>();
            var subscriptionSubject2 = subject.Subscribe(observer2);

            subject.OnCompleted();
            //This won't appear because of the contract of IObservable
            subject.OnNext(9);
        }

        private static void LikeIfItWereInfinite()
        {
            var subject = new AsyncSubject<int>();
            var consoleObserver = new ConsoleObserver<int>();
            var subscription = subject.Subscribe(consoleObserver);

            var observer2 = new ConsoleObserver<int>();
            var subscriptionSubject2 = subject.Subscribe(observer2);

            subject.OnNext(9);
        }

        public class ConsoleObserver<T> : IObserver<T>
        {
            public void OnNext(T value)
            {
                Console.WriteLine("Received value {0}", value);
            }
            public void OnError(Exception error)
            {
                Console.WriteLine("Sequence faulted with {0}", error);
            }
            public void OnCompleted()
            {
                Console.WriteLine("Sequence terminated");
            }
        }

        public class SynchronousObservable : IObservable<int>
        {
            public IDisposable Subscribe(IObserver<int> observer)
            {
                observer.OnNext(1);
                observer.OnNext(2);
                observer.OnNext(3);
                observer.OnCompleted();
                return Disposable.Empty;
            }
        }

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
}
