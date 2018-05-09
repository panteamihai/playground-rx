using System;

namespace RxWorkshop.Extensions
{
    public static class ObservableExtensions
    {
        public static void Dump<T>(this IObservable<T> source, string name)
        {
            source.Subscribe(
                i => Console.WriteLine($"{name} --> {i}"),
                ex => Console.WriteLine($"{name} failed! --> {ex.Message}"),
                () => Console.WriteLine($"{name} completed @{DateTime.Now.ToUniversalTime().ToString()}"));
        }
    }
}
