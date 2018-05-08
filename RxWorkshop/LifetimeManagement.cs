using RxWorkshop.Implementations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace RxWorkshop
{
    public class LifetimeManagement
    {
        public static void TheSubscribeExtensionMethods_WillReturnAnIDisposableImplementation_ThatGetsAutomaticallyDisposed_AfterOnCompletedOrOnErrorAreCalled()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(500)).Take(3);
            var longerLastingObservableThanSource = source.Concat(Observable.Return(1L));

            var subscription = source.Subscribe(_ => { });
            var subscription2 = longerLastingObservableThanSource.Subscribe(
                _ => { },
                () => Console.WriteLine("Should be true now that OnCompleted has been called: " + ((StableCompositeDisposable)subscription).IsDisposed));

            Console.WriteLine("Should be false here because it is not completed yet: " + ((StableCompositeDisposable)subscription).IsDisposed);
            Console.Read();
        }

        public static void NotDisposingOfSubjectSubscription_CausesMemoryLeaks_BecauseOfTheyWaySubscribe_IsImplementedOnTheSubjectClass()
        {
            var subject = new Subject<int>();
            var subscription = subject.Subscribe(new ConsoleObserver<int>());

            var longerLastingObservableThanSubject = subject.Concat(Observable.Return(-1));
            var subscription2 = longerLastingObservableThanSubject
                .Subscribe(_ => { },
                    () => Console.WriteLine("Put a breakpoint here and check the IsDisposed flag of the subject through the captured:" + subscription));

            subject.OnNext(1);
            subject.OnNext(15);
            subject.OnNext(21);
            subject.OnNext(4);
            subject.OnNext(63);

            subject.OnCompleted();

            //There is really no need for the subject's subscription to be automatically
            //disposed since it is basically bound to the lifetime of the subject

            //subject.Dispose();
        }

        public static void BooleanDisposable_HasAFlagSetToTrue_AfterItHasBeenDisposed()
        {
            var booleanDisposable = new BooleanDisposable();
            if (!booleanDisposable.IsDisposed)
            {
                Console.WriteLine("Disposing");
                booleanDisposable.Dispose();
            }

            if (booleanDisposable.IsDisposed)
            {
                Console.WriteLine("Succesfully Disposed");
            }
        }

        public static void CancelDisposable_HasACancellationToken_ThatWillBeCancelled_WhenTheDisposableIsDisposed()
        {
            var cancellationDisposable = new CancellationDisposable();

            cancellationDisposable.Token.Register(() => Console.WriteLine("Disposed from cancellation token"));

            Console.WriteLine("Disposing");
            cancellationDisposable.Dispose();
        }

        public static void ContextDisposable_WillExecuteItsDisposableImplementation_OnTheSpecifiedSynchronizationContext(Action<string> logging = null)
        {
            //Needs to be run from a UI application

            logging = logging ?? (message => Debug.WriteLine(message));

            var contextDisposable = new ContextDisposable(
                SynchronizationContext.Current, // Will throw if run from the console, but will capture the UI sync context if run inside an event handler on a Form
                Disposable.Create(() => logging("Disposing Thread ID: " + Thread.CurrentThread.ManagedThreadId)));

            Scheduler.Default.Schedule(() =>
            {
                logging("Calling Thread ID: " + Thread.CurrentThread.ManagedThreadId);
                contextDisposable.Dispose();
            });
        }

        public static void ScheduledDisposable_WillExecuteItsDisposableImplementation_OnTheSpecifiedScheduler()
        {
            Console.WriteLine("We are starting on Thread ID: " + Thread.CurrentThread.ManagedThreadId);

            var contextDisposable = new ScheduledDisposable(
                NewThreadScheduler.Default,
                Disposable.Create(() => Console.WriteLine("Disposing Thread ID: " + Thread.CurrentThread.ManagedThreadId)));

            Scheduler.Default.Schedule(() =>
            {
                Console.WriteLine("Calling Thread ID: " + Thread.CurrentThread.ManagedThreadId);
                contextDisposable.Dispose();
            });

            Console.Read();
        }

        public static void CompsiteDisposable_GroupsASetOfDisposables_IntoASingleContainer_ThatCanBeUsedToDisposeAllInOneSwoop()
        {
            //Look at the Thread ID where the observation takes place.
            //Now try a variation of 500 - 350 - 110 ms
            var composite = new CompositeDisposable
                            {
                                Observable.Interval(TimeSpan.FromMilliseconds(500))
                                    .Subscribe(_ => Console.WriteLine("Ticking from 500ms on " + Thread.CurrentThread.ManagedThreadId)),
                                Observable.Interval(TimeSpan.FromMilliseconds(250))
                                    .Subscribe(_ => Console.WriteLine("Ticking from 250ms on " + Thread.CurrentThread.ManagedThreadId)),
                                Observable.Interval(TimeSpan.FromMilliseconds(125))
                                    .Subscribe(_ => Console.WriteLine("Ticking from 125ms on " + Thread.CurrentThread.ManagedThreadId))
                            };

            NewThreadScheduler.Default.Schedule(TimeSpan.FromSeconds(2), () =>
            {
                Console.WriteLine("Disposing from (new) Thread ID " + Thread.CurrentThread.ManagedThreadId);
                composite.Dispose();
            });
        }

        public static void SingleAssignmentDisposable_WillOnlyAllowSettingTheUnderlyingDisposableOnce_FurtherModificationsWillMakeItThrow()
        {
            var singleAssignmentDisposable = new SingleAssignmentDisposable();
            var notDisposedDisposable = Disposable.Create(() => Console.WriteLine("I am gonna be disposed because I was first."));
            var replacementDisposable = Disposable.Create(() => Console.WriteLine("I can't replace the first assigned disposable. Estupido!"));

            singleAssignmentDisposable.Disposable = notDisposedDisposable;

            try
            {
                singleAssignmentDisposable.Disposable = replacementDisposable;
            }
            catch
            {
                Console.WriteLine("Told you you can't replace it once it has been set.");
            }

            Console.WriteLine("Disposing of the single assignement disposable.");
            singleAssignmentDisposable.Dispose();

            Console.WriteLine($"Single assignement disposable is disposed: {singleAssignmentDisposable.IsDisposed}.");
        }

        public static void MultipleAssignmentDisposable_WillAllowRotatingTheUnderlyingDisposable_WithoutDisposingOfTheReplacedDisposable()
        {
            var multipleAssignmentDisposable = new MultipleAssignmentDisposable();
            var notDisposedDisposable = Disposable.Create(() => Console.WriteLine("Wouldn't you believe it, I ain't gonna be disposed. Estupido!"));
            var replacementDisposable = Disposable.Create(() => Console.WriteLine("I'm gonna be disposed by disposing of the multi assignement disposable."));

            multipleAssignmentDisposable.Disposable = notDisposedDisposable;
            multipleAssignmentDisposable.Disposable = replacementDisposable;

            Console.WriteLine("The switch has been made, but the replaced disposable has not been disposed.");
            Console.WriteLine("Disposing of the multi assignement disposable.");
            multipleAssignmentDisposable.Dispose();

            Console.WriteLine($"Multi assignement disposable is disposed: {multipleAssignmentDisposable.IsDisposed}.");
        }

        public static void SerialDisposable_WillAllowRotatingTheUnderlyingDisposable_WhileDisposingOfTheReplacedDisposable()
        {
            var serialDisposable = new SerialDisposable();
            var notDisposedDisposable = Disposable.Create(() => Console.WriteLine("I will be disposed upon replacement."));
            var notAnotherDisposedDisposable = Disposable.Create(() => Console.WriteLine("I will also be disposed upon replacement."));
            var replacementDisposable = Disposable.Create(() => Console.WriteLine("I'm gonna be disposed by disposing of the serial disposable."));

            serialDisposable.Disposable = notDisposedDisposable;
            serialDisposable.Disposable = notAnotherDisposedDisposable;
            serialDisposable.Disposable = replacementDisposable;

            Console.WriteLine("Disposing of the serial disposable.");
            serialDisposable.Dispose();

            Console.WriteLine($"Serial disposable is disposed: {serialDisposable.IsDisposed}.");
        }

        public static void RefCountDisposable_SuppliesAsManyDependentDisposablesAsYouNeed_ButOnlyGetsDisposedAfterAllDependentsAreDisposedOf()
        {
            //Can be used as part of a synchonization mechanism

            var refCountDisposable = new RefCountDisposable(
                Disposable.Create(() =>
                    Debug.WriteLine("Underlying disposable has been disposed.")));

            var firstDependentDisposable = refCountDisposable.GetDisposable();
            var secondDependentDisposable = refCountDisposable.GetDisposable();
            var thirdDependentDisposable = refCountDisposable.GetDisposable();

            Console.WriteLine("Disposing of the second dependent.");
            secondDependentDisposable.Dispose();

            Console.WriteLine("Trying to dispose of the RefCountDisposable");
            refCountDisposable.Dispose();

            Console.WriteLine($"Evidently it fails! RefCountDisposable is disposed: {refCountDisposable.IsDisposed}");

            Console.WriteLine("Disposing of the third dependent.");
            thirdDependentDisposable.Dispose();

            Console.WriteLine("Disposing of the first dependent.");
            firstDependentDisposable.Dispose();

            Console.WriteLine($"Now that the last dependent is disposed, RefCountDisposable is disposed: {refCountDisposable.IsDisposed}");
        }

        public static void UsingDisposableCreate_ToWrapActionsInADisposableObject()
        {
            var disposable = Disposable.Create(() => Console.WriteLine("Do something like `control.EndUpdate()` upon exiting the `using`"));
            using (disposable)
            {
                Console.WriteLine("Do something like `control.BeginUpdate()`");
            }
        }
    }
}
