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
using System.Windows.Threading;

namespace Montageplan.View
{
    /// <summary>
    /// Interaktionslogik für ZoomControl.xaml
    /// </summary>
    public partial class ZoomControl : UserControl
    {
        private const double DEFAULT_VALUE = 4; // Dieser Wert ist 100 % Zoom

        private readonly DispatcherTimer timer;
        private TimeSpan showingTime;
        private UIElement element;
        private bool isCtrlPressed;

        public ZoomControl()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.element = null;
            this.isCtrlPressed = false;
            this.SetShowingTime(TimeSpan.FromSeconds(2));
            this.zoomSlider.Minimum = 1;
            this.zoomSlider.Maximum = 6;
            this.zoomSlider.Value = DEFAULT_VALUE;
            this.zoomSlider.TickFrequency = 1;

            this.timer.Tick += timer_Tick;
            this.SliderValueChanged = null;
        }

        public Action<double> SliderValueChanged { get; set; }

        public void ObserveMouseWheel(UIElement element)
        {
            if (this.element != null)
            {
                this.element.KeyDown -= element_KeyDown;
                this.element.KeyUp -= element_KeyUp;
                this.element.MouseWheel -= element_MouseWheel;
            }
            this.element = element;
            this.element.PreviewKeyDown += element_KeyDown;
            this.element.PreviewKeyUp += element_KeyUp;
            this.element.PreviewMouseWheel += element_MouseWheel;
        }

        public void SetSliderMax(double max)
        {
            this.zoomSlider.Maximum = max;
        }

        public void SetSliderValue(double value)
        {
            this.zoomSlider.Value = value;
        }

        private void ShowSlider(TimeSpan showingTime)
        {
            this.SetShowingTime(showingTime);
            this.ShowSlider();
        }

        private void ShowSlider()
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.timer.Stop();
            this.timer.Start();
        }

        private void SetShowingTime(TimeSpan showingTime)
        {
            this.showingTime = showingTime;
            this.timer.Interval = this.showingTime;
        }

        private void RaiseSliderValue()
        {
            double newValue = this.zoomSlider.Value += 1;
            if (newValue <= this.zoomSlider.Maximum)
            {
                this.zoomSlider.Value = newValue;
                this.UpdatePercentValueText();
            }
        }

        private void ReduceSliderValue()
        {
            double newValue = this.zoomSlider.Value -= 1;
            if (newValue >= 0)
            {
                this.zoomSlider.Value = newValue;
                this.UpdatePercentValueText();
            }
        }

        private void UpdatePercentValueText()
        {
            double percent = (this.zoomSlider.Value / DEFAULT_VALUE) * 100;
            this.percentTextBlock.Text = (int)percent + " %";
        }

        // EventHandler

        void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.Dispatcher.Invoke((Action)delegate
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        void element_KeyDown(object sender, KeyEventArgs e)
        {
            this.isCtrlPressed = (e.Key == Key.LeftCtrl);
        }

        void element_KeyUp(object sender, KeyEventArgs e)
        {
            this.isCtrlPressed = false;
        }

        void element_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this.isCtrlPressed && e.Delta != 0)
            {
                this.ShowSlider();
                if (e.Delta > 0)
                {
                    this.RaiseSliderValue();
                }
                else if (e.Delta < 0)
                {
                    this.ReduceSliderValue();
                }
                if (this.SliderValueChanged != null)
                    this.SliderValueChanged(this.zoomSlider.Value);
            }
        }

    }
}
