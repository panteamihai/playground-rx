using RxWorkshop.Extensions;
using RxWorkshop.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RxWorkshop
{
    public static class TimeshiftedSequences
    {
        private static Func<IList<long>, string> _bufferProjection = list => $"[{string.Join(",", list)}]";

        public static void Buffer_CanUseSizeTrigger_TheSimplestWayForPairwise()
        {
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                      .Buffer(2, 1)
                      .Take(10)
                      .Subscribe(
                            p => Console.WriteLine($"Previous: {p[0]}, Current: {p[1]}"),
                            () => Console.WriteLine("Compleeteed"));
        }

        public static void Buffer_CanUseTimeTrigger()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(450)).Take(28);
            source.Dump("Source");

            source
                .Buffer(TimeSpan.FromMilliseconds(1700))
                .Take(10)
                .Dump("Buffer", list => $"Buffer: {string.Join(",", list)}");
        }

        public static void Buffer_CanBeUsedWithEitherATimeOrSizeTrigger()
        {
            var idealBatchSize = 15;
            var maxTimeDelay = TimeSpan.FromSeconds(3);
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10)
                                   .Concat(Observable.Interval(TimeSpan.FromSeconds(0.01)).Take(100));

            Get.Now();
            source.Buffer(maxTimeDelay, idealBatchSize)
                .Subscribe(
                    buffer => Console.WriteLine($"Buffer of {buffer.Count} @ {DateTime.Now}"),
                    () => Console.WriteLine("Completed"));
        }

        public static void Buffer_Overlapping_WhenSkipIsLessThanCount()
        {
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Buffer(3, 1)
                .Take(15)
                .Dump("Buffer", list => $"Buffer: {string.Join(",", list)}");
        }

        public static void Buffer_Rolling_WhenSkipIsEqualToCount()
        {
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Buffer(2, 2)
                .Take(6)
                .Dump("Buffer", list => $"Buffer: {string.Join(",", list)}");
        }

        public static void Buffer_Skipped_WhenSkipIsGreaterThanCount()
        {
            Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Buffer(2, 3)
                .Take(3)
                .Dump("Buffer", _bufferProjection);
        }

        public static void Buffer_TimeTriggeredSequences_OverlappingRollingSkipped()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(200)).Take(10);
            var bufferSpan = TimeSpan.FromMilliseconds(600); // Not 100% accurate time keeping

            var overlapped = source.Buffer(bufferSpan, TimeSpan.FromMilliseconds(200)).Dump("Overlapped", _bufferProjection);
            Console.ReadLine();
            var rolling = source.Buffer(bufferSpan, bufferSpan).Dump("Rolling", _bufferProjection);
            Console.ReadLine();
            var skipped = source.Buffer(bufferSpan, TimeSpan.FromSeconds(5)).Dump("Skipped", _bufferProjection);
        }

        public static void Delay_WillTimeshift_ThusPreservingIntervalsBetweenItemsInSequence()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Timestamp();
            var delay = source.Delay(TimeSpan.FromSeconds(2));
            source.Dump("Source");
            delay.Dump("Delayed");
        }

        public static void Sample_TamesOverproducingSequences_Backpressure()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(250)).Timestamp().Take(25);
            source.Dump("Source");

            source.Sample(TimeSpan.FromSeconds(1)).Timestamp().Dump("Sampled");
        }

        public static void Throttle_GuardsAgainstPeaks_IsGreatForUserInteraction(Form form)
        {
            var moves = Observable.FromEventPattern<EventArgs>(form, nameof(form.Move));
            moves.Sample(TimeSpan.FromMilliseconds(500))
                 .Timestamp()
                 .ObserveOn(SynchronizationContext.Current)
                 .Subscribe(m => form.AppendToBox($"Still moving it around, last one @{m.Timestamp}"));
            moves.Throttle(TimeSpan.FromMilliseconds(750))
                .Timestamp()
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(i => form.AppendToBox($"Blipped @{i.Timestamp}"), ex => form.AppendToBox(ex.Message));
        }

        public static void Timeout_WillTimeOutOnVariableRateSequences()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(10)
                    .Concat(Observable.Interval(TimeSpan.FromSeconds(2)));
                    //.Merge(Observable.Interval(TimeSpan.FromSeconds(2)));
            var timeout = source.Timeout(TimeSpan.FromSeconds(1));
            timeout.Subscribe(
                Console.WriteLine,
                Console.WriteLine,
                () => Console.WriteLine("Completed"));
        }

        public static void Timeout_CanBeAbsolute()
        {
            var dueDate = DateTimeOffset.Now.AddSeconds(4);
            var source = Observable.Interval(TimeSpan.FromSeconds(1));
            var timeout = source.Timeout(dueDate);
            timeout.Dump("Abs TO");
        }

        public static void Timeout_CanContinueWithAnotherSequence()
        {
            var fastObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
            var slowObservable = Observable.Interval(TimeSpan.FromSeconds(2)).Take(3);
            var source = fastObservable.Concat(slowObservable);

            var timeoutSpan = TimeSpan.FromSeconds(1.5);
            var continuationObservable = Observable.Interval(TimeSpan.FromMilliseconds(200)).Take(5);

            var timeout = source.Timeout(timeoutSpan, continuationObservable).Timestamp().Dump("Timeout continue");

            Console.ReadLine();
            var timoutTrigger = source.Throttle(timeoutSpan).FirstAsync();
            var puf = source.TakeUntil(timoutTrigger).Concat(continuationObservable).Timestamp().Dump("Stiched");
        }
    }
}
