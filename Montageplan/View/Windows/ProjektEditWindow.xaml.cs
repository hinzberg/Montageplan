using System.Windows;
using Montageplan.Classes;

namespace Montageplan.Windows
{
    public partial class ProjektEditWindow : Window
    {
        public ProjektEditWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}