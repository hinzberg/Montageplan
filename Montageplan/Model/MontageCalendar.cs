using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class MontageCalendar
    {
        private readonly RepositoryContainer repositories;
        private DateTime dateFrom;
        private DateTime dateTo;

        public MontageCalendar(RepositoryContainer repositories)
        {
            this.repositories = repositories;
            this.SetCurrentWeek();
        }

        /// <summary>
        /// Datum des Wochenanfangs (MO) im sichtbaren Bereich des Kalenders.
        /// </summary>
        public DateTime DateFrom
        {
            get { return this.dateFrom; }
        }
        /// <summary>
        /// Datum des Wochenendes (SO) im sichtbaren Bereich des Kalenders.
        /// </summary>
        public DateTime DateTo
        {
            get { return this.dateTo; }
        }

        public CalendarDayRepository Days
        {
            get { return this.repositories.Days; }
        }

        /// <summary>
        /// Hier werden Kalendertage erstellt, wenn diese Tage auf dem Kalender sichtbar werden. Falls ein tag schon vorhanden ist,
        /// werden die Daten auf den aktuellsten Stand gebracht (Kolonnen, Mitarbeiter).
        /// Da die Tage eher temporär sind, muss ein Tag mindestens ein Projekt beinhalten um als gültiger "Aktionstag"
        /// zu gelten und um behalten zu werden. Ansonsten werden die "leeren" Tage beim Programmende nicht gepeichert
        /// </summary>
        public void CreateOrUpdateDays(DateTime dateFrom, DateTime dateTo)
        {
            int days = (int)dateTo.Subtract(dateFrom).TotalDays;
            for (int n = 0; n < days; n++)
            {
                DateTime date = dateFrom.AddDays(n);
                this.CreateOrUpdateDay(date);
            }
        }
        public CalendarDay CreateOrUpdateDay(DateTime date)
        {
            CalendarDay calendarDay;
            // Die aktuellen Kolonnen und Mitarbeiter müssen immer im Kalender angezeigt werden
            List<Kolonne> currentKolonnen = this.repositories.Kolonnen.GetAll();
            List<Mitarbeiter> mitarbeiter = this.repositories.Mitarbeiter.GetAll();

            // Gibt es zu diesem Tag schon Speicherungen ?
            // Wenn Nein, neue Tageseinträge erstellen. Wenn Ja, dann diese mit den aktuellen Kolonnen synchronisieren
            if (this.repositories.Days.Contains(date))
            {
                calendarDay = this.repositories.Days.Get(date);
                // Kolonnen und Mitarbeiter des Tages aktualisieren
                calendarDay.Engagements.UniformToDayKolonnen(currentKolonnen, date);
                calendarDay.MitarbeiterChoice.UniformToDayMitarbeiter(mitarbeiter);
                calendarDay.Engagements.RefreshKolonnenMitarbeiterOnUnvalidEngagements();
                calendarDay.UpdateMitarbeiterStatus();
            }
            else
            {
                calendarDay = new CalendarDay(date, Engagement.CreateList(currentKolonnen, date), DayMitarbeiter.CreateLinks(mitarbeiter));
                calendarDay.UpdateMitarbeiterStatus();
                this.repositories.Days.Add(calendarDay);
            }
            return calendarDay;
        }

        public void ReplaceKolonneAtDate(Engagement engagment)
        {
            // TODO Mike
            // Wenn ich Kolonnen lösche werden die Engagments nicht richtig zurückgesetzt.
            // Beim Start will er dann hier mit einer Kolonne zusammensetzten die es nicht mehr gibt.
            if (engagment.Kolonne == null)
                return;


            CalendarDay cDate = this.repositories.Days.Get(engagment.Date);
            if (cDate != null)
            {
                Engagement engToReplace = null;
                foreach (var engItem in cDate.Engagements.GetAll())
                {
                    if (engItem.Kolonne.Id == engagment.Kolonne.Id)
                    {
                        engToReplace = engItem;
                        break;
                    }
                }
                if (engToReplace != null)
                {
                    int index = cDate.Engagements.GetAll().IndexOf(engToReplace);
                    cDate.Engagements.Remove(engToReplace);
                    cDate.Engagements.Insert(index, engagment);
                }
            }
        }

        public void SetCurrentWeek()
        {
            this.SetDateRange(DateTime.Today);
        }

        public void SetNextWeek()
        {
            DateTime nextDateFrom = this.dateFrom.AddDays(7);
            this.SetDateRange(nextDateFrom);
        }

        public void SetPreviousWeek()
        {
            DateTime previuousDateFrom = this.dateFrom.Subtract(TimeSpan.FromDays(7));
            this.SetDateRange(previuousDateFrom);
        }

        public void SetDateRange(DateTime dateAtWeek)
        {
            DateHelper helper = new DateHelper();
            this.dateFrom = helper.GetMondayBeforeDate(dateAtWeek);
            this.dateTo = this.DateFrom.AddDays(5);
        }
        public void SetDateRange(int year, int calendarWeek)
        {
            DateHelper helper = new DateHelper();
            this.dateFrom = helper.FirstDateOfWeek(year, calendarWeek);
            this.dateTo = this.DateFrom.AddDays(5);
        }

        public CalendarWeek GetGermanWeek()
        {
            CalendarWeek week;
            DateHelper helper = new DateHelper();
            week = helper.GetGermanCalendarWeek(this.DateFrom);
            return week;
        }

        public List<DateTime> GetDatesAtWeek()
        {
            List<DateTime> dates = new List<DateTime>();
            int days = (int)this.DateTo.Subtract(this.DateFrom).TotalDays;
            for (int n = 0; n <= days; n++)
            {
                DateTime date = this.DateFrom.AddDays(n);
                dates.Add(date);
            }
            return dates;
        }

    }
}
