using System.Windows;
using Montageplan.Classes;

namespace Montageplan.Windows
{
    public partial class KolonneEditWindow : Window
    {
        public KolonneEditWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}