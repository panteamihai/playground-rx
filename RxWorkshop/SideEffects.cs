using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxWorkshop
{
    public static class SideEffects
    {
        public static void IntroducingAndManipulatingState_IsVeryBadForComprehension()
        {
            var letters = Observable.Range(1, 34).Select(i => i % 3 == 0 ? (char)(i + 64) : 'X');

            var index = -1;
            var result = letters.Select(c => { index++; return c; });

            result.Subscribe(
                l => Console.WriteLine($"Got {l} @ {index}"),
                () => Console.WriteLine("Done."));

            Console.ReadLine();
            result.Subscribe(
                l => Console.WriteLine($"Some more {l} @ {index}"),
                () => Console.WriteLine("Done."));
        }

        public static void IfIndexIsWhatInterestsYou_GoForTheFunctionalApproach_WithTheSelectOverload()
        {
            var result = Observable.Range(1, 34).Select((i, idx) =>
                new { Letter = i % 3 == 0 ? (char)(i + 64) : 'X', Index = idx });

            result.Subscribe(
                l => Console.WriteLine($"Got {l.Letter} @ {l.Index}"),
                () => Console.WriteLine("Done."));

            Console.ReadLine();
            result.Subscribe(
                l => Console.WriteLine($"Some more {l.Letter} @ {l.Index}"),
                () => Console.WriteLine("Done."));
        }

        public static void YouCanEncapsulateState_InsideThePipeline_UsingScan()
        {
            var source = Observable.Range(1, 34);
            var result = source.Scan(
                new
                {
                    Index = -1,
                    Letter = new char()
                },
                (acc, value) => new
                                {
                                    Index = acc.Index + 1,
                                    Letter = value % 3 == 0 ? (char)(value + 64) : 'X'
                });
            result.Subscribe(
                x => Console.WriteLine("Received {0} at index {1}", x.Letter, x.Index),
                () => Console.WriteLine("completed"));

            Console.ReadLine();
            result.Subscribe(
                x => Console.WriteLine("Also received {0} at index {1}", x.Letter, x.Index),
                () => Console.WriteLine("2nd completed"));
        }

        public static void EnhanceSequenceManipulation_UsingDo()
        {
            var source = Observable
                .Interval(TimeSpan.FromMilliseconds(290))
                .Take(14);

            var result = source.Do(Log, Log, Log);
            result.Subscribe(
                i => Console.WriteLine($"Subscription value: {i}"),
                () => Console.WriteLine("Subscription completed"));
        }

        private static void Log<T>(T t) { Console.WriteLine($"Logging value: {t}"); }

        private static void Log(Exception ex) { Console.WriteLine($"Logging error: {ex.Message}"); }

        private static void Log() { Console.WriteLine("Logging completion"); }

        public static void YouCanDoNastyStuff_ToReferenceValues_WithDo()
        {
            //reference types
            var refSource = new Subject<Account>();
            refSource.Do(account => account.Name = "Garbage")
                     .Subscribe(
                        account => Console.WriteLine($"Account: {account.Id} {account.Name}"),
                        () => Console.WriteLine("Ref source completed"));
            refSource.OnNext(new Account { Id = 1, Name = "Microsoft" });
            refSource.OnNext(new Account { Id = 2, Name = "Google" });
            refSource.OnNext(new Account { Id = 3, Name = "IBM" });
            refSource.OnCompleted();

            Console.ReadLine();

            //value types
            var valSource = Observable
                .Interval(TimeSpan.FromMilliseconds(700))
                .Take(4);

            var result = valSource.Do(i => i = i + 1);
            result.Subscribe(
                i => Console.WriteLine($"Value: {i}"),
                () => Console.WriteLine("Val source completed"));
        }

        public class Account
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
