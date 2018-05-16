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

            //CreatingSequences.Factory.ButFirst_SomeHandyToolsForReasoning_Timestamp();
            //CreatingSequences.Factory.AndThen_TheresAnotherHandyToolsForReasoning_TimeInterval();
            //CreatingSequences.Factory.ObservableReturn_IsASynchronousColdObservable_ThatReturnsASingleValue();
            //CreatingSequences.Factory.ObservableEmpty_IsASynchronousColdObservable_ThatCompletesImmediately();
            //CreatingSequences.Factory.ObservableNever_IsAnObservable_ThatNeverCompletes();
            //CreatingSequences.Factory.ObservableThrow_IsAnObservable_ThatCompletesWithAnExceptionImmediately();
            //CreatingSequences.Factory.ObservableCreate_WillReturnALazilyEvaluatedSequence();
            //CreatingSequences.Factory.ObservableCreate_IsAVeryPowerfulThing();
            //CreatingSequences.Factory.ObservableCreate_IsSeriouslyPowerfullDude();
            //CreatingSequences.Unfold.ObservableRange_IsTheMostBasicObservableUnfold_ButNoticeItIsAlsoSynchrnous();
            //CreatingSequences.Unfold.ObservableGenerate_IsTheOtherReallyPowerfulThing_IsCatersToUnfolds();
            //CreatingSequences.Unfold.ObservableInterval_IsPrettyBasic();
            //CreatingSequences.Unfold.ObservableTimer_WillReturnOneValue_ThenComplete();
            //CreatingSequences.Unfold.ObservableTimer_OrItCanBeUsedToDefferTheFirstValue_ThenGenerateUsingAConstantInterval();
            //CreatingSequences.Transitioning.ObservableStart_WillTakeALongRunningDelegate_AndRunItAsynchronously();
            //WithUI -> CreatingSequences.Transitioning.ObservableFromEventPattern_IsTheEventObserverPatternImplementationOnSteroids();
            //CreatingSequences.Transitioning.TaskToObservable_IsASwitchBetweenDomains_ThatOnlyWorksForHotTasks_AndDoesSoWithNoSubscriptionNecessary();
            //CreatingSequences.Transitioning.ObservableFromEnumerable_IsADangerousThing_UnlessYouKnowWhatYouAreDoing();
            //CreatingSequences.Transitioning.ObservableFromAsync_IsASwitchBetweenDomains_ButItIsDeferredUntilSubscription();
            //CreatingSequences.Transitioning.ObservableFromAsyncPattern_AllowsYouToTransitionLegacyAPM_ToTAP();

            Console.ReadLine();
        }
    }
}
