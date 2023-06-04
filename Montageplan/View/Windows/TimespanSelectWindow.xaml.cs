using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class TimespanSelectWindow : Window
    {
        public TimespanSelectWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}