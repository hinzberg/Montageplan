using System;
using System.Windows.Input;
using Montageplan.Model;
using Montageplan.View;
using Montageplan.View.Windows;
using System.Globalization;

namespace Montageplan.Presenter
{
    public class DateNavigationBarPresenter
    {
        private readonly DateNavigationBar dateNavigationBar;
        private readonly MontageCalendar calendar;
        private readonly CalendarPresenter calenderPresenter;

        public DateNavigationBarPresenter(DateNavigationBar navigationBar, MontageCalendar calendar, CalendarPresenter calenderPres)
        {
            this.dateNavigationBar = navigationBar;
            this.calenderPresenter = calenderPres;
            this.calendar = calendar;
            this.dateNavigationBar.btnToday.Click += BtnTodayClick;
            this.dateNavigationBar.btnPreviousWeek.Click += BtnPreviousWeekClick;
            this.dateNavigationBar.btnNextWeek.Click += BtnNextWeekClick;
            this.dateNavigationBar.btnDatePicker.Click += BtnDatePickerClick;
            this.dateNavigationBar.dateRangeTextBlock.MouseDown += DateRangeTextBlockMouseDown;
            this.dateNavigationBar.mainExpanderButton.Expanded += MainExpanderButtonExpanded;
            this.dateNavigationBar.mainExpanderButton.Collapsed += MainExpanderButtonExpanded;
            // Eingaben in der Textbox für die Kalenderwochen.
            this.dateNavigationBar.txtCalenderWeek.KeyDown += TxtCalenderWeekKeyDown;
            this.DateRangeChanged = null;
        }


        public Action DateRangeChanged { get; set; }

        public void UpdateView()
        {
            this.UpdateDateRange();
            this.UpdateCalenderWeek();
        }

        private void UpdateDateRange()
        {
            // MMMM = Monatsname
            string text = string.Format("{0:dd}. {1} {0:yyyy} - {2:dd}. {3} {2:yyyy}",
                this.calendar.DateFrom, this.calendar.DateFrom.ToString("MMMM", CultureInfo.CurrentCulture),
                this.calendar.DateTo, this.calendar.DateTo.ToString("MMMM", CultureInfo.CurrentCulture));

            this.dateNavigationBar.dateRangeTextBlock.Text = text;
            this.dateNavigationBar.dateRangeTextBlock.ToolTip = text;

        }

        private void UpdateCalenderWeek()
        {
            this.dateNavigationBar.txtCalenderWeek.Text = this.calendar.GetGermanWeek().Week.ToString();
        }

        private void CallDateRangeChanged()
        {
            if (this.DateRangeChanged != null)
                this.DateRangeChanged();
        }

        // EventHandler

        /// <summary>
        ///  TextBlock Klick : Datum aus dem DatePicker zuweisen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DateRangeTextBlockMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ShowDatePicker();
        }

        /// <summary>
        /// Button Klick: Datum aus dem DatePicker zuweisen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnDatePickerClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ShowDatePicker();
        }

        /// <summary>
        /// DatePicker anzeigen
        /// </summary>
        private void ShowDatePicker()
        {
            DatePickerWindow datePickerWindow = new DatePickerWindow();
            //datePickerWindow.Owner = this.mainWindow;
            datePickerWindow.SelectedDate = this.calendar.DateFrom;

            bool? showDialog = datePickerWindow.ShowDialog();
            if (showDialog != null && showDialog.Value)
            {
                this.calendar.SetDateRange(datePickerWindow.SelectedDate);
                this.UpdateView();
                this.CallDateRangeChanged();
            }
        }

        void MainExpanderButtonExpanded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.dateNavigationBar.mainExpanderButton.IsExpanded)
            {
                this.calenderPresenter.OpenAllKolonneExpander(true);
            }
            else
            {
                this.calenderPresenter.OpenAllKolonneExpander(false);
            }
        }

        /// <summary>
        /// Sieben Tage weiter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnNextWeekClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.calendar.SetNextWeek();
            this.UpdateView();
            this.CallDateRangeChanged();
        }

        /// <summary>
        /// Sieben Tage zurück
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnPreviousWeekClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.calendar.SetPreviousWeek();
            this.UpdateView();
            this.CallDateRangeChanged();
        }

        /// <summary>
        /// Zurück zur Woche mit dem heutigen Datum.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnTodayClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.calendar.SetCurrentWeek();
            this.UpdateView();
            this.CallDateRangeChanged();
        }

        /// <summary>
        /// Eingaben in der Textbox für die Kalenderwochen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TxtCalenderWeekKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string entry = this.dateNavigationBar.txtCalenderWeek.Text;
                int week = 1;
                if (int.TryParse(entry, out week))
                {
                    if (week >= 1 && week <= 52)
                    {
                        this.calendar.SetDateRange(this.calendar.DateFrom.Year, week);
                        this.UpdateView();
                        this.CallDateRangeChanged();
                    }
                }
            }
        }

    }
}
