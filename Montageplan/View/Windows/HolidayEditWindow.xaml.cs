using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    /// <summary>
    /// Interaktionslogik für HolidayEditWindow.xaml
    /// </summary>
    public partial class HolidayEditWindow : Window
    {
        public HolidayEditWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}