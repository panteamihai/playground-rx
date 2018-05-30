# Reactive Programming with Rx.NET

We're walking the path set forth by Lee Campbell's [Intro to Rx].

## Stopovers

* [Key types]
    * `IObservable<T>` and `IObserver<T>` (and maybe even `ISubject<T>`)
    * `Subject<T>`, `ReplaySubject<T>`, `BehaviorSubject<T>`, `AsyncSubject<T>`
* [Lifetime management]
    * `IDisposable` and its variants
    * The `IDisposable` returned by the `Subscribe` extension methods will dispose itself upon `OnCompleted` or `OnError`. Other implementations won't. Still, you should always capture and dispose of subscriptions yourself when possible.
* [Creating sequences]
    * Introducing `Timestamp` and `TimeInterval` as handy tools for reasoning about streams
    * Creating sequences and the power of the lazily evaluated `Observable.Create`
    * Functional unfolds and the other powerful generation method: `Observable.Generate`
    * Transitioning into observables by switching between domains (from `Task` or `Action`/`Func`)
* [Reducing sequences]
    * Filtering with `Where`
    * Determining `Distinct` elements, and introducing pairwise distinction with `DistinctUntilChanged`
    * `Skip` and `Take`
* [Inspecting sequences]
    * `Any` vs. `All` vs. `Contains`
    * Gracefully handling the empty sequence scenario
    * `ElementAt` and why not to use it
    * Comparing two sequences for equality with `SequenceEqual`
* [Aggregating sequences]
    * Sequence math: `Min`, `Max`, `Sum`, `Average`
    * Functional folds: `First/Last/Single[OrDefault][Async]`
    * Custom aggregations and the relationship between `Aggregate` and `Scan`
    * Partitioning sequences
* [Transforming sequences]
    * Mapping with `Select` and its contactual obligations
    * Casting and materializing
    * `SelectMany`, the powerful `bind function`
    * Visualizing sequences

[Intro to Rx]: <http://introtorx.com/>
[Key types]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/KeyTypes.cs>
[Lifetime management]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/LifetimeManagement.cs>
[Creating sequences]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/CreatingSequences.cs>
[Reducing sequences]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/ReducingSequences.cs>
[Inspecting sequences]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/InspectingSequences.cs>
[Aggregating sequences]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/AggregatingSequences.cs>
[Transforming sequences]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/TransformingSequences.cs>
