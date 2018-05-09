using RxWorkshop.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace RxWorkshop
{
    public static class Sequences
    {
        private static void GetNow()
        {
            Console.WriteLine($"Now: {DateTime.Now.ToUniversalTime():HH:mm:ss fff}");
        }

        private static void GetCurrentThread()
        {
            Console.WriteLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        }

        public static class Create
        {
            public static void ButFirst_SomeHandyToolsForReasoning_Timestamp()
            {
                Console.WriteLine($"Now: {DateTime.Now/*.ToUniversalTime()*/.ToLongTimeString()}");
                //GetNow();
                Observable.Interval(TimeSpan.FromMilliseconds(450)).Take(6).Timestamp().Dump("Timestamp");

                //Timestamped class
            }

            public static void AndThen_TheresAnotherHandyToolsForReasoning_TimeInterval()
            {
                GetNow();
                Observable.Interval(TimeSpan.FromMilliseconds(750)).Take(3)
                    .Concat(Observable.Interval(TimeSpan.FromMilliseconds(230)).Take(5))
                    .TimeInterval().Dump("TimeInterval");
            }

            public static void ObservableReturn_IsASynchronousColdObservable_ThatReturnsASingleValue()
            {
                GetNow();
                var source = Observable.Return("A value delivered as an IObservable").Timestamp();
                source.Dump("Observable.Return");

                //subscribe again
                Console.ReadLine();
                source.Dump("2");
            }

            public static void ObservableEmpty_IsASynchronousColdObservable_ThatCompletesImmediately()
            {
                GetNow();
                Observable.Empty("A witness value for inferring the type").Timestamp().Dump("Observable.Empty");
            }

            public static void ObservableNever_IsAnObservable_ThatNeverCompletes()
            {
                GetNow();
                Observable.Never("A witness value for inferring the type").Timestamp().Dump("Observable.Never");
            }

            public static void ObservableThrow_IsAnObservable_ThatCompletesWithAnExceptionImmediately()
            {
                GetNow();
                Observable.Throw<string>(new AccessViolationException()).Timestamp().Dump("Observable.Throw");
            }

            public static void ObservableCreate_WillReturnALazilyEvaluatedSequence()
            {
                //A significant benefit that the CreateLazySequence method has over subjects is that the sequence will be lazily evaluated.
                //Lazy evaluation opens the doors to other powerful features such as scheduling and combination of sequences that we will see later.
                IObservable<string> CreateLazySequence(string name, int timeout)
                {
                    return Observable.Create(
                        (IObserver<string> observer) =>
                        {
                            observer.OnNext($"Frist on {name}" + Thread.CurrentThread.ManagedThreadId);
                            Thread.Sleep(timeout);

                            observer.OnNext($"Sucond on {name}" + Thread.CurrentThread.ManagedThreadId);
                            Thread.Sleep(timeout);

                            observer.OnCompleted();
                            return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
                            //or can return an Action like
                            //return () => Console.WriteLine("Observer has unsubscribed");
                        });
                }

                //Blocked before they even receive the IObservable<string>, regardless of if they do actually subscribe to it or not.
                IObservable<string> CreateBlockingSequence(string name, int timeout)
                {
                    var subject = new ReplaySubject<string>();

                    subject.OnNext($"Furst on {name}" + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(timeout);

                    subject.OnNext($"Sicomd on {name}" + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(timeout);

                    subject.OnCompleted();

                    return subject;
                }

                GetCurrentThread();
                GetNow();
                CreateLazySequence("Dre", 2000).Timestamp().Dump("Lazy Sequence");

                Console.ReadLine();
                GetNow();
                CreateBlockingSequence("Still Dre", 2000).Timestamp().Dump("Blocking Sequence");
            }

            public static void ObservableCreate_IsAVeryPowerfulThing()
            {
                IObservable<T> Empty<T>()
                {
                    return Observable.Create<T>(
                        o =>
                        {
                            o.OnCompleted();
                            return Disposable.Empty;
                        });
                }

                IObservable<T> Return<T>(T value)
                {
                    return Observable.Create<T>(
                        o =>
                        {
                            o.OnNext(value);
                            o.OnCompleted();
                            return Disposable.Empty;
                        });
                }

                IObservable<T> Never<T>()
                {
                    return Observable.Create<T>(o => Disposable.Empty);
                }

                IObservable<T> Throws<T>(Exception exception)
                {
                    return Observable.Create<T>(
                        o =>
                        {
                            o.OnError(exception);
                            return Disposable.Empty;
                        });
                }

                Empty<string>().Dump("Observable.Create->Empty");
                Return(42).Dump("Observable.Create->Return");
                Never<string>().Dump("Observable.Create->Never");
                Throws<string>(new PingException("Pong")).Dump("Observable.Create->Throws");
            }

            public static void ObservableCreate_IsSeriouslyPowerfullDude()
            {
                var count = 1;

                void OnTimerElapsed(object sender, ElapsedEventArgs e)
                {
                    Console.WriteLine($"Signal time: {e.SignalTime} (Count: {count})");
                }

                var observable = Observable.Create<string>(
                    observer =>
                    {
                        var timer = new System.Timers.Timer { Enabled = true, Interval = 250 };
                        timer.Elapsed += OnTimerElapsed;
                        timer.Start();
                        return () =>
                        {
                            timer.Elapsed -= OnTimerElapsed;
                            timer.Dispose();
                        };
                    });

                var subscription = observable.Subscribe(
                    Console.WriteLine,
                    ex => Console.WriteLine($"Powerful observable faulted: {ex.Message}"),
                    () => Console.WriteLine("Powerful observable completed"));

                Console.ReadLine();
                subscription.Dispose();

                //count++;
                //var secondSubscription = observable.Subscribe(
                //    Console.WriteLine,
                //    ex => Console.WriteLine($"Powerful observable faulted: {ex.Message}"),
                //    () => Console.WriteLine("Powerful observable completed"));

                //Console.ReadLine();
                //secondSubscription.Dispose();
            }
        }

        public static class Unfold
        {
            public static void ObservableRange_IsTheMostBasicObservableUnfold_ButNoticeItIsAlsoSynchrnous()
            {
                Observable.Range(113, 17).Dump("Range");
            }

            public static void ObservableGenerate_IsTheOtherReallyPowerfulThing_IsCatersToUnfolds()
            {
                var initialState = 1;
                Func<int, bool> condition = x => x < 6;
                Func<int, int> iterate = x => x + 1;
                Func<int, int> resultSelector = x => x;
                Func<int, TimeSpan> timeSelector = x => TimeSpan.FromSeconds(1);

                GetNow();
                Observable.Generate(initialState, condition, iterate, resultSelector, timeSelector)
                          .Timestamp()
                          .Dump("Observable.Generate->Interval(.Take(5))");
            }

            public static void ObservableInterval_IsPrettyBasic()
            {
                GetNow();
                Observable.Interval(TimeSpan.FromMilliseconds(250)).Timestamp().Dump("Interval");
            }

            public static void ObservableTimer_WillReturnOneValue_ThenComplete()
            {
                GetNow();
                Observable.Timer(TimeSpan.FromMilliseconds(2500)).Timestamp().Dump("Timer");
            }

            public static void ObservableTimer_OrItCanBeUsedToDefferTheFirstValue_ThenGenerateUsingAConstantInterval()
            {
                GetNow();
                Observable.Timer(DateTime.Now + TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1)).Timestamp()
                          .Dump("Timer (recursive)");
            }
        }

        public static class Transitioning
        {
            public static void ObservableStart_WillTakeALongRunningDelegate_AndRunItAsynchronously()
            {
                IObservable<Unit> AsynchronousAction()
                {
                    return Observable.Start(() =>
                    {
                        Console.WriteLine("Working away");
                        for (var i = 0; i < 25; i++)
                        {
                            Thread.Sleep(100);
                            Console.Write(".");
                        }

                        Console.WriteLine("|");
                    });
                }

                IObservable<string> AsynchronousFunc()
                {
                    return Observable.Start(() =>
                    {
                        Console.WriteLine("Working away");
                        for (var i = 0; i < 10; i++)
                        {
                            Thread.Sleep(100);
                            Console.Write(".");
                        }

                        Console.WriteLine("|");
                        return "Published value";
                    });
                }

                AsynchronousAction().Dump("Async action");

                Console.ReadLine();
                AsynchronousFunc().Dump("Async func");

                //Start lazily evaluates the value from a function, Return provided the value eagerly!!
            }

            public static void ObservableFromEventPattern_IsTheEventObserverPatternImplementationOnSteroids(Form form)
            {
                Observable.FromEventPattern<MouseEventArgs>(form, nameof(form.MouseClick))
                          .Subscribe(
                                mm =>
                                {
                                    var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                                    textBox?.Invoke(
                                        new Action(
                                            () => { textBox.Text += $"Clicked @ {mm.EventArgs.X} - {mm.EventArgs.Y}{Environment.NewLine}"; }
                                        ));
                                },
                                ex =>
                                {
                                    var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                                    textBox?.Invoke(new Action(() => textBox.Text += ex.Message));
                                });
            }

            public static void TaskToObservable_IsASwitchBetweenDomains_ThatOnlyWorksForHotTasks_AndDoesSoWithNoSubscriptionNecessary()
            {
                GetNow();
                var coldTask = new Task<int>(() =>
                {
                    Console.WriteLine("Starting cold task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    Thread.Sleep(5270);
                    Console.WriteLine("Finished cold task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    return 122;
                });

                Console.ReadLine();
                Console.WriteLine("You'll never see shit from cold tasks :(...");
                coldTask.ToObservable();

                Console.ReadLine();
                Console.WriteLine("...even if you subscribe to it!");
                coldTask.ToObservable().Dump("Nope");

                var observableOverHotTask = Task.Run(() =>
                {
                    Console.WriteLine("Starting hot task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    Thread.Sleep(5270);
                    Console.WriteLine("Finished hot task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    return 221;
                }).ToObservable();

                Console.ReadLine();
                Console.WriteLine("ToObservable wraps a hot task's result in a consumable observable");
                observableOverHotTask.Dump("HotTaskObservable");
            }

            public static void ObservableFromAsync_IsASwitchBetweenDomains_ButItIsDeferredUntilSubscription()
            {
                GetNow();
                var observableFromColdTask = Observable.FromAsync(() => new Task<int>(() =>
                {
                    Console.WriteLine("Starting cold task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    Thread.Sleep(3270);
                    Console.WriteLine("Finished cold task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    return 665;
                }));

                Console.WriteLine("Nothing happens by default for cold tasks");
                Console.ReadLine();

                Console.WriteLine("Nothing happens on purpose for cold tasks either!!!");
                observableFromColdTask.Dump("FromColdTask");
                Console.ReadLine();

                var observableFromHotTask = Observable.FromAsync(() => Task.Run(() =>
                {
                    Console.WriteLine("Starting hot task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    Thread.Sleep(3270);
                    Console.WriteLine("Finished hot task @ " + DateTime.Now.ToUniversalTime().ToString("HH:mm:ss fff"));
                    return 665;
                }));

                Console.WriteLine("Nothing happens by default for hot tasks either...");
                Console.ReadLine();
                Console.WriteLine("...until a subscription is made.");
                observableFromHotTask.Dump("FromHotTask");
            }

            public static void ObservableFromEnumerable_IsADangerousThing_UnlessYouKnowWhatYouAreDoing()
            {
                new[] { 1, 4, 245, 51, 2 }.ToObservable().TimeInterval().Dump("From Enumerable");
            }

            public static void ObservableFromAsyncPattern_AllowsYouToTransitionLegacyAPM_ToTAP()
            {
                //We about to BING it!
                var request = (HttpWebRequest)WebRequest.Create("http://bing.com");
                request.Method = "GET";

                Observable.FromAsyncPattern(request.BeginGetResponse, request.EndGetResponse)().Subscribe(wr => Console.WriteLine((int)((HttpWebResponse)wr).StatusCode));
                //Task.Factory.FromAsync(request.BeginGetResponse, asyncResult => request.EndGetResponse(asyncResult), null).ToObservable().Subscribe(wr => Console.WriteLine((int)((HttpWebResponse)wr).StatusCode));
            }
        }
    }
}
