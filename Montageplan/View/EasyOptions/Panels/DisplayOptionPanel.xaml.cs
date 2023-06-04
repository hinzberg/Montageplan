using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Montageplan.EasyOptions;
using Montageplan.View.Windows;

namespace Montageplan.View.EasyOptions.Panels
{
    public partial class DisplayOptionPanel : UserControl, IOptionPanel
    {
        private readonly Window ownerWindow;

        public DisplayOptionPanel(Window owner)
        {
            InitializeComponent();
            this.ownerWindow = owner;
            this.checkDisplayNachname.Click += CheckDisplayNachnameClick;
            this.checkDisplayVorname.Click += CheckDisplayVornameClick;
            this.colorDisplay.MouseDown += ColorDisplayMouseDown;
        }

        #region IOptionPanel Members

        public string PanelName
        {
            get { return "Darstellung"; }
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

        /// <summary>
        /// Vorname darf nicht abgewählt werden, wenn schon Nachname inaktiv ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckDisplayVornameClick(object sender, RoutedEventArgs e)
        {
            bool valueVorname = this.checkDisplayVorname.IsChecked.Value;
            bool valueName = this.checkDisplayNachname.IsChecked.Value;

            if (valueVorname == false && valueName == false)
                this.checkDisplayVorname.IsChecked = true;
        }

        /// <summary>
        /// Namen darf nicht abgwählt werden, wenn schon Vorname inaktiv ist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckDisplayNachnameClick(object sender, RoutedEventArgs e)
        {
            bool valueVorname = this.checkDisplayVorname.IsChecked.Value;
            bool valueName = this.checkDisplayNachname.IsChecked.Value;

            if (valueName == false && valueVorname == false)
                this.checkDisplayNachname.IsChecked = true;
        }

        private void ColorDisplayMouseDown(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush solidColor = (SolidColorBrush) this.colorDisplay.Background;
            if (solidColor == null)
                solidColor = new SolidColorBrush(Colors.Black);

            ColorPickerWindow colorPickerWindow = new ColorPickerWindow(solidColor.Color, 200);
            colorPickerWindow.Owner = this.ownerWindow;
            bool? res = colorPickerWindow.ShowDialog();
            if (res != null && res == true)
            {
                Color col = colorPickerWindow.SelectedColor;
                this.colorDisplay.Background = new SolidColorBrush(col);
            }
        }
    }
}