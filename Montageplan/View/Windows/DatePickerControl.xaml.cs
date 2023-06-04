using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Montageplan.Model;

namespace Montageplan.View.Windows
{
    /// <summary>
    /// Der Calender beginnt immer an dem Montag
    /// vor dem 1. des Monates und der Kalender hat immer 6 Wochen,
    /// </summary>
    public partial class DatePickerControl : UserControl
    {
        private DateTime workingDate;

        public DatePickerControl()
        {
            InitializeComponent();
            this.SelectedDate = DateTime.MinValue;
            this.workingDate = DateTime.Now;
            this.Loaded += DatePickerControlLoaded;
            this.btnNextMonth.Click += BtnNextMonthClick;
            this.btnPreviousMonth.Click += BtnPreviousMonthClick;
        }

        public DateTime SelectedDate { get; set; }

        public void GoToday(DateTime date)
        {
            this.workingDate = date;
            this.SelectedDate = date;
            this.CreateCalenderLayout();
        }

        private void DatePickerControlLoaded(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDate != DateTime.MinValue)
            {
                this.workingDate = this.SelectedDate;
            }

            this.CreateCalenderLayout();
        }

        private void BtnPreviousMonthClick(object sender, RoutedEventArgs e)
        {
            int days = DateTime.DaysInMonth(this.workingDate.Year, this.workingDate.Month);
            this.workingDate = this.workingDate.AddDays(-1*days);
            this.CreateCalenderLayout();
        }

        private void BtnNextMonthClick(object sender, RoutedEventArgs e)
        {
            int days = DateTime.DaysInMonth(this.workingDate.Year, this.workingDate.Month);
            this.workingDate = this.workingDate.AddDays(days);
            this.CreateCalenderLayout();
        }

        private void CreateCalenderLayout()
        {
            this.txtSelectedDay.Text = this.SelectedDate.ToLongDateString();

            this.monthStack.Children.Clear();
            this.CreateDescriptionStack();

            DateTime initDate = this.workingDate;
            DateTime firstOfMonth = new DateTime(initDate.Year, initDate.Month, 1);

            string date = string.Format("{0:MMMM yyyy}", firstOfMonth);
            this.txtMonthDescription.Text = date;

            DateHelper helper = new DateHelper();
            DateTime startDate = helper.GetMondayBeforeDate(firstOfMonth);

            for (int i = 0; i < 6; i++)
            {
                TimeSpan ts = new TimeSpan((i*7), 0, 0, 0);
                DateTime dt = startDate + ts;
                this.CreateWeekStack(dt);
            }
        }

        private void CreateDescriptionStack()
        {
            StackPanel wStackPanel = new StackPanel();
            wStackPanel.Orientation = Orientation.Horizontal;

            List<string> descr = new List<string>();
            descr.Add("Mo");
            descr.Add("Di");
            descr.Add("Mi");
            descr.Add("Do");
            descr.Add("Fr");
            descr.Add("Sa");
            descr.Add("So");

            foreach (string s in descr)
            {
                Label lbl = new Label();
                lbl.Content = s;
                lbl.Foreground = Brushes.Black;
                lbl.Padding = new Thickness(0);
                lbl.Margin = new Thickness(2);
                lbl.BorderThickness = new Thickness(1);
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                lbl.Height = 20;
                lbl.Width = 30;

                wStackPanel.Children.Add(lbl);
            }
            this.monthStack.Children.Add(wStackPanel);
        }

        private void CreateWeekStack(DateTime startDate)
        {
            StackPanel wStackPanel = new StackPanel();
            wStackPanel.Orientation = Orientation.Horizontal;

            for (int i = 0; i < 7; i++)
            {
                TimeSpan ts = new TimeSpan(i, 0, 0, 0);
                DateTime dt = startDate + ts;

                Brush borderBrush = Brushes.Black;
                Brush fillBrush = Brushes.Transparent;

                if (dt.Month != this.workingDate.Month)
                {
                    borderBrush = Brushes.DarkGray;
                }

                if (dt.Date == DateTime.Now.Date)
                    borderBrush = new SolidColorBrush(Colors.CornflowerBlue);

                if (dt.Date == this.SelectedDate.Date)
                {
                    fillBrush = Brushes.CornflowerBlue;
                    borderBrush = Brushes.Black;
                }

                Label lbl = new Label();
                lbl.Content = dt.Day.ToString();
                lbl.Foreground = borderBrush;
                lbl.BorderBrush = borderBrush;
                lbl.Background = fillBrush;
                lbl.Padding = new Thickness(5);
                lbl.Margin = new Thickness(2);
                lbl.BorderThickness = new Thickness(1);
                lbl.Tag = dt;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                lbl.Height = 30;
                lbl.Width = 30;
                lbl.MouseDown += LblMouseDown;

                wStackPanel.Children.Add(lbl);
            }
            this.monthStack.Children.Add(wStackPanel);
        }

        private void LblMouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lbl = sender as Label;

            if (lbl != null)
            {
                DateTime dt = lbl.Tag is DateTime ? (DateTime) lbl.Tag : new DateTime();
                this.SelectedDate = dt;
                this.CreateCalenderLayout();
            }
        }
    }
}