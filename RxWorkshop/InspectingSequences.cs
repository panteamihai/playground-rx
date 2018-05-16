using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Security.Cryptography;

namespace RxWorkshop
{
    public static class InspectingSequences
    {
        public static void Any_IsUsedToCheckForElements()
        {
            Observable.Return(42).Any().Subscribe(_ => Console.WriteLine("The sequence has elements"));
            Observable.Empty<DateTime>().Any().Subscribe(_ => { },
                                                         () => Console.WriteLine("The sequence has NO elements"));
            Observable.Never<int>().Any().Subscribe(_ => { },
                                                    () => Console.WriteLine("Never gonna hit this"));

            Console.ReadLine();
            Observable.Throw<int>(new CryptographicException()).Any().Subscribe(
                _ => { },
                ex => { Console.WriteLine("Caught an exception as the first element, cannot answer"); },
                () => Console.WriteLine("The sequence has NO elements"));

            Console.ReadLine();
            Observable.Create<string>(
                observer =>
                {
                    observer.OnNext("");
                    observer.OnError(new AmbiguousMatchException());

                    return Disposable.Create(() => Console.WriteLine("Disposed"));
                }).Any().Subscribe(_ => Console.WriteLine("The sequence has elements"));
        }

        public static void All_IsUsedToMatchAllElements_ToAPredicate()
        {
            Observable.Create<string>(
                observer =>
                {
                    observer.OnNext("rw1");
                    observer.OnNext("rwe");
                    observer.OnNext("ret");
                    observer.OnNext("rwew");
                    observer.OnNext("");
                    observer.OnCompleted();

                    return Disposable.Empty;
                }).All(s => !string.IsNullOrEmpty(s))
                  .Subscribe(answer => Console.WriteLine($"All values match predicate: {answer}"));
        }

        public static void Contains_IsBasicallyAnAnyImplementation_ThatTargetsOneSpecificValue_AsOpposedToACategoryOfValuesThatFitAPredicate()
        {
            Observable.Create<string>(
                observer =>
                {
                    observer.OnNext("rw1");
                    observer.OnNext("rwe");
                    observer.OnNext("ROT");
                    observer.OnNext("rwew");
                    observer.OnNext("");
                    observer.OnCompleted();

                    return Disposable.Empty;
                }).Contains("rot", StringComparer.CurrentCultureIgnoreCase)
                  .Subscribe(answer => Console.WriteLine($"Contains \"rot\": {answer}"));
        }

        public static void DefaultIfEmpty_HandlesTheMissingValuesCasesGracefully()
        {
            Observable.Empty<DateTime>().DefaultIfEmpty().Subscribe(d => Console.WriteLine($"Date is: {d}"));
            Observable.Empty<DateTime>().DefaultIfEmpty(DateTime.Now).Subscribe(d => Console.WriteLine($"Date is: {d}"));

            Console.ReadLine();
            Observable.Create<string>(
                observer =>
                {
                    observer.OnNext("rw1");
                    observer.OnNext("rwe");
                    observer.OnNext("ROT");
                    observer.OnNext("rwew");
                    observer.OnNext("_");
                    observer.OnCompleted();

                    return Disposable.Create(() => Console.WriteLine("Everything passed through"));
                }).DefaultIfEmpty().Subscribe(Console.WriteLine);
        }

        public static void ElementAt_CherryPicksBasedOnPosition()
        {
            var source = Observable.Create<int>(
                observer =>
                {
                    observer.OnNext(432);
                    observer.OnNext(93);
                    observer.OnNext(782);
                    observer.OnNext(1);
                    observer.OnNext(239);
                    observer.OnCompleted();

                    return Disposable.Empty;
                });

            source.ElementAt(2).Subscribe(v => Console.WriteLine($"3rd value is {v}"));

            Console.ReadLine();
            source.ElementAt(5).Subscribe(v => Console.WriteLine($"6th value is {v}"), ex => Console.WriteLine("You're out of your league here man."));

            Console.ReadLine();
            source.Skip(5).Take(1).Subscribe(v => Console.WriteLine($"6th value is {v}"), () => Console.WriteLine("Finished skiping and taking. Did you get any value?"));

            Console.ReadLine();
            source.ElementAtOrDefault(5).Subscribe(v => Console.WriteLine($"6th value is {v}"));

            Console.ReadLine();
            source.ElementAt(5).Catch(Observable.Return(-1)).Subscribe(v => Console.WriteLine($"6th value is {v}"));
        }

        public static void SequenceEqual_WillTakeOrderIntoAccountAsWell()
        {
            var subject1 = new Subject<int>();
            subject1.Subscribe(
                i => Console.WriteLine("subject1.OnNext({0})", i),
                () => Console.WriteLine("subject1 completed"));
            var subject2 = new Subject<int>();
            subject2.Subscribe(
                i => Console.WriteLine("subject2.OnNext({0})", i),
                () => Console.WriteLine("subject2 completed"));
            var areEqual = subject1.SequenceEqual(subject2);
            areEqual.Subscribe(
                i => Console.WriteLine("areEqual.OnNext({0})", i),
                () => Console.WriteLine("areEqual completed"));
            subject1.OnNext(1);
            subject1.OnNext(2);
            subject2.OnNext(1);
            subject2.OnNext(2);
            subject2.OnNext(3);
            subject1.OnNext(3);
            subject1.OnCompleted();
            subject2.OnCompleted();
        }
    }
}
