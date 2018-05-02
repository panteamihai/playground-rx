# Reactive Programming

## Rx.NET

We're walking the path set forth by Lee Campbell's [Intro to Rx].

### Stopovers

* [Key types]
    * `IObservable<T>` & `IObserver<T>`
    * `Subject<T>`, `ReplaySubject<T>`, `BehaviorSubject<T>`, `AsyncSubject<T>`
* [Lifetime management]
    * `IDisposable` and its variants
    * The `IDisposable` returned by the `Subscribe` extension methods will dispose itself upon `OnCompleted` or `OnError`. Other implementations won't. Still you should always capture and dispose of subscriptions yourself.

[Intro to Rx]: <http://introtorx.com/>
[Key types]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/KeyTypes.cs>
[Lifetime management]: <https://github.com/panteamihai/workshop-rx/blob/master/RxWorkshop/LifetimeManagement.cs>