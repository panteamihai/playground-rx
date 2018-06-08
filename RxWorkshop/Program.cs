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
            //CreatingSequences.Transitioning.ObservableFromAsync_IsASwitchBetweenDomains_ButItIsDeferredUntilSubscription();
            //CreatingSequences.Transitioning.ObservableFromEnumerable_IsADangerousThing_UnlessYouKnowWhatYouAreDoing();
            //CreatingSequences.Transitioning.ObservableFromAsyncPattern_AllowsYouToTransitionLegacyAPM_ToTAP();

            //ReducingSequences.Where_FilteringStuffIsEasyPeasy();
            //ReducingSequences.Distinct_HoldsNoSurprises();
            //ReducingSequences.Distinct_ComesInAKeySelectorFlavor();
            //ReducingSequences.DistinctUntilChanged_PerformsPairwiseDistinct_NoTwoInARow();
            //ReducingSequences.IgnoreElements_UseItWhenYouOnlyCareAboutCompletion();
            //ReducingSequences.Skip_SkipsAFew();
            //ReducingSequences.Take_TakesAFew();
            //ReducingSequences.SkipWhile_GuardsTheFrontOfTheStream();
            //ReducingSequences.TakeWhile_GuardsTheBackOfTheStream();
            //ReducingSequences.SkipLast_WillBufferTheNumberOfElementsToSkip_ThenEjectOneAtATimeAfterTheBufferIsOverfilled_UntilCompletion();
            //ReducingSequences.TakeLast_WillBufferTheNumberOfElementsToSkip_ThenDiscardOneAtATimeAfterTheBufferIsOverfilled_UntilOnCompletedIsReceived();
            //ReducingSequences.SkipUntil_UsesSecondObservable_AsASignalToStartConsuming();
            //ReducingSequences.TakeUntil_UsesSecondObservable_AsASignalToStopConsuming();

            //InspectingSequences.Any_IsUsedToCheckForElements();
            //InspectingSequences.All_IsUsedToMatchAllElements_ToAPredicate();
            //InspectingSequences.Contains_IsBasicallyAnAnyImplementation_ThatTargetsOneSpecificValue_AsOpposedToACategoryOfValuesThatFitAPredicate();
            //InspectingSequences.DefaultIfEmpty_HandlesTheMissingValuesCasesGracefully();
            //InspectingSequences.ElementAt_CherryPicksBasedOnPosition();
            //InspectingSequences.SequenceEqual_WillTakeOrderIntoAccountAsWell();

            //AggregatingSequences.Count_IsBoundByTheObservableCompleting();
            //AggregatingSequences.SequenceMath_AlsoNeedsRegularCompletion();
            //AggregatingSequences.FunctionalFolds.First_WillBlockUntilTheValueIsProduced_OrItWillThrowIfEmptySequence();
            //AggregatingSequences.FunctionalFolds.LastOrDefault_WillStillBlockUntilTheValueOrCompleteIsProduced_OrItWillThrowAnyEncounteredException();
            //AggregatingSequences.FunctionalFolds.SingleAsync_WillNotBlock();
            //WithUI -> AggregatingSequences.CustomAggregations.Aggregate_IsPowerful_ButBoundByCompletion();
            //WithUI -> AggregatingSequences.CustomAggregations.Scan_IsJustAggregate_WithAllIntermediateSteps();
            //AggregatingSequences.Partitioning.MinBy_WillGetTheGroupOfItemsWhoseKeysMeetTheMinimum_OnlyAfterCompletion();
            //AggregatingSequences.Partitioning.GroupBy_IntroducesIGroupedObservable();
            //AggregatingSequences.Partitioning.GroupBy_IfYouNeedToManipulateGroupes_ThenCombineTheResultsOfAllGroupManipulations_UseSelectMany();
            //AggregatingSequences.Partitioning.GroupBy_NestedObservables();

            //TransformingSequences.Select_TransitionsEachElementToAnotherShape();
            //TransformingSequences.Select_CanAlsoLeverageIndexes_IndexFizzBuzz();
            //TransformingSequences.Casting_ErrorsAndCompletesWhenCastFails();
            //TransformingSequences.SafeCasting_WithOfType_AlwaysCompletes();
            //TransformingSequences.Materialize_GivesYouAMetaViewOfASequence();
            //TransformingSequences.Dematerialize_IsTheInverseAction();
            //TransformingSequences.SelectMany_IsTheBindOperation_ShouldBeThoughtOfAs_FromOneSelectZeroOneOrMany();
            //TransformingSequences.SelectMany_FlattensTheOutputedSequences();
            //TransformingSequences.SelectMany_IsVeryPowerfulAsWell();
            //TransformingSequences.VisualizingSequences_Helps();

            //SideEffects.IntroducingAndManipulatingState_IsVeryBadForComprehension();
            //SideEffects.IfIndexIsWhatInterestsYou_GoForTheFunctionalApproach_WithTheSelectOverload();
            //SideEffects.YouCanEncapsulateState_InsideThePipeline_UsingScan();
            //SideEffects.EnhanceSequenceManipulation_UsingDo();
            //SideEffects.YouCanDoNastyStuff_ToReferenceValues_WithDo();

            //AdvancedErrorHandling.Catch_ForSwallowingAllExceptions();
            //AdvancedErrorHandling.Catch_CanBeUsedForSpecificExceptionsToo();
            //AdvancedErrorHandling.Finally_IsInvokedIfWeTerminateNormally_Erronously_OrTheSubscriptionIsDisposedOf();
            //AdvancedErrorHandling.Using_AllowsYouToBindTheLifetimeOfAResource_ToThatOfAnObservable();
            //AdvancedErrorHandling.OnErrorResumeNext_ProvidesAFallBack();
            //AdvancedErrorHandling.Retry_IsTricky();

            //CombiningSequences.Sequential.Concat_RequiresSequentialCompletionOfStichedObservables();
            //CombiningSequences.Sequential.Concat_WorksWellWithLazilyEvaluatedSequences();
            //CombiningSequences.Sequential.Repeat_RequiresNormalCompletion();
            //CombiningSequences.Sequential.StartWith_PreprendsValuesToASequence_JustLikeBehaviorSubject();
            //CombiningSequences.Concurrent.Amb_IsTheQuintessentialFirstWins();
            //CombiningSequences.Concurrent.Merge_InterleavesObservableResults();
            //CombiningSequences.Concurrent.Switch_AlwaysEmitsFromTheMostRecentSequence();
            //CombiningSequences.Pairing.CombineLatest_EmitsWheneverOneOfThePairedSequencesEmits();
            //CombiningSequences.Pairing.Zip_OnlyEmitsWheneverBothOfThePairedSequencesEmit();
            //CombiningSequences.Pairing.Zip_CanAlsoPairASingleEmitingObservableWithElementsFromAnEnumerable_JustLikeItWasAQueue();
            //CombiningSequences.Pairing.AndThenWhen_MultiZippingMadeEasy();

            Console.ReadLine();
        }
    }
}