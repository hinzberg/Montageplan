using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class FunktionListWindow : Window
    {
        public FunktionListWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}