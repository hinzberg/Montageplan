using System.Windows;
using System.Windows.Controls;

namespace Montageplan.EasyOptions.Panels
{
    public partial class GeneralOptionPanel : UserControl, IOptionPanel
    {
        public GeneralOptionPanel()
        {
            InitializeComponent();
        }

        #region IOptionPanel Members

        public string PanelName
        {
            get { return "Allgemein"; }
        }

        public string PanelDescription
        {
            get { return ""; }
        }

        public UIElement View
        {
            get { return this; }
        }

        #endregion
    }
}