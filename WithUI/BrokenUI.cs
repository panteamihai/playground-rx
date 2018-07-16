using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms;

namespace WithUI
{
    public partial class BrokenUI : Form
    {
        private readonly Subject<string> _subject = new Subject<string>();

        public BrokenUI()
        {
            InitializeComponent();

            button.Text = "Default value";
            button.Refresh();

            //Deadlock!
            //We need the dispatcher to continue to allow me to click the button to produce a value
            button.Text = _subject.First();

            //This will give same result but will not be blocking (deadlocking).
            _subject.Take(1).Subscribe(value => button.Text = value);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            _subject.OnNext("New Value");
        }
    }
}
