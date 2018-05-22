using RxWorkshop.Extensions;
using System;
using System.Diagnostics.Eventing;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

using static RxWorkshop.Helpers.Benchmarker;

namespace RxWorkshop
{
    public static class AggregatingSequences
    {
        public static void Count_IsBoundByTheObservableCompleting()
        {
            var numbers = Observable.Range(0, 3);
            numbers.Dump("numbers");
            numbers.Count().Dump("count");

            Console.ReadLine();
            Observable.Never<int>().Count().Dump("never count");

            Console.ReadLine();
            Observable.Throw<int>(new InvalidDataException()).Count().Dump("errored count");
        }

        public static void SequenceMath_AlsoNeedsRegularCompletion()
        {
            var numbers = Observable.Range(-8, 20);
            numbers.Min().Dump("Min");
            numbers.Sum().Dump("Sum");
            numbers.Average().Dump("Average (mean)");

            //Mean, median or mode
        }

        public static class FunctionalFolds
        {
            public static void First_WillBlockUntilTheValueIsProduced_OrItWillThrowIfEmptySequence()
            {
                var interval = Observable.Interval(TimeSpan.FromSeconds(3));
                Benchmark(() => Console.WriteLine(interval.First()));

                Console.ReadLine();
                try
                {
                    Observable.Empty<int>().First();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            public static void LastOrDefault_WillStillBlockUntilTheValueOrCompleteIsProduced_OrItWillThrowAnyEncounteredException()
            {
                Benchmark(() => Console.WriteLine(Observable.Empty<int>().LastOrDefault()));

                Console.ReadLine();
                try
                {
                    Observable.Throw<int>(new EventLogException()).LastOrDefault();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            public static void SingleAsync_WillNotBlock()
            {
                Benchmark(() =>
                    {
                        Observable.Return("3.14").SingleAsync()
                            .Subscribe(v => { Console.WriteLine($"Value was {v}"); });
                    });

                Console.ReadLine();

                Benchmark(async () =>
                {
                    var singleValue = await Observable.Return("3.14").SingleAsync();
                    Console.WriteLine($"Value was {singleValue}");
                });

                Console.ReadLine();

                try
                {
                    Observable.Empty<int>().SingleAsync().Subscribe(_ => {}, ex => Console.WriteLine(ex));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static class CustomAggregations
        {
            public static void Aggregate_IsPowerful_ButBoundByCompletion(Form form)
            {
                var moves = Observable.FromEventPattern<MouseEventArgs>(form, nameof(form.MouseMove));
                var doubleClicks = Observable.FromEventPattern<EventArgs>(form, nameof(form.DoubleClick));

                moves.TakeUntil(doubleClicks).Aggregate(
                    (start: DateTime.Now, distance: 0, previous: new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)),
                    (accumulator, current) =>
                    {
                        accumulator.distance += Math.Abs(current.EventArgs.X - accumulator.previous.X);
                        accumulator.previous = current.EventArgs;

                        return accumulator;
                    }).Subscribe(
                    agg =>
                    {
                        var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                        textBox?.Invoke(
                            new Action(
                                () =>
                                {
                                    textBox.Text +=
                                        $"Moused a total of {agg.distance} pixels over a time period of {DateTime.Now.Subtract(agg.start)}";
                                }));
                    },
                    ex =>
                    {
                        var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                        textBox?.Invoke(new Action(() => textBox.Text += ex.Message));
                    });
            }

            public static void Scan_IsJustAggregate_WithAllIntermediateSteps(Form form)
            {
                var moves = Observable.FromEventPattern<MouseEventArgs>(form, nameof(form.MouseMove));
                var doubleClicks = Observable.FromEventPattern<EventArgs>(form, nameof(form.DoubleClick));

                var start = DateTime.Now;
                moves.TakeUntil(doubleClicks).Scan(
                    (start, distance: 0, previous: new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0)),
                    (accumulator, current) =>
                    {
                        accumulator.distance += Math.Abs(current.EventArgs.X - accumulator.previous.X);
                        accumulator.previous = current.EventArgs;

                        return accumulator;
                    }).Subscribe(
                    agg =>
                    {
                        var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                        textBox?.Invoke(
                            new Action(
                                () =>
                                {
                                    textBox.AppendText($"Moused a total of {agg.distance} pixels *so far* {Environment.NewLine}");
                                }));
                    },
                    ex =>
                    {
                        var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                        textBox?.Invoke(new Action(() => textBox.Text += ex.Message));
                    },
                    () => {
                        var textBox = form.Controls.OfType<TextBox>().FirstOrDefault();
                        textBox?.Invoke(new Action(() =>
                        {
                            textBox.AppendText($"Moused over a time period of {DateTime.Now.Subtract(start)}");
                            textBox.ScrollToCaret();
                        }));
                    });
            }
        }

        public static class Partitioning
        {
            public static void MinBy_WillGetTheGroupOfItemsWhoseKeysMeetTheMinimum_OnlyAfterCompletion()
            {
                Observable.Range(12, 44).MinBy(i => i % 3).Subscribe(l =>
                {
                    Console.WriteLine("Minimums:");
                    Console.WriteLine(string.Join(",", l));
                });
                Observable.Range(12, 44).MaxBy(i => i % 3).Subscribe(l =>
                {
                    Console.WriteLine("Maximums:");
                    Console.WriteLine(string.Join(",", l));
                });
            }

            public static void GroupBy_IntroducesIGroupedObservable()
            {
                Observable.Range(12, 44).GroupBy(i => i % 3)
                                        .Subscribe(groupedObservable => groupedObservable.Dump($"Grouped observable {groupedObservable.Key}"));
            }

            public static void GroupBy_IfYouNeedToManipulateGroupes_ThenCombineTheResultsOfAllGroupManipulations_UseSelectMany()
            {
                //Also don't subscribe to inner observables

                Observable.Range(12, 44)
                          .GroupBy(i => i % 3)
                          .SelectMany(grp => grp.Max()
                                                .Select(value => new { grp.Key, value }))
                          .Dump("All maxes");
            }

            public static void GroupBy_NestedObservables()
            {
                var watcher = new FileSystemWatcher
                              {
                                  NotifyFilter = NotifyFilters.LastAccess
                                                 | NotifyFilters.LastWrite
                                                 | NotifyFilters.FileName
                                                 | NotifyFilters.DirectoryName,
                                  Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                  Filter = "*.txt"
                              };

                // Add event handlers.
                var created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    ev => watcher.Created += ev,
                    ev => watcher.Created -= ev)
                    .Select(i => new { Type = i.EventArgs.ChangeType, Value = "Created: " + i.EventArgs.Name });
                var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    ev => watcher.Deleted += ev,
                    ev => watcher.Deleted -= ev)
                    .Select(i => new { Type = i.EventArgs.ChangeType, Value = "Deleted: " + i.EventArgs.Name });
                var changed = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    ev => watcher.Changed += ev,
                    ev => watcher.Changed -= ev)
                    .Select(i => new { Type = i.EventArgs.ChangeType, Value = "Changed: " + i.EventArgs.Name });
                //var renamed = Observable.FromEvent<RenamedEventHandler, RenamedEventArgs>(
                //    ev => watcher.Renamed += ev,
                //    ev => watcher.Renamed -= ev)
                //    .Select(i => "Renamed: " + i.Name);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                created.Merge(deleted).Merge(changed).GroupBy(i => i.Type).SelectMany(group => group.Select(i => i).Take(2)).Dump("All collapsed");
            }
        }
    }
}
