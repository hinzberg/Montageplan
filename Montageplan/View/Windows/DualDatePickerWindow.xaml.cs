using System;
using System.Windows;
using Montageplan.Classes;
using Montageplan.Model;

namespace Montageplan.View.Windows
{
    public partial class DualDatePickerWindow : Window
    {
        public DualDatePickerWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
            this.btnToday1.Click += BtnToday1Click;
            this.btnToday2.Click += BtnToday2Click;
            this.btnCancel.Click += BtnCancelClick;
            this.btnSubmit.Click += BtnSubmitClick;
        }

        public DateTime SelectedStartDate
        {
            get { return this.pickerCtrl1.SelectedDate; }
            set { this.pickerCtrl1.SelectedDate = value; }
        }

        public DateTime SelectedEndDate
        {
            get { return this.pickerCtrl2.SelectedDate; }
            set { this.pickerCtrl2.SelectedDate = value; }
        }

        private void BtnToday1Click(object sender, RoutedEventArgs e)
        {
            this.pickerCtrl1.GoToday(DateTime.Now);
        }

        private void BtnToday2Click(object sender, RoutedEventArgs e)
        {
            this.pickerCtrl2.GoToday(DateTime.Now);
        }

        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            if (this.ValidateDates())
                this.DialogResult = true;
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private bool ValidateDates()
        {
            if (this.pickerCtrl2.SelectedDate.Date < this.pickerCtrl1.SelectedDate.Date)
            {
                const string message = "Das Startdatum muss vor dem Endedatum liegen.";
                MessageBoxHelper.ShowOkBoxWindow("",message, this);
                return false;
            }
            return true;
        }
    }
}