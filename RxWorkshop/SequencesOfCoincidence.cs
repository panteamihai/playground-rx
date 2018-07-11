using RxWorkshop.Extensions;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxWorkshop
{
    public static class SequencesOfCoincidence
    {
        public static void Window_IntoTheSoul()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(350)).Take(20);

            //IObservable<IObservable<T>> FTFW!!
            source.Window(3).WindowedDump("Window");
        }

        public static void Window_FlatteningBackIntoTheOriginalSequence_UsingSwitchMergeOrConcat_RegardlessOfWhich()
        {
            var coldWindowedSource = Observable.Interval(TimeSpan.FromMilliseconds(200))
                                       .Timestamp()
                                       .Do(i => Console.WriteLine($"Origin: {i}"))
                                       .Take(10)
                                       .Window(TimeSpan.FromMilliseconds(500));

            coldWindowedSource.Switch().Timestamp().Dump("Switched window");

            Console.ReadLine();
            coldWindowedSource.Concat().Timestamp().Dump("Concated window");

            Console.ReadLine();
            coldWindowedSource.Merge().Timestamp().Dump("Merged window");
        }

        public static void Window_CustomClosingWindowMechanism()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Take(25);
            var closer = new Subject<Unit>();

            using (source.Window(() => closer).WindowedDump("Window"))
            {
                ConsoleKey input;
                do
                {
                    input = Console.ReadKey().Key;
                    closer.OnNext(Unit.Default);
                }
                while (input != ConsoleKey.Escape);
            }
        }

        public static void Window_NaiveImplementationOfFixedSizeWindowingOperator()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(750)).Take(25);

            source.NaiveWindow(4).WindowedDump("Naive windowing");
        }

        public static void Window_NaiveImplementationOfFixedSizeWindowingOperator_WithSkip()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(500)).Take(25);

            source.NaiveWindow(2, 1).WindowedDump("Naive pairwise");
        }

        public static void Window_NaiveImplementationOfFixedSizeBufferingOperator()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(25);

            source.NaiveBuffer(4).Dump("Naive buffer", l => string.Join(",", l));
        }

        public static void Join_AllowsYouToJoinSequencesByIntersectingWindows_LikeInANaiveCombineLatest()
        {
            var left = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(20);
            var right = Observable.Interval(TimeSpan.FromMilliseconds(350)).Take(10);

            left.NaiveCombineLatest(right, (l, r) => $"[{l},{r}]").Dump("Naive combine latest");
        }

        public static void GroupJoin_NaiveImplementationOfJoin()
        {
            var left = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(20);
            var right = Observable.Interval(TimeSpan.FromMilliseconds(350)).Take(10);

            left.NaiveJoin(right, _ => Observable.Never<Unit>(), _ => Observable.Empty<Unit>(), (l, r) => $"[{l},{r}]").Dump("Naive join");
            //left.NaiveJoin(right, _ => Observable.Empty<Unit>(), _ => Observable.Never<Unit>(), (l, r) => $"[{l},{r}]").Dump("Naive join");
        }
    }
}