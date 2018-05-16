using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Windows.Forms;

namespace RxWorkshop
{
    public static class ReducingSequences
    {
        public static void Where_FilteringStuffIsEasyPeasy()
        {
            Observable.Range(0, 10).Where(i => i % 2 == 0)
                                    .Subscribe(i => Console.WriteLine($"Passed the predicate: {i}"),
                                               () => Console.WriteLine("Completed."));
        }

        public static void Distinct_HoldsNoSurprises()
        {
            var subject = new Subject<int>();
            var distinct = subject.Distinct();
            subject.Subscribe(i => Console.WriteLine("{0}", i),
                              () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(i => Console.WriteLine($"\tdistinct.OnNext({i})"),
                               () => Console.WriteLine("\tdistinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        public static void Distinct_ComesInAKeySelectorFlavor()
        {
            var subject = new Subject<MouseEventArgs>();
            var distinct = subject.Distinct(mea => mea.Button);
            distinct.Subscribe(i => Console.WriteLine($"\tdistinct.OnNext({i.Button} {i.Clicks} {i.X} {i.Y} {i.Delta})"),
                               () => Console.WriteLine("\tdistinct.OnCompleted()"));

            subject.OnNext(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            subject.OnNext(new MouseEventArgs(MouseButtons.Right, 1, 10, 15, 0));
            subject.OnNext(new MouseEventArgs(MouseButtons.Left, 1, 24, 65, 0));
            subject.OnNext(new MouseEventArgs(MouseButtons.Middle, 1, 543, 76, 0));
            subject.OnNext(new MouseEventArgs(MouseButtons.Right, 1, 756, 87, 0));
            subject.OnNext(new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
            subject.OnCompleted();
        }

        public static void DistinctUntilChanged_PerformsPairwiseDistinct_NoTwoInARow()
        {
            var subject = new Subject<int>();
            var distinct = subject.DistinctUntilChanged();
            subject.Subscribe(i => Console.WriteLine($"{i}"),
                              () => Console.WriteLine("subject.OnCompleted()"));
            distinct.Subscribe(i => Console.WriteLine($"distinct.OnNext({i})"),
                               () => Console.WriteLine("distinct.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();
        }

        public static void IgnoreElements_UseItWhenYouOnlyCareAboutCompletion()
        {
            var subject = new Subject<int>();
            subject.IgnoreElements()
                    .Subscribe(i => Console.WriteLine($"{i}"),
                               () => Console.WriteLine("subject.OnCompleted()"));
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(4);
            subject.OnCompleted();

            var erroredSubject = new Subject<int>();
            erroredSubject.IgnoreElements()
                            .Subscribe(i => Console.WriteLine($"{i}"),
                                       ex => { Console.WriteLine("erroredSubject.OnError()"); },
                                       () => Console.WriteLine("erroredSubject.OnCompleted()"));
            erroredSubject.OnNext(1);
            erroredSubject.OnNext(1);
            erroredSubject.OnError(new AmbiguousMatchException());
            erroredSubject.OnNext(4);
        }

        public static void Skip_SkipsAFew()
        {
            Observable.Range(0, 10)
                .Skip(3)
                .Subscribe(Console.WriteLine,
                           () => Console.WriteLine("Skip(3) completed and returned the last 7 out of 10"));

            Observable.Range(0, 10)
                .Skip(15)
                .Subscribe(Console.WriteLine,
                           () => Console.WriteLine("Skip(15) completed without returning any of the 10 elements"));
        }

        public static void Take_TakesAFew()
        {
            Observable.Range(0, 10)
                .Take(3)
                .Subscribe(Console.WriteLine,
                           () => Console.WriteLine("Take(3) completed and returned the first 3 out of 10"));

            Observable.Range(0, 10)
                .Take(15)
                .Subscribe(Console.WriteLine,
                           () => Console.WriteLine("Take(15) completed returning 10 elements"));
        }

        public static void SkipWhile_GuardsTheFrontOfTheStream()
        {
            var subject = new Subject<int>();
            subject.SkipWhile(i => i < 4)
                   .Subscribe(Console.WriteLine,
                              () => Console.WriteLine("SkipWhile completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
            subject.OnNext(3);
            subject.OnNext(2);
            subject.OnNext(1);
            subject.OnNext(0);
            subject.OnCompleted();
        }

        public static void TakeWhile_GuardsTheBackOfTheStream()
        {
            var subject = new Subject<int>();
            subject.TakeWhile(i => i < 4)
                   .Subscribe(Console.WriteLine,
                              () => Console.WriteLine("TakeWhile completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(4);
            subject.OnNext(3);
            subject.OnNext(2);
            subject.OnNext(1);
            subject.OnNext(0);
            subject.OnCompleted();
        }

        public static void SkipLast_WillBufferTheNumberOfElementsToSkip_ThenEjectOneAtATimeAfterTheBufferIsOverfilled_UntilCompletion()
        {
            var subject = new Subject<int>();
            subject.SkipLast(2)
                   .Subscribe(Console.WriteLine,
                              ex => Console.WriteLine("Something blew up"),
                              () => Console.WriteLine("SkipLast completed"));
            Console.WriteLine("Pushing 1");
            subject.OnNext(1);
            Console.WriteLine("Pushing 2");
            subject.OnNext(2);
            Console.WriteLine("Pushing 3");
            subject.OnNext(3);
            Console.WriteLine("Pushing 4");
            subject.OnNext(4);
            subject.OnCompleted();
            //subject.OnError(new EntryPointNotFoundException());
        }

        public static void TakeLast_WillBufferTheNumberOfElementsToSkip_ThenDiscardOneAtATimeAfterTheBufferIsOverfilled_UntilOnCompletedIsReceived()
        {
            var subject = new Subject<int>();
            subject.TakeLast(2)
                   .Subscribe(Console.WriteLine,
                              ex => Console.WriteLine("Something blew up"),
                              () => Console.WriteLine("TakeLast completed"));
            Console.WriteLine("Pushing 1");
            subject.OnNext(1);
            Console.WriteLine("Pushing 2");
            subject.OnNext(2);
            Console.WriteLine("Pushing 3");
            subject.OnNext(3);
            Console.WriteLine("Pushing 4");
            subject.OnNext(4);
            subject.OnCompleted();
            //subject.OnError(new NotFiniteNumberException());
        }

        public static void SkipUntil_UsesSecondObservable_AsASignalToStartConsuming()
        {
            var subject = new Subject<int>();
            var signal = new Subject<Unit>();
            subject.SkipUntil(signal)
                   .Subscribe(Console.WriteLine,
                              () => Console.WriteLine("SkipUntil completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            signal.OnNext(Unit.Default);
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }

        public static void TakeUntil_UsesSecondObservable_AsASignalToStopConsuming()
        {
            var subject = new Subject<int>();
            var signal = new Subject<Unit>();
            subject.TakeUntil(signal)
                   .Subscribe(Console.WriteLine,
                              () => Console.WriteLine("TakeUntil completed"));
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            signal.OnNext(Unit.Default);
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }
    }
}
