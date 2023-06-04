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
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class TimePickerControl : UserControl
    {
        public static readonly DependencyProperty IsStartEnabledProperty =
            DependencyProperty.Register("IsStartEnabled", typeof(bool), typeof(TimePickerControl), new PropertyMetadata(true));
        public static readonly DependencyProperty IsEndEnabledProperty =
            DependencyProperty.Register("IsEndEnabled", typeof(bool), typeof(TimePickerControl), new PropertyMetadata(true));

        private readonly List<TimePickerItemViewModel> startTimes;
        private readonly List<TimePickerItemViewModel> endTimes;

        private DateTime? startTime;
        private DateTime? endTime;

        public TimePickerControl()
        {
            InitializeComponent();
            this.startTimes = TimePickerItemViewModel.CreateList(0, 23);
            this.endTimes = TimePickerItemViewModel.CreateList(0, 23);
            this.cmbStartTime.ItemsSource = startTimes;
            this.cmbEndTime.ItemsSource = endTimes;
            this.startTime = new DateTime(1, 1, 1, 8, 0, 0);
            this.endTime = new DateTime(1, 1, 1, 18, 0, 0);
            this.SelectTime(this.cmbStartTime, startTime);
            this.SelectTime(this.cmbEndTime, endTime);
            this.cmbStartTime.SelectionChanged += ComboSelectionChanged;
            this.cmbEndTime.SelectionChanged += ComboSelectionChanged;
            this.IsStartEnabled = true;
            this.IsEndEnabled = true;
        }

        public bool IsStartEnabled
        {
            get { return (bool)GetValue(IsStartEnabledProperty); }
            set { SetValue(IsStartEnabledProperty, value); }
        }

        public bool IsEndEnabled
        {
            get { return (bool)GetValue(IsEndEnabledProperty); }
            set { SetValue(IsEndEnabledProperty, value); }
        }

        void ComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateTimeSpan();
        }


        public void SetTimes(DateTime? start, DateTime? end)
        {
            this.startTime = start;
            this.endTime = end;

            if (start.HasValue)
                this.SelectTime(this.cmbStartTime, new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                    start.Value.Hour, start.Value.Minute, 0));
            if (end.HasValue)
                this.SelectTime(this.cmbEndTime, new DateTime(DateTime.MinValue.Year, DateTime.MinValue.Month, DateTime.MinValue.Day,
                    end.Value.Hour, end.Value.Minute, 0));
        }

        public void UpdateTimeSpan()
        {
            DateTime? s = this.GetStartTime();
            DateTime? e = this.GetEndTime();

            string spanText = "";
            if (s.HasValue && e.HasValue)
            {
                TimeSpan ts = e.Value - s.Value;
                double hours = Math.Abs(ts.TotalHours);
                spanText = hours.ToString("N2") + " Stunden";
            }

            this.lblTimeSpan.Content = spanText;
        }


        public DateTime? GetStartTime()
        {
            TimePickerItemViewModel vm = this.cmbStartTime.SelectedItem as TimePickerItemViewModel;
            if (vm != null)
                this.startTime = vm.GetModel();
            return this.startTime;
        }

        public DateTime? GetEndTime()
        {
            TimePickerItemViewModel vm = this.cmbEndTime.SelectedItem as TimePickerItemViewModel;
            if (vm != null)
                this.endTime = vm.GetModel();
            return this.endTime;
        }

        private void SelectTime(ComboBox cmb, DateTime? dt)
        {
            ItemCollection vms = cmb.Items;

            foreach (var vm in vms)
            {
                TimePickerItemViewModel timeVm = vm as TimePickerItemViewModel;
                if (timeVm != null)
                {
                    DateTime? itemDt = timeVm.GetModel();
                    if (itemDt.HasValue && dt.HasValue)
                    {
                        if (itemDt.Value.Hour == dt.Value.Hour && itemDt.Value.Minute == dt.Value.Minute)
                        {
                            cmb.SelectedItem = vm;
                            break;
                        }
                    }
                    else if (!itemDt.HasValue && !dt.HasValue)
                    {
                        cmb.SelectedItem = vm;
                        break;
                    }

                }
            }
        }


    }
}
