using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Montageplan.View.Windows
{
    public partial class ColorPickerWindow : Window
    {
        private readonly byte alphaValue;

        public ColorPickerWindow(Color col)
            : this(col,60)
        {
                
        }

        public ColorPickerWindow(Color col, byte alpha)
        {
            this.SelectedColor = col;
            this.alphaValue = alpha;

            InitializeComponent();
            this.btnCancel.Click += BtnCancelClick;
            this.btnSubmit.Click += BtnSubmitClick;
            this.sliderColorR.ValueChanged += SliderValueChanged;
            this.sliderColorG.ValueChanged += SliderValueChanged;
            this.sliderColorB.ValueChanged += SliderValueChanged;

            this.sliderColorR.Minimum = 0;
            this.sliderColorR.Maximum = 255;
            this.sliderColorG.Minimum = 0;
            this.sliderColorG.Maximum = 255;
            this.sliderColorB.Minimum = 0;
            this.sliderColorB.Maximum = 255;

            this.InitSilderValues();
            this.InitColorWell();
            this.UpdateColorDisplay();
        }

        /// <summary>
        /// Die ausgewählte Farbe des Pickers.
        /// </summary>
        public Color SelectedColor { get; set; }

        /// <summary>
        /// Default-Farben anzeigen und Mouse-Events anhängen.
        /// </summary>
        private void InitColorWell()
        {
            this.defaultColor1.Background = this.CreateTransparentBrush(Colors.Green, this.alphaValue);
            this.defaultColor2.Background = this.CreateTransparentBrush(Colors.LimeGreen, this.alphaValue);
            this.defaultColor3.Background = this.CreateTransparentBrush(Colors.SlateBlue, this.alphaValue);
            this.defaultColor4.Background = this.CreateTransparentBrush(Colors.DarkSlateBlue, this.alphaValue);
            this.defaultColor5.Background = this.CreateTransparentBrush(Colors.DarkBlue, this.alphaValue);
            this.defaultColor6.Background = this.CreateTransparentBrush(Colors.Purple, this.alphaValue);
            this.defaultColor7.Background = this.CreateTransparentBrush(Colors.DeepPink, this.alphaValue);

            this.defaultColor8.Background = this.CreateTransparentBrush(Colors.Red, this.alphaValue);
            this.defaultColor9.Background = this.CreateTransparentBrush(Colors.OrangeRed, this.alphaValue);
            this.defaultColor10.Background = this.CreateTransparentBrush(Colors.Crimson, this.alphaValue);
            this.defaultColor11.Background = this.CreateTransparentBrush(Colors.DarkRed, this.alphaValue);
            this.defaultColor12.Background = this.CreateTransparentBrush(Colors.Orange, this.alphaValue);
            this.defaultColor13.Background = this.CreateTransparentBrush(Colors.DarkOrange, this.alphaValue);
            this.defaultColor14.Background = this.CreateTransparentBrush(Colors.Yellow, this.alphaValue);
            

            this.defaultColor1.MouseDown += ColorWellMouseDown;
            this.defaultColor2.MouseDown += ColorWellMouseDown;
            this.defaultColor3.MouseDown += ColorWellMouseDown;
            this.defaultColor4.MouseDown += ColorWellMouseDown;
            this.defaultColor5.MouseDown += ColorWellMouseDown;
            this.defaultColor6.MouseDown += ColorWellMouseDown;
            this.defaultColor7.MouseDown += ColorWellMouseDown;
            this.defaultColor8.MouseDown += ColorWellMouseDown;
            this.defaultColor9.MouseDown += ColorWellMouseDown;
            this.defaultColor10.MouseDown += ColorWellMouseDown;
            this.defaultColor11.MouseDown += ColorWellMouseDown;
            this.defaultColor12.MouseDown += ColorWellMouseDown;
            this.defaultColor13.MouseDown += ColorWellMouseDown;
            this.defaultColor14.MouseDown += ColorWellMouseDown;
        }

        /// <summary>
        /// Erteugt aus einer Farben einen Semi-Transparenten Brush
        /// </summary>
        /// <param name="col"></param>
        /// <param name="transparency"></param>
        /// <returns></returns>
        private SolidColorBrush CreateTransparentBrush(Color col, byte transparency)
        {
            Color c = Color.FromArgb(transparency, col.R, col.G, col.B);
            SolidColorBrush br = new SolidColorBrush(c);
            return br;
        }

        /// <summary>
        /// Klick auf eine der Defaultfarben. Einstellungen in Schieberegler übernehmen. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorWellMouseDown(object sender, MouseButtonEventArgs e)
        {
            Border selectedBorder = sender as Border;
            if (selectedBorder != null)
            {
                SolidColorBrush solidColorBrush = selectedBorder.Background as SolidColorBrush;
                if (solidColorBrush != null)
                {
                    int r = (int)solidColorBrush.Color.R;
                    this.sliderColorR.Value = r;
                    int g = (int)solidColorBrush.Color.G;
                    this.sliderColorG.Value = g;
                    int b = (int)solidColorBrush.Color.B;
                    this.sliderColorB.Value = b;
                    this.UpdateColorDisplay();
                }
            }
        }

        /// <summary>
        /// Einer der Schieberegler wurde verschoben.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.UpdateColorDisplay();
        }

        /// <summary>
        /// Nach dem öffnen des Fensters die übergebene Farbe anzeigen.
        /// </summary>
        private void InitSilderValues()
        {
            int r = (int)this.SelectedColor.R;
            this.sliderColorR.Value = r;
            int g = (int)this.SelectedColor.G;
            this.sliderColorG.Value = g;
            int b = (int)this.SelectedColor.B;
            this.sliderColorB.Value = b;
        }

        /// <summary>
        /// Erzeugt aus den Werten der Schiebregler die Vorschau
        /// und aktualisieriert die Labels.
        /// </summary>
        private void UpdateColorDisplay()
        {
            double r = this.sliderColorR.Value;
            double g = this.sliderColorG.Value;
            double b = this.sliderColorB.Value;
            //this.txtColorA.Text = a.ToString("N0");
            this.txtColorR.Text = r.ToString("N0");
            this.txtColorG.Text = g.ToString("N0");
            this.txtColorB.Text = b.ToString("N0");

            Color col = Color.FromArgb(this.alphaValue, (byte)r, (byte)g, (byte)b);
            this.colorDisplay.Background = new SolidColorBrush(col);
        }

        /// <summary>
        /// Ok gewählt. Farbe für öffentliche Eigenschaft bereitstellen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            double r = this.sliderColorR.Value;
            double g = this.sliderColorG.Value;
            double b = this.sliderColorB.Value;
            Color col = Color.FromArgb(this.alphaValue, (byte)r, (byte)g, (byte)b);
            this.SelectedColor = col;
            this.DialogResult = true;
        }

        /// <summary>
        /// Abgebrochen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}