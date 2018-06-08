using System;
using System.Linq;
using System.Windows.Forms;

namespace RxWorkshop.Extensions
{
    public static class FormExtensions
    {
        public static void AppendToBox(this Form form, string message)
        {
            var textBox = form.Controls.OfType<TextBox>().First();
            textBox.AppendText(message + Environment.NewLine);
        }
    }
}
