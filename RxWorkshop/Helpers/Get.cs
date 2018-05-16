using System;
using System.Threading;

namespace RxWorkshop.Helpers
{
    public static class Get
    {
        public static void Now()
        {
            Console.WriteLine($"Now: {DateTime.Now.ToUniversalTime():HH:mm:ss fff}");
        }

        public static void CurrentThread()
        {
            Console.WriteLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
