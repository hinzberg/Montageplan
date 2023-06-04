using System.Windows;
using System.Windows.Controls;

namespace PolePosition.CustomToolbar
{
    public partial class ToolBar : UserControl
    {
        public ToolBar()
        {
            InitializeComponent();
            this.NewButtonText = "Neu";
            this.DeleteButtonText = "Löschen";
            this.EditButtonText = "Bearbeiten";

            this.newButton.Click += NewButtonClick;
            this.deleteButton.Click += DeleteButtonClick;
            this.editButton.Click += EditButtonClick;
        }

        public string NewButtonText
        {
            get { return this.txtNewButton.Text; }
            set { this.txtNewButton.Text = value; }
        }

        public string DeleteButtonText
        {
            get { return this.txtDeleteButton.Text; }
            set { this.txtDeleteButton.Text = value; }
        }

        public string EditButtonText
        {
            get { return this.txtEditButton.Text; }
            set { this.txtEditButton.Text = value; }
        }

        public void SetEnabledNewButton(bool isEnabled)
        {
            this.newButton.IsEnabled = isEnabled;
            if (isEnabled)
            {
                this.symbolNewButton.Opacity = 1;
            }
            else
            {
                this.symbolNewButton.Opacity = 0.5;
            }
        }

        public void SetEnabledDeleteButton(bool isEnabled)
        {
            this.deleteButton.IsEnabled = isEnabled;
            if (isEnabled)
            {
                this.symbolDeleteButton.Opacity = 1;
            }
            else
            {
                this.symbolDeleteButton.Opacity = 0.5;
            }
        }

        public void SetEnabledEditButton(bool isEnabled)
        {
            this.editButton.IsEnabled = isEnabled;
            if (isEnabled)
            {
                this.symbolEditButton.Opacity = 1;
            }
            else
            {
                this.symbolEditButton.Opacity = 0.5;
            }
        }

        public event RoutedEventHandler NewButtonClicked;
        public event RoutedEventHandler EditButtonClicked;
        public event RoutedEventHandler DeleteButtonClicked;

        private void NewButtonClick(object sender, RoutedEventArgs e)
        {
            if (NewButtonClicked != null)
                NewButtonClicked(sender, e);
        }

        void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (EditButtonClicked != null)
                EditButtonClicked(sender, e);
        }

        void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (DeleteButtonClicked != null)
                DeleteButtonClicked(sender, e);
        }
    }
}