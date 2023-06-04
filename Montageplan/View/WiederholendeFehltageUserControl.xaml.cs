using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Montageplan.View
{
    public partial class WiederholendeFehltageUserControl : UserControl
    {
        private readonly Label lblDienstag;
        private readonly Label lblDonnerstag;
        private readonly Label lblFreitag;
        private readonly Label lblMittwoch;
        private readonly Label lblMontag;
        private readonly Label lblSamstag;
        private readonly Label lblSontag;

        public WiederholendeFehltageUserControl()
        {
            InitializeComponent();
            this.lblMontag = this.CreateDescriptionLabel("Mo");
            this.lblDienstag = this.CreateDescriptionLabel("Di");
            this.lblMittwoch = this.CreateDescriptionLabel("Mi");
            this.lblDonnerstag = this.CreateDescriptionLabel("Do");
            this.lblFreitag = this.CreateDescriptionLabel("Fr");
            this.lblSamstag = this.CreateDescriptionLabel("Sa");
            this.lblSontag = this.CreateDescriptionLabel("So");

            this.lblMontag.MouseDown += lblMouseDown;
            this.lblDienstag.MouseDown += lblMouseDown;
            this.lblMittwoch.MouseDown += lblMouseDown;
            this.lblDonnerstag.MouseDown += lblMouseDown;
            this.lblFreitag.MouseDown += lblMouseDown;
            this.lblSamstag.MouseDown += lblMouseDown;
            this.lblSontag.MouseDown += lblMouseDown;
        }

        private void lblMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null && Equals(lbl.Background, Brushes.CornflowerBlue))
            {
                lbl.Background = Brushes.Transparent;
            }
            else
            {
                if (lbl != null) lbl.Background = Brushes.CornflowerBlue;
            }
        }

        private Label CreateDescriptionLabel(string description)
        {
            Label lbl = new Label();
            lbl.Content = description;
            lbl.Foreground = Brushes.Gray;
            lbl.Padding = new Thickness(0);
            lbl.Margin = new Thickness(2);
            lbl.BorderBrush = Brushes.Gray;
            lbl.BorderThickness = new Thickness(1);
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
            lbl.Height = 30;
            lbl.Width = 30;
            this.daysStack.Children.Add(lbl);
            return lbl;
        }

        public void SetDays(int value)
        {
            this.lblMontag.Background = Brushes.Transparent;
            this.lblDienstag.Background = Brushes.Transparent;
            this.lblMittwoch.Background = Brushes.Transparent;
            this.lblDonnerstag.Background = Brushes.Transparent;
            this.lblFreitag.Background = Brushes.Transparent;
            this.lblSamstag.Background = Brushes.Transparent;
            this.lblSontag.Background = Brushes.Transparent;

            if ((value & 64) == 64)
                this.lblSontag.Background = Brushes.CornflowerBlue;
            if ((value & 32) == 32)
                this.lblSamstag.Background = Brushes.CornflowerBlue;
            if ((value & 16) == 16)
                this.lblFreitag.Background = Brushes.CornflowerBlue;
            if ((value & 8) == 8)
                this.lblDonnerstag.Background = Brushes.CornflowerBlue;
            if ((value & 4) == 4)
                this.lblMittwoch.Background = Brushes.CornflowerBlue;
            if ((value & 2) == 2)
                this.lblDienstag.Background = Brushes.CornflowerBlue;
            if ((value & 1) == 1)
                this.lblMontag.Background = Brushes.CornflowerBlue;
        }

        public int GetDays()
        {
            int value = 0;

            if (Equals(this.lblSontag.Background, Brushes.CornflowerBlue))
                value += 64;
            if (Equals(this.lblSamstag.Background, Brushes.CornflowerBlue))
                value += 32;
            if (Equals(this.lblFreitag.Background, Brushes.CornflowerBlue))
                value += 16;
            if (Equals(this.lblDonnerstag.Background, Brushes.CornflowerBlue))
                value += 8;
            if (Equals(this.lblMittwoch.Background, Brushes.CornflowerBlue))
                value += 4;
            if (Equals(this.lblDienstag.Background, Brushes.CornflowerBlue))
                value += 2;
            if (Equals(this.lblMontag.Background, Brushes.CornflowerBlue))
                value += 1;

            return value;
        }
    }
}