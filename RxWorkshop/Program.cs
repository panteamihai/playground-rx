using System;

namespace RxWorkshop
{
    public class Program
    {
        static void Main()
        {
            //KeyTypes.ObservableObserverPair();
            //KeyTypes.UsingSynchronousObservables();
            //KeyTypes.NaiveSubjectImplementation();
            //KeyTypes.WorkingWithAnActualSubject();
            //KeyTypes.TimeOfSubscriptionActuallyMattersForSubjects();
            //KeyTypes.HowAboutACache();
            //KeyTypes.CacheItEvenAfterCompletion();
            //KeyTypes.TheDefaultValueForASubscriptionBeforeEmittingAValue();
            //KeyTypes.TheLastValueBeforeSubscription();
            //KeyTypes.TheLastValueBeforeCompletion();
            //KeyTypes.UnlessThereIsNone_ButAtLeastItFinishes();
            //KeyTypes.NotCompletingAnAsyncSubject_WillNotPushAnyValueOut();

            //LifetimeManagement.TheSubscribeExtensionMethods_WillReturnAnIDisposableImplementation_ThatGetsAutomaticallyDisposed_AfterOnCompletedOrOnErrorAreCalled();
            //LifetimeManagement.NotDisposingOfSubjectSubscription_CausesMemoryLeaks_BecauseOfTheyWaySubscribe_IsImplementedOnTheSubjectClass();
            //LifetimeManagement.UsingDisposableCreate_ToWrapActionsInADisposableObject();

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
