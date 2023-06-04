using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class GenericWaitWindow : Window
    {
        public GenericWaitWindow(string topLine, string bottomLine)
        {
            InitializeComponent();
            this.txtTopLine.Text = topLine;
            this.txtBottomLine.Text = bottomLine;
            WindowIconSetter.TrySetIcon(this);
        }
    }
}