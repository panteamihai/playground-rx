using RxWorkshop.Extensions;
using RxWorkshop.Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Disposable = System.Reactive.Disposables.Disposable;

namespace RxWorkshop
{
    public static class TransformingSequences
    {
        public static void Select_TransitionsEachElementToAnotherShape()
        {
            Observable.Range(1, 5)
                      .Select(i => new { Number = i, Character = (char)(i + 64) })
                      .Dump("anon");

            //Contractual obligations
        }

        public static void Select_CanAlsoLeverageIndexes_IndexFizzBuzz()
        {
            Observable.Range(44, 63)
                .Select((v, i) =>
                {
                    var result = "";
                    if (i % 3 == 0)
                        result = "Fizz";
                    if (i % 5 == 0)
                        result += "Buzz";

                    result = string.IsNullOrEmpty(result) ? v.ToString() : result;

                    return (value: v, index: i, result);
                }).Dump("FizzBuzz on sequence indexes");
        }

        public static void Casting_ErrorsAndCompletesWhenCastFails()
        {
            var objects = Observable.Create<object>(
                observer =>
                {
                    observer.OnNext(1);
                    observer.OnNext(2);
                    observer.OnNext("3"); //Ignored
                    observer.OnNext(4);
                    observer.OnCompleted();

                    return Disposable.Empty;
                });

            objects.Cast<int>().Dump("Cast"); // is equivalent to objects.Select(i => (int)i);
        }

        public static void SafeCasting_WithOfType_AlwaysCompletes()
        {
            var objects = Observable.Create<object>(
                observer =>
                {
                    observer.OnNext(1);
                    observer.OnNext(2);
                    observer.OnNext("3"); //Ignored
                    observer.OnNext(4);
                    observer.OnCompleted();

                    return Disposable.Empty;
                });

            objects.OfType<decimal>().Dump("OfType"); //is equivalent to source.Where(i=>i is decimal).Select(i=>(decimal)i);
        }

        public static void Materialize_GivesYouAMetaViewOfASequence()
        {
            var source = new Subject<int>();
            source.Materialize()
                  //.Where(n => n.Kind != NotificationKind.OnCompleted)
                  .Dump("Materialize");
            source.OnNext(1);
            source.OnNext(2);
            source.OnNext(3);
            source.OnCompleted(); //meta, then passed on
            //source.OnError(new Exception("Fail?"));
        }

        public static void Dematerialize_IsTheInverseAction()
        {
            var source = new Subject<int>();
            source.Materialize()
                  .Dematerialize()
                  .Dump("Dematerialize");
            source.OnNext(1);
            source.OnNext(2);
            source.OnNext(3);
            source.OnCompleted();
        }

        public static void SelectMany_IsTheBindOperation_ShouldBeThoughtOfAs_FromOneSelectZeroOneOrMany()
        {
            Observable.Return(3)
                .SelectMany(i =>
                {
                    //Returns an IObservable<T> based on a T

                    return Observable.Empty<int>();
                    //return Observable.Return(i + 1);
                    //return Observable.Range(1, i);
                })
                .Dump("SelectMany");
        }

        public static void SelectMany_FlattensTheOutputedSequences()
        {
            Observable.Range(1, 3)
                .SelectMany(i => Observable.Range(1, i))
                .Dump("Flat");
        }

        public static void SelectMany_IsVeryPowerfulAsWell()
        {
            //Does Select, Where, etc.

            char ToLetter(int i) => (char)(i + 64);

            Observable.Range(1, 30)
                .SelectMany(
                    i =>
                    {
                        if (0 < i && i < 27)
                        {
                            return Observable.Return(ToLetter(i));
                        }

                        return Observable.Empty<char>();
                    })
                .Dump("Alphabet");
        }

        public static void VisualizingSequences_Helps()
        {
            Get.Now();
            IObservable<long> GetSubValues(long offset)
            {
                //Produce values [x*10, (x*10)+1, (x*10)+2] 4 seconds apart, but starting immediately.
                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(2000))
                    .Select(x => offset * 10 + x)
                    .Take(3);
            }

            Observable.Interval(TimeSpan.FromMilliseconds(3000))
                .Select(i => i + 1)
                .Take(3)
                .SelectMany(GetSubValues)
                .TimeInterval()
                .Dump("Powerful");
        }
    }
}
