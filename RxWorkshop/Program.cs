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
            //LifetimeManagement.CompositeDisposable_GroupsASetOfDisposables_IntoASingleContainer_ThatCanBeUsedToDisposeAllInOneSwoop();
            //LifetimeManagement.SingleAssignmentDisposable_WillOnlyAllowSettingTheUnderlyingDisposableOnce_FurtherModificationsWillMakeItThrow();
            //LifetimeManagement.MultipleAssignmentDisposable_WillAllowRotatingTheUnderlyingDisposable_WithoutDisposingOfTheReplacedDisposable();
            //LifetimeManagement.SerialDisposable_WillAllowRotatingTheUnderlyingDisposable_WhileDisposingOfTheReplacedDisposable();
            //LifetimeManagement.RefCountDisposable_SuppliesAsManyDependentDisposablesAsYouNeed_ButOnlyGetsDisposedAfterAllDependentsAreDisposedOf();
            //LifetimeManagement.UsingDisposableCreate_ToWrapActionsInADisposableObject();

            //Sequences.Create.ButFirst_SomeHandyToolsForReasoning_Timestamp();
            //Sequences.Create.AndThen_TheresAnotherHandyToolsForReasoning_TimeInterval();
            //Sequences.Create.ObservableReturn_IsASynchronousColdObservable_ThatReturnsASingleValue();
            //Sequences.Create.ObservableEmpty_IsASynchronousColdObservable_ThatCompletesImmediately();
            //Sequences.Create.ObservableNever_IsAnObservable_ThatNeverCompletes();
            //Sequences.Create.ObservableThrow_IsAnObservable_ThatCompletesWithAnExceptionImmediately();
            //Sequences.Create.ObservableCreate_WillReturnALazilyEvaluatedSequence();
            //Sequences.Create.ObservableCreate_IsAVeryPowerfulThing();
            //Sequences.Create.ObservableCreate_IsSeriouslyPowerfullDude();
            //Sequences.Unfold.ObservableRange_IsTheMostBasicObservableUnfold_ButNoticeItIsAlsoSynchrnous();
            //Sequences.Unfold.ObservableGenerate_IsTheOtherReallyPowerfulThing_IsCatersToUnfolds();
            //Sequences.Unfold.ObservableInterval_IsPrettyBasic();
            //Sequences.Unfold.ObservableTimer_WillReturnOneValue_ThenComplete();
            //Sequences.Unfold.ObservableTimer_OrItCanBeUsedToDefferTheFirstValue_ThenGenerateUsingAConstantInterval();
            //Sequences.Transitioning.ObservableStart_WillTakeALongRunningDelegate_AndRunItAsynchronously();
            //WithUI -> Sequences.Transitioning.ObservableFromEventPattern_IsTheEventObserverPatternImplementationOnSteroids();
            //Sequences.Transitioning.TaskToObservable_IsASwitchBetweenDomains_ThatOnlyWorksForHotTasks_AndDoesSoWithNoSubscriptionNecessary();
            //Sequences.Transitioning.ObservableFromEnumerable_IsADangerousThing_UnlessYouKnowWhatYouAreDoing();
            //Sequences.Transitioning.ObservableFromAsync_IsASwitchBetweenDomains_ButItIsDeferredUntilSubscription();
            //Sequences.Transitioning.ObservableFromAsyncPattern_AllowsYouToTransitionLegacyAPM_ToTAP();

            Console.ReadLine();
        }
    }
}
