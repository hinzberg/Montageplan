using System.Windows;
using Montageplan.Classes;

namespace Montageplan.Windows
{
    public partial class MitarbeiterEditWindow : Window
    {
        public MitarbeiterEditWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}