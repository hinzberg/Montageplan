using System;
using System.Windows;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{
    public partial class TimePickerWindow : Window
    {
        public TimePickerWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
            this.btnCancel.Click += BtnCancelClick;
            this.btnSubmit.Click += BtnSubmitClick;
            this.allDayCheckBox.Checked += allDayCheckBox_CheckedOrUnchecked;
            this.allDayCheckBox.Unchecked += allDayCheckBox_CheckedOrUnchecked;
        }

        public void SetTimes(DateTime? start, DateTime? end)
        {
            bool hasDate = (start.HasValue && start.HasValue);
            if (hasDate)
                hasDate = (start.Value.Hour > 0 || start.Value.Minute > 0 || end.Value.Hour > 0 || end.Value.Minute > 0);

            if (hasDate)
                this.timePickerCtrl.SetTimes(start, end);
            this.allDayCheckBox.IsChecked = !hasDate;
        }

        public DateTime? GetStartTime()
        {
            DateTime? time = null;
            if (!this.allDayCheckBox.IsChecked.Value)
                time = this.timePickerCtrl.GetStartTime();
            return time;
        }

        public DateTime? GetEndTime()
        {
            DateTime? time = null;
            if (!this.allDayCheckBox.IsChecked.Value)
                time = this.timePickerCtrl.GetEndTime();
            return time;
        }

        // EventHandler

        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void allDayCheckBox_CheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            this.timePickerCtrl.UpdateTimeSpan();
            this.timePickerCtrl.IsStartEnabled = !this.allDayCheckBox.IsChecked.Value;
            this.timePickerCtrl.IsEndEnabled = !this.allDayCheckBox.IsChecked.Value;
        }
    }
}