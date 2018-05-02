using RxWorkshop.Implementations;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
                () => Console.WriteLine("Put a breakpoint here and check the IsDisposed flag of the subject through the captured:" + subscription));

            Console.WriteLine("Wait for it...");
            Console.Read();
        }

        public static void NotDisposingOfSubjectSubscription_CausesMemoryLeaks_BecauseOfTheyWaySubscribe_IsImplementedOnTheSubjectClass()
        {
            var subject = new Subject<int>();
            var subscription = subject.Subscribe(new ConsoleObserver<int>());

            var longerLastingObservableThanSubject = subject.Concat(Observable.Return(-1));
            var subscription2 = longerLastingObservableThanSubject
                .Subscribe(_ => { Console.WriteLine("The Subscribe method implementation will not return a \"fancy\" IDisposable"); },
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

        public static void UsingDisposableCreate_ToWrapActionsInADisposableObject()
        {
            var disposable = Disposable.Create(() => Console.WriteLine("Do something like `control.EndUpdate()` upon exiting the `using`"));
            using (disposable)
            {
                Console.WriteLine("Do something like `control.BeginUpdate()`");
            }
        }

        //PS: MultipleAssignmentDisposable doesn't dispose the previous Disposable but SerialDisposable does.
    }
}
