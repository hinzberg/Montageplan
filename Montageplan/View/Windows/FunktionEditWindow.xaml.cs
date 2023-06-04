using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class FunktionEditWindow : Window
    {
        public FunktionEditWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}