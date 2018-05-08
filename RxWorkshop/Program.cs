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
            //LifetimeManagement.BooleanDisposable_HasAFlagSetToTrue_AfterItHasBeenDisposed();
            //LifetimeManagement.CancelDisposable_HasACancellationToken_ThatWillBeCancelled_WhenTheDisposableIsDisposed();
            //WithUI -> LifetimeManagement.ContextDisposable_WillExecuteItsDisposableImplementation_OnTheSpecifiedSynchronizationContext();
            //LifetimeManagement.ScheduledDisposable_WillExecuteItsDisposableImplementation_OnTheSpecifiedScheduler();
            //LifetimeManagement.CompsiteDisposable_GroupsASetOfDisposables_IntoASingleContainer_ThatCanBeUsedToDisposeAllInOneSwoop();
            //LifetimeManagement.SingleAssignmentDisposable_WillOnlyAllowSettingTheUnderlyingDisposableOnce_FurtherModificationsWillMakeItThrow();
            //LifetimeManagement.MultipleAssignmentDisposable_WillAllowRotatingTheUnderlyingDisposable_WithoutDisposingOfTheReplacedDisposable();
            //LifetimeManagement.SerialDisposable_WillAllowRotatingTheUnderlyingDisposable_WhileDisposingOfTheReplacedDisposable();
            //LifetimeManagement.RefCountDisposable_SuppliesAsManyDependentDisposablesAsYouNeed_ButOnlyGetsDisposedAfterAllDependentsAreDisposedOf();
            //LifetimeManagement.UsingDisposableCreate_ToWrapActionsInADisposableObject();

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}
