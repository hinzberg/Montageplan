using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}