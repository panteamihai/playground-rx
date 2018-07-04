using Microsoft.Reactive.Testing;
using RxWorkshop.Helpers;
using System;
using System.Globalization;
using System.Reactive.Linq;

namespace RxWorkshop.Extensions
{
    public static class ObservableExtensions
    {
        public static IDisposable Dump<T>(this IObservable<T> source, string name)
        {
            return source.Dump(name, i => i.ToString());
        }

        public static IDisposable Dump<T, TProject>(this IObservable<T> source, string name, Func<T, TProject> projection)
        {
            return source.Subscribe(
                i => Console.WriteLine($"{name} --> {projection(i)}"),
                ex => Console.WriteLine($"{name} failed! --> {ex.Message}"),
                () => Console.WriteLine($"{name} completed @{DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture)}"));
        }

        public static IObservable<T> DecorateWithTime<T>(this IObservable<T> source)
        {
            return source.Do(i => { Get.Now(); });
        }

        public static void RunOn<T>(this IObservable<T> source, TestScheduler scheduler, long creationTick = 0, long subscriptionTick = 0, long disposalTick = 0)
        {
            var testObserver = scheduler.Start(
                () => source,
                creationTick,
                subscriptionTick,
                disposalTick);

            Console.WriteLine("Time is {0} ticks", scheduler.Clock);
            Console.WriteLine("Received {0} notifications", testObserver.Messages.Count);
            foreach (var message in testObserver.Messages)
            {
                Console.WriteLine("{0} @ {1}", message.Value, message.Time);
            }
        }
    }
}
