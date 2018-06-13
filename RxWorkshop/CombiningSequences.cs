using RxWorkshop.Extensions;
using RxWorkshop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;

using Disposable = System.Reactive.Disposables.Disposable;

namespace RxWorkshop
{
    public static class CombiningSequences
    {
        public static class Sequential
        {
            public static void Concat_RequiresSequentialCompletionOfStichedObservables()
            {
                var first = Observable.Range(54, 2);
                var second = Observable.Range(42, 5);
                first.Concat(second).Dump("Concated");

                Console.ReadLine();

                var never = Observable.Never<int>();
                second.Concat(never).Dump("Never after");


                Console.ReadLine();
                never.Concat(first).Dump("Never before");
            }

            public static void Concat_WorksWellWithLazilyEvaluatedSequences()
            {
                IEnumerable<IObservable<long>> GetSequences()
                {
                    Console.WriteLine("GetSequences() is starting to yield");
                    yield return Observable.Timer(TimeSpan.FromMilliseconds(1600))
                        .Select(i => 1L)
                        .DecorateWithTime();
                    Console.WriteLine("Aia");
                    yield return Observable.Timer(TimeSpan.FromMilliseconds(2800))
                        .Select(i => 2L)
                        .DecorateWithTime();

                    Thread.Sleep(1000);
                    yield return Observable.Timer(TimeSpan.FromMilliseconds(1400))
                        .Select(i => 3L)
                        .DecorateWithTime();

                    Console.WriteLine("GetSequences() finished yielding");
                }

                GetSequences().Concat().Dump("Concat a (sync) sequence of (async) sequences");
            }

            public static void Repeat_RequiresNormalCompletion()
            {
                var source = Observable.Create<int>(
                    o =>
                    {
                        o.OnNext(135);
                        o.OnNext(654);
                        o.OnCompleted();
                        //o.OnError(new ObjectDisposedException("this"));

                        return Disposable.Empty;
                    });

                source.Repeat(2).Dump("Repeat");
            }

            public static void StartWith_PreprendsValuesToASequence_JustLikeBehaviorSubject()
            {
                var source = Observable.Create<string>(
                    o =>
                    {
                        o.OnNext("Me,");
                        o.OnNext("Myself");
                        o.OnNext("and Irene");
                        o.OnCompleted();

                        return Disposable.Empty;
                    });

                source.StartWith("Jim Carrey", "is fantastic in").Dump("Muvi");
            }
        }

        public static class Concurrent
        {
            public static void Amb_IsTheQuintessentialFirstWins()
            {
                IObservable<string> MakePoliteRequestForGreatness(string url)
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";

                    return Observable.Defer(() => request.GetResponseAsync().ToObservable().Select(wr => ((HttpWebResponse)wr).ResponseUri.Host));
                }

                var bingIt = MakePoliteRequestForGreatness("https://www.bing.com/search?q=freddie+mercury");
                var googleIt = MakePoliteRequestForGreatness("https://www.google.com/search?q=freddie+mercury");
                var duckIt = MakePoliteRequestForGreatness("https://duckduckgo.com/?q=freddie+mercury");

                new[] { bingIt, googleIt, duckIt }.Amb().Dump("There can be only one");
            }

            public static void Merge_InterleavesObservableResults()
            {
                var tens = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(3)
                                   .Select(i => i + 10);
                var hundreds = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(5)
                                   .Select(i => i + 100);

                tens.Merge(hundreds).Dump("Mearged");

                //Race condition
            }

            public static void Switch_AlwaysEmitsFromTheMostRecentSequence()
            {
                Observable.Interval(TimeSpan.FromMilliseconds(2000))
                    .Select(
                        count =>  Observable.Interval(TimeSpan.FromMilliseconds(500)).
                            Select(i => $"Inner {count}: {i}")).Switch().Dump("Outer");
            }
        }

        public static class Pairing
        {
            public static void CombineLatest_EmitsWheneverOneOfThePairedSequencesEmits()
            {
                var lower = Observable.Interval(TimeSpan.FromMilliseconds(700))
                                     .Take(6)
                                     .Select(i => (char)(i + 65));
                var upper = Observable.Interval(TimeSpan.FromMilliseconds(1300))
                                      .Take(4)
                                      .Select(i => (char)(i + 97));
                Get.Now();
                lower.CombineLatest(upper, (l, u) => l + " " + u).DecorateWithTime().Dump("CoLa");
            }

            public static void Zip_OnlyEmitsWheneverBothOfThePairedSequencesEmit()
            {
                var lower = Observable.Interval(TimeSpan.FromMilliseconds(700))
                    .Take(6)
                    .Select(i => (char)(i + 65));
                var upper = Observable.Interval(TimeSpan.FromMilliseconds(1300))
                    .Take(4)
                    .Select(i => (char)(i + 97));

                lower.Zip(upper, (l, u) => l + " " + u).DecorateWithTime().Dump("Zip");
            }

            public static void Zip_CanAlsoPairASingleEmitingObservableWithElementsFromAnEnumerable_JustLikeItWasAQueue()
            {
                var lower = Observable.Interval(TimeSpan.FromMilliseconds(700))
                    .Take(6)
                    .Select(i => (char)(i + 65));

                lower.Zip(new [] { "a", "b", "c", "d" }, (l, u) => l + " " + u).DecorateWithTime().Dump("Zip");

                //Only two sequences can be zipped with Zip though
            }

            public static void AndThenWhen_MultiZippingMadeEasy()
            {
                var one = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
                var two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(10);
                var three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(14);

                var pattern = one.And(two).And(three);
                var plan = pattern.Then((first, second, third) => new { One = first, Two = second, Three = third });
                var zippedSequence = Observable.When(plan);

                zippedSequence.Dump("MultiZip");

                //Pattern?? Plan??
            }
        }
    }
}
