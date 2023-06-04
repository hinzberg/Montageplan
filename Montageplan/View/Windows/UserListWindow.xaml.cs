using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class UserListWindow : Window
    {
        public UserListWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
        }
    }
}