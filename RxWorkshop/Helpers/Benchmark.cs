using System;
using RxDisposable = System.Reactive.Disposables.Disposable;

namespace RxWorkshop.Helpers
{
    public class Benchmarker
    {
        public static void Benchmark(Action action)
        {
            Get.Now();
            using (RxDisposable.Create(Get.Now))
            {
                action();
            }
        }
    }
}
