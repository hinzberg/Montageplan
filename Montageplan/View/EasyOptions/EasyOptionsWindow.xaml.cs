using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Montageplan.EasyOptions.Panels;
using Montageplan.Model;
using Montageplan.View.EasyOptions.Panels;
using PolePosition.EasyOptions;
using Montageplan.Classes;

namespace Montageplan.EasyOptions
{
    public partial class EasyOptionsWindow : Window
    {
        private List<OptionNavigationItemVM> navigationItems;
        private List<IOptionPanel> optionPanels;
        private GeneralOptionPanel generalOptionPanel;
        private AboutOptionPanel aboutOptionPanel;
        private DisplayOptionPanel displayOptionPanel;
        private PrintOptionPanel printOptionPanel;

        // Index des zuletzt verwendeten Panels. Wird wieder selektiert
        // wenn das Fenster erneut geöffnet wird.
        public static int LastSelectedTabIndex { get; set; }

        static EasyOptionsWindow()
        {
            LastSelectedTabIndex = 0;
        }

        public EasyOptionsWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
            this.CreatePanels();
        }

        private void CreatePanels()
        {
            this.navigationItems = new List<OptionNavigationItemVM>();
            this.optionPanels = new List<IOptionPanel>();

            this.generalOptionPanel = new GeneralOptionPanel();
            //navigationItems.Add(new OptionNavigationItemVM(generalOptionPanel));
            //optionPanels.Add(generalOptionPanel);

            this.displayOptionPanel = new DisplayOptionPanel(this);
            navigationItems.Add(new OptionNavigationItemVM(displayOptionPanel));
            optionPanels.Add(displayOptionPanel);

            this.printOptionPanel = new PrintOptionPanel();
            navigationItems.Add(new OptionNavigationItemVM(printOptionPanel));
            optionPanels.Add(printOptionPanel);

            this.aboutOptionPanel = new AboutOptionPanel();
            navigationItems.Add(new OptionNavigationItemVM(aboutOptionPanel));
            optionPanels.Add(aboutOptionPanel);

            this.NavigationListBox.ItemsSource = navigationItems;
            this.NavigationListBox.SelectedIndex = LastSelectedTabIndex;
        }

        private void NavigationListBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            OptionNavigationItemVM itemVM = this.NavigationListBox.SelectedItem as OptionNavigationItemVM;
            if (itemVM != null)
                this.ShowContentPanel(itemVM.PanelName);
        }

        private void ShowContentPanel(string name)
        {
            foreach (IOptionPanel optionPanel in optionPanels)
            {
                if (optionPanel.PanelName == name)
                {
                    this.ContentPanel.Children.Clear();
                    this.ContentPanel.Children.Add(optionPanel.View);
                }
            }
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            this.UpdateModelForDisplayPanel();
            this.UpdateModelForPrintPanel();
            this.DialogResult = true;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdateViewForDisplayPanel();
            this.UpdateViewForPrintPanel();
        }

        private void UpdateViewForDisplayPanel()
        {
            switch (AppConfig.Settings.KalenderTageAusrichtung)
            {
                case DaysAlignment.FixedWidth:
                    this.displayOptionPanel.festBreiteRadioButton.IsChecked = true;
                    break;
                case DaysAlignment.DynamicWidth:
                    this.displayOptionPanel.dynamischeBreiteRadioButton.IsChecked = true;
                    break;
            }

            this.displayOptionPanel.checkDisplayBezeichnung.IsChecked
                = AppConfig.Settings.MitarbeiterDisplayWithBezeichnung;

            this.displayOptionPanel.checkDisplayVorname.IsChecked
                = AppConfig.Settings.MitarbeiterDisplayWithVorname;

            this.displayOptionPanel.checkDisplayNachname.IsChecked
                = AppConfig.Settings.MitarbeiterDisplayWithName;

            this.displayOptionPanel.colorDisplay.Background
                = AppConfig.Settings.DayBrush;

            this.displayOptionPanel.checkShowKolonnePrefix.IsChecked
                = AppConfig.Settings.ShowKolonnePrefix;

            this.aboutOptionPanel.softwareVersionTextBlock.Text = string.Format("{0}  ({1})",
                AppConfig.Version.ProductVersion, AppConfig.AssemblyDate.ToShortDateString());
            this.aboutOptionPanel.databaseVersionTextBlock.Text = AppConfig.MetaData.DatabaseVersion.ToString();
        }

        private void UpdateModelForDisplayPanel()
        {
            if (this.displayOptionPanel.festBreiteRadioButton.IsChecked.Value == true)
            {
                AppConfig.Settings.KalenderTageAusrichtung = DaysAlignment.FixedWidth;
            }
            else if (this.displayOptionPanel.dynamischeBreiteRadioButton.IsChecked.Value == true)
            {
                AppConfig.Settings.KalenderTageAusrichtung = DaysAlignment.DynamicWidth;
            }

            AppConfig.Settings.MitarbeiterDisplayWithBezeichnung
                = this.displayOptionPanel.checkDisplayBezeichnung.IsChecked.Value;

            AppConfig.Settings.MitarbeiterDisplayWithName
                = this.displayOptionPanel.checkDisplayNachname.IsChecked.Value;

            AppConfig.Settings.MitarbeiterDisplayWithVorname
                = this.displayOptionPanel.checkDisplayVorname.IsChecked.Value;

            AppConfig.Settings.ShowKolonnePrefix
                = this.displayOptionPanel.checkShowKolonnePrefix.IsChecked.Value;

            AppConfig.Settings.DayBrush 
                = (SolidColorBrush)this.displayOptionPanel.colorDisplay.Background;
        }

        private void UpdateViewForPrintPanel()
        {
            this.printOptionPanel.txtPageOffsetX.Text = AppConfig.Settings.Print.PageOffsetX.ToString();
            this.printOptionPanel.txtPageOffsetY.Text = AppConfig.Settings.Print.PageOffsetY.ToString();
        }

        private void UpdateModelForPrintPanel()
        {
            int i = 0;
            if (int.TryParse(this.printOptionPanel.txtPageOffsetX.Text, out i))
                AppConfig.Settings.Print.PageOffsetX = i;

            if (int.TryParse(this.printOptionPanel.txtPageOffsetY.Text, out i))
                AppConfig.Settings.Print.PageOffsetY = i;

        }

    }
}