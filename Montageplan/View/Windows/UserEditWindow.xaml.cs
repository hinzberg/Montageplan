using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class UserEditWindow : Window
    {
        public UserEditWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}