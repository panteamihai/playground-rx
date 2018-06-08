using RxWorkshop;
using System;
using System.Threading;
using System.Windows.Forms;

namespace WithUI
{
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
        }

        private void BtnContextDisposableClick(object sender, EventArgs e)
        {
            txtOutput.Text = $"The UI Thread ID is: {Thread.CurrentThread.ManagedThreadId}{Environment.NewLine}";

            var logging = new Action<string>(message => txtOutput.Invoke(new Action(() => txtOutput.Text += message + Environment.NewLine)));
            LifetimeManagement.ContextDisposable_WillExecuteItsDisposableImplementation_OnTheSpecifiedSynchronizationContext(logging);
        }

        private void BtnFromEventPatternClick(object sender, EventArgs e)
        {
            CreatingSequences.Transitioning.ObservableFromEventPattern_IsTheEventObserverPatternImplementationOnSteroids(this);
        }

        private void BtnAggregateClick(object sender, EventArgs e)
        {
            AggregatingSequences.CustomAggregations.Aggregate_IsPowerful_ButBoundByCompletion(this);
        }

        private void BtnScanClick(object sender, EventArgs e)
        {
            AggregatingSequences.CustomAggregations.Scan_IsJustAggregate_WithAllIntermediateSteps(this);
        }

        private void BtnThrottleClick(object sender, EventArgs e)
        {
            TimeshiftedSequences.Throttle_GuardsAgainstPeaks_IsGreatForUserInteraction(this);
        }
    }
}
