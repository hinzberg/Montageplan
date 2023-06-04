using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Montageplan.Model;
using Montageplan.Classes;

namespace Montageplan.View.Windows
{

    public partial class DatePickerWindow : Window
    {
        public DatePickerWindow()
        {
            InitializeComponent();
            WindowIconSetter.TrySetIcon(this);
            this.btnToday.Click += BtnTodayClick;
            this.btnCancel.Click += BtnCancelClick;
            this.btnSubmit.Click += BtnSubmitClick;
        }

        void BtnTodayClick(object sender, RoutedEventArgs e)
        {
            this.pickerCtrl.GoToday(DateTime.Now);
        }

        void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public DateTime SelectedDate
        {
            get { return this.pickerCtrl.SelectedDate; }
            set { this.pickerCtrl.SelectedDate = value; }
        }
    }
}