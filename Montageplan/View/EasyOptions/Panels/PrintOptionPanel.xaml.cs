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
using Montageplan.EasyOptions;

namespace Montageplan.View.EasyOptions.Panels
{
    public partial class PrintOptionPanel : UserControl , IOptionPanel 
    {
        public PrintOptionPanel()
        {
            InitializeComponent();
        }

        public string PanelName
        {
            get { return "Druck"; }
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
