using System;
using System.Windows.Media;

namespace Montageplan.Model
{
    public class Projekt
    {
        public const int MAX_LENGTH_NAME = 50;


        public Projekt()
            : this("")
        {
        }
        public Projekt(string name)
        {
            this.Id = 0;
            this.Name = name;
            this.Startdatum = null;
            this.Endedatum = null;
            this.ProjectColorBrush = new SolidColorBrush(Color.FromArgb(60, Colors.Green.R, Colors.Green.G, Colors.Green.B));
            this.IsCompleted = false;
            this.IsLinkedToEngagement = false;
            this.IsTemplate = true;
        }

        /// <summary>
        /// Datenbank Id (PK)
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Startdatum { get; set; }
        public DateTime? Endedatum { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsTemplate { get; set; }
        public SolidColorBrush ProjectColorBrush { get; set; }
        /// <summary>
        /// Wurde dieses Projekt mind. einem Termin zugewiesen ?
        /// </summary>
        public bool IsLinkedToEngagement { get; set; }

        /// <summary>
        /// Gibt an, ob das Projekt bereits gestartet ist und ob es mit einem Termin verknüpft ist.
        /// </summary>
        /// <returns></returns>
        public ProjektStatus GetStatus()
        {
            // Idle = rote Farbe
            ProjektStatus status = ProjektStatus.Idle;

            // Projekte ohne Start- Ende werden immer schwarz angezeigt.
            if (!this.HasDate())
                status = ProjektStatus.NoTime;
            // Projekt fällig mit Termin verbunden
            else if (this.IsStarted() && this.IsLinkedToEngagement)
                status = ProjektStatus.Running;
            // Projekt fällig aber nicht mit Termin verbunden.
            else if (this.IsStarted() && !this.IsLinkedToEngagement)
                status = ProjektStatus.Warning;
            // Projekt noch nicht fällig aber schon mit Termin verbunden
            else if (!this.IsStarted() && this.IsLinkedToEngagement)
                status = ProjektStatus.Linked;

            return status;
        }

        public bool HasTime()
        {
            bool hasTime = (this.GetStartTime().HasValue && this.GetEndTime().HasValue);
            return hasTime;
        }

        public DateTime? GetStartTime()
        {
            DateTime? time = this.GetTime(this.Startdatum);
            return time;
        }

        public DateTime? GetEndTime()
        {
            DateTime? time = this.GetTime(this.Endedatum);
            return time;
        }

        private DateTime? GetTime(DateTime? dateTime)
        {
            DateTime? time = null;
            if (dateTime.HasValue && (dateTime.Value.Hour > 0 || dateTime.Value.Minute > 0))
                time = new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day,
                    dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second);
            return time;
        }

        public bool IsZeitAbgelaufen()
        {
            bool istAbgelaufen = (this.Endedatum.HasValue && this.Endedatum.Value.Date < DateTime.Today);
            return istAbgelaufen;
        }

        /// <summary>
        /// Gibt an, ob ein Projekt ein Startdatum hat und der Tag heute oder ein Tag vor heute ist.
        /// </summary>
        /// <returns></returns>
        public bool IsStarted()
        {
            bool isStarted = (this.Startdatum.HasValue && this.Startdatum.Value <= DateTime.Today);
            return isStarted;
        }

        public bool HasDate()
        {
            if (this.Startdatum.HasValue || this.Endedatum.HasValue)
                return true;

            return false;
        }

        public bool IsDifferent(Projekt projekt)
        {
            bool isDiff = false;
            if (
                this.IsCompleted != projekt.IsCompleted ||
                this.ProjectColorBrush.ToString() != projekt.ProjectColorBrush.ToString() ||
                this.Name != projekt.Name ||
                this.Startdatum.ToString() != projekt.Startdatum.ToString() ||
                this.Endedatum.ToString() != projekt.Endedatum.ToString())
                isDiff = true;

            return isDiff;
        }

        /// <summary>
        /// Gänztägig - Keine Zeit angegeben.
        /// </summary>
        /// <returns></returns>
        public bool IsAllDay()
        {
            bool isAllDay = false;
            if (this.Startdatum.HasValue && this.Endedatum.HasValue)
            {
                isAllDay = (
                this.Startdatum.Value.Hour == 0 && this.Startdatum.Value.Minute == 0 ||
                this.Endedatum.Value.Hour == 0 && this.Endedatum.Value.Minute == 0);
            }
            else
                isAllDay = true;
            return isAllDay;
        }

        public override string ToString()
        {
            string startdatum = "";
            string endeDatum = "";
            if (this.Startdatum.HasValue)
                startdatum = string.Format("{0:dd.MM.yyyy}", this.Startdatum.Value);
            if (this.Endedatum.HasValue)
                endeDatum = string.Format("{0:dd.MM.yyyy}", this.Endedatum.Value);

            return string.Format("{0} ({1}-{2})", this.Name, startdatum, endeDatum);
        }

    }
}
