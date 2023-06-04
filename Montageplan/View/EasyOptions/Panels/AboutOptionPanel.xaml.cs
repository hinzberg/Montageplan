using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Montageplan.EasyOptions.Panels
{
    public partial class AboutOptionPanel : UserControl, IOptionPanel
    {
        public AboutOptionPanel()
        {
            InitializeComponent();
        }

        public string PanelName
        {
            get { return "Über"; }
        }

        public string PanelDescription
        {
            get { return ""; }
        }

        public UIElement View
        {
            get { return this; }
        }
    }
}
