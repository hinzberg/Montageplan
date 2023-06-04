using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Montageplan.Model;
using Montageplan.View;
using Montageplan.ViewModel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;

namespace Montageplan.Presenter
{
    public class CalendarPresenter
    {
        private readonly CalendarView view;
        private readonly MontageCalendar calendar;
        private readonly EngagementRepository engagements;
        private readonly DateNavigationBarPresenter navigationBarPresenter;
        private List<CalendarDayPresenter> displayDayPresenters;

        public CalendarPresenter(CalendarView view, MontageCalendar calendar, EngagementRepository engagements)
        {
            this.view = view;
            this.calendar = calendar;
            this.engagements = engagements;
            this.navigationBarPresenter = new DateNavigationBarPresenter(view.dateNavigationBar, calendar, this);

            this.navigationBarPresenter.UpdateView();
            this.displayDayPresenters = new List<CalendarDayPresenter>();

            this.view.zoomControl.ObserveMouseWheel(this.view);
            this.view.zoomControl.SliderValueChanged = new Action<double>(this.NotifyZoomSliderValueChanged);
            this.navigationBarPresenter.DateRangeChanged = new Action(this.NotifyDateRangeChanged);

            CalendarDaysHorizontalStackPanel.PanelCreated = new Action(this.NotifyDayItemsPanelCreated);
        }

        /// <summary>
        /// Erzeugt 7 Tage-Items im sichtbaren Bereich. Von Montag bis Sonntag.
        /// </summary>
        public void UpdateView()
        {
            if (this.displayDayPresenters != null && this.displayDayPresenters.Count > 0)
            {
                foreach (var item in this.displayDayPresenters)
                    item.Dispose();
            }
            this.displayDayPresenters = new List<CalendarDayPresenter>();

            List<CalendarDayView> dayViews = new List<CalendarDayView>();
            List<CalendarMitarbeiterView> mitarbeiterViews = new List<CalendarMitarbeiterView>();
            List<DateTime> dates = this.calendar.GetDatesAtWeek();

            foreach (var date in dates)
            {
                this.calendar.CreateOrUpdateDay(date);
                CalendarDay day = this.calendar.Days.Get(date);

                CalendarDayView view = new CalendarDayView();
                dayViews.Add(view);

                CalendarMitarbeiterView mitarbView = this.CreateMitarbeiterView();
                mitarbeiterViews.Add(mitarbView);
                CalendarMitarbeiterPresenter mitarbPresenter = new CalendarMitarbeiterPresenter(mitarbView, day.Date);

                CalendarDayPresenter presenter = new CalendarDayPresenter(view, day, this.engagements, this.calendar, mitarbPresenter);
                presenter.IsFirstDisplayedDay = (date == dates.First());
                presenter.KolonneExpandedOrCollapsed = new Action(this.NotifyDayKolonneExpandedOrCollapsed);
                presenter.UpdateView();
                this.displayDayPresenters.Add(presenter);
            }

            this.view.daysListBox.ItemsSource = dayViews;
            this.view.mitarbeiterViewsItemsControl.ItemsSource = mitarbeiterViews;
        }

        private CalendarMitarbeiterView CreateMitarbeiterView()
        {
            CalendarMitarbeiterView mitarbView = new CalendarMitarbeiterView();
    
            ListBoxDragHelper dragHelper = new ListBoxDragHelper(mitarbView, this.GetMitarbeiterAdornerTemplate());
            ListBoxDragHelper.AddHelper(dragHelper);

            return mitarbView;
        }

        public void SetMitarbeiterListsVisibility(bool isVisible)
        {
            // Neu, macht das Panel sichtbar/unsichtbar.
            if (isVisible)
            {
                this.view.mitarbeiterPanel.Visibility = Visibility.Visible;
                this.view.mitarbeiterListRow.Height = new GridLength(163);
            }
            else
            {
                this.view.mitarbeiterPanel.Visibility = Visibility.Collapsed; 
                this.view.mitarbeiterListRow.Height = new GridLength(0);
            }
        }

        public void UpdateDayItemsAlignment()
        {
            this.SetDayItemsAlignment(true);
        }
        private void SetDayItemsAlignment(bool updateItems)
        {
            CalendarDaysHorizontalStackPanel.SetAllItemsAlignment(AppConfig.Settings.KalenderTageAusrichtung);
            if (updateItems)
                CalendarDaysHorizontalStackPanel.UpdateAllItemsWidth();
        }

        /// <summary>
        /// Öffnet oder Schließt sämtliche Kolonnen im View.
        /// </summary>
        /// <param name="isOpen"></param>
        public void OpenAllKolonneExpander(bool isOpen)
        {
            foreach (var dayPresenter in this.displayDayPresenters)
            {
                foreach (var kolPresenter in dayPresenter.GetKolonnePresenters())
                {
                    if (isOpen)
                    {
                        Engagement kol = kolPresenter.GetDayKolonne();
                        kol.Kolonne.IsExpanded = true;
                    }
                    else
                    {
                        Engagement kol = kolPresenter.GetDayKolonne();
                        kol.Kolonne.IsExpanded = false;
                    }
                    kolPresenter.UpdateKolonneExpandedState();
                }
            }
        }

        private DataTemplate GetMitarbeiterAdornerTemplate()
        {
            DataTemplate template = null;
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri(@"..\View\ItemTemplates\DragAdornerTemplates.xaml", UriKind.Relative);
            template = dictionary["mitarbeiterAdornerTemplate"] as DataTemplate;
            return template;
        }

        private void NotifyDayKolonneExpandedOrCollapsed()
        {
            foreach (var dayPresenter in this.displayDayPresenters)
            {
                foreach (var kolPresenter in dayPresenter.GetKolonnePresenters())
                {
                    kolPresenter.UpdateKolonneExpandedState();
                }
            }
        }

        private void NotifyDateRangeChanged()
        {
            this.UpdateView();
            this.UpdateDayItemsAlignment();
        }

        private void NotifyZoomSliderValueChanged(double value)
        {
            double scaleValue = this.GetScaleValueByZoomFactor(value);
            ScaleTransform scale = new ScaleTransform(scaleValue, scaleValue);
            this.view.daysListBox.LayoutTransform = scale;
        }

        private double GetScaleValueByZoomFactor(double factor)
        {
            double value = 1.0;
            switch ((int)factor)
            {
                case 1: value = 0.7; break;
                case 2: value = 0.8; break;
                case 3: value = 0.9; break;
                case 4: value = 1.0; break;
                case 5: value = 1.1; break;
                case 6: value = 1.2; break;
                default:
                    break;
            }
            return value;
        }

        // EventHandler

        private void NotifyDayItemsPanelCreated()
        {
            this.SetDayItemsAlignment(false);
        }

    }
}