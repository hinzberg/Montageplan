using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using Montageplan.Model;
using Montageplan.View;
using System.Windows;
using Montageplan.ViewModel;

namespace Montageplan.Presenter
{
    public class CalendarDayPresenter : IDisposable
    {
        private readonly CalendarDay day;
        private readonly CalendarMitarbeiterPresenter mitarbeiterPresenter;
        private readonly CalendarDayView view;
        private readonly EngagementRepository engagements;
        private readonly MontageCalendar calendar;
        private List<CalendarEngagementPresenter> engagementPresenters;

        public CalendarDayPresenter(
            CalendarDayView view, CalendarDay day, EngagementRepository engagements, MontageCalendar calendar, CalendarMitarbeiterPresenter mitarbeiterPresenter)
        {
            this.view = view;
            this.day = day;
            this.engagements = engagements;
            this.calendar = calendar;
            this.mitarbeiterPresenter = mitarbeiterPresenter;
            this.engagementPresenters = new List<CalendarEngagementPresenter>();
            this.IsFirstDisplayedDay = false;
            this.KolonneExpandedOrCollapsed = null;

            NotificationCenter.GetInstance().Add(new NotificationCenter.IdentifierNotification((int)Notifications.RefreshCompleteCalendar, 
                this.UpdateCompleteCalendar));
        }
        ~CalendarDayPresenter()
        {
            NotificationCenter.GetInstance().Remove((int)Notifications.RefreshCompleteCalendar);
        }

        public Action KolonneExpandedOrCollapsed { get; set; }

        /// <summary>
        /// Ist der Tag der erste, der im Kalender ganz links angezeigt wird ?
        /// </summary>
        public bool IsFirstDisplayedDay { get; set; }

        // IDisposable
        public void Dispose()
        {
            NotificationCenter.GetInstance().Remove((int)Notifications.RefreshCompleteCalendar);
        }

        public void UpdateView()
        {
            // Farbe aus den Optionen
            this.view.dayTextBlock.Foreground = AppConfig.Settings.DayBrush;

            // Wenn der Tag der heutige Tag dann kräftige Farbe.
            if (this.day.IsToday)
            {
                Color col = AppConfig.Settings.DayBrush.Color;
                col.A = 255;

                SolidColorBrush strongBrush = new SolidColorBrush(col);
                this.view.dayTextBlock.Foreground = strongBrush;
            }

            this.engagementPresenters = new List<CalendarEngagementPresenter>();

            this.view.dayTextBlock.Text = this.BuildDayHeader(this.day.Date);
            this.mitarbeiterPresenter.SetMitarbeiter(this.day.MitarbeiterChoice.GetAllNameSorted());

            List<CalendarEngagementView> kolonnenViews = new List<CalendarEngagementView>();
            foreach (var item in this.day.Engagements.GetAll())
            {
                CalendarEngagementView kolView = new CalendarEngagementView();
                kolView.AllowDrop = true;
                kolonnenViews.Add(kolView);
                CalendarEngagementPresenter presenter = new CalendarEngagementPresenter(kolView, item, this.day, this.engagements, this.calendar);
                presenter.IsFirstDisplayedDay = this.IsFirstDisplayedDay;
                presenter.KolonneExpandedOrCollapsed = this.KolonneExpandedOrCollapsed;
                presenter.MitarbeiterInChooseListStatusUpdateAndRefreshing = new Action<int>(this.UpdateMitarbeiterStatusAndRefreshOnChooseList);
                this.engagementPresenters.Add(presenter);

                presenter.Init();
                presenter.UpdateView();
            }
            this.view.kolonnenItemsControl.ItemsSource = kolonnenViews;
        }

        public List<CalendarEngagementPresenter> GetKolonnePresenters()
        {
            return new List<CalendarEngagementPresenter>(this.engagementPresenters);
        }

        private void UpdateCompleteCalendar()
        {
            foreach (var item in this.engagementPresenters)
            {
                item.UpdateView();
            }
            this.mitarbeiterPresenter.UpdateView();
        }

        private string BuildDayHeader(DateTime dayDate)
        {
            string header = "";
            if (DateTimeFormatInfo.CurrentInfo != null)
                header = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(dayDate.DayOfWeek) + " ";
            header += dayDate.ToShortDateString();
            return header;
        }

        private void UpdateMitarbeiterStatusAndRefreshOnChooseList(int mitarbeiterId)
        {
            DayMitarbeiterViewModel chooseListViewModel = this.mitarbeiterPresenter.GetDayMitarbeiter(mitarbeiterId);
            if (chooseListViewModel != null)
            {
                this.day.UpdateMitarbeiterStatus(chooseListViewModel.GetModel());
                chooseListViewModel.NotifyStatusColor();
            }
        }
    }
}