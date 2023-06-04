using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class CalendarDay
    {
        public CalendarDay()
            : this(DateTime.MinValue)
        {
        }
        public CalendarDay(DateTime date)
            : this(date, new List<Engagement>(), new List<DayMitarbeiter>())
        {
        }
        public CalendarDay(DateTime date, IEnumerable<Engagement> dayKolonnen, IEnumerable<DayMitarbeiter> dayMitarbeiter)
        {
            this.Date = date;
            this.Engagements = new EngagementRepository();
            this.Engagements.AddRange(dayKolonnen);
            this.MitarbeiterChoice = new DayMitarbeiterRepository();
            this.MitarbeiterChoice.AddRange(dayMitarbeiter);
        }

        public DateTime Date { get; set; }
        public EngagementRepository Engagements { get; set; }
        public DayMitarbeiterRepository MitarbeiterChoice { get; set; }
        public bool IsToday
        {
            get { return this.Date.Date == DateTime.Today; }
        }

        /// <summary>
        /// Hier werden die Stati der Mitarbeiter des Tages aktualisiert. Berücksichtigt wird, ob der Mitarbeiter bereits einer Kolonne
        /// zugewiesen wurde oder eventuell Krank ist bzw. Urlaub hat.
        /// </summary>
        public void UpdateMitarbeiterStatus()
        {
            foreach (var mitar in this.MitarbeiterChoice.GetAll())
            {
                this.UpdateMitarbeiterStatus(mitar);
            }
        }
        /// <summary>
        /// Hier wird der Status des übergebenen Mitarbeiters des Tages aktualisiert. Berücksichtigt wird, ob der Mitarbeiter bereits einer Kolonne
        /// zugewiesen wurde oder eventuell Krank ist bzw. Urlaub hat.
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiters</param>
        public void UpdateMitarbeiterStatus(DayMitarbeiter mitarbeiter)
        {
            bool isLinked = false;
            // Alle Kolonnen an diesem Tag überprüfen, ob der Mitarbeiter einer Kolonne bereits zugewiesen ist
            foreach (var dayKolonne in this.Engagements.GetAll())
            {
                if (dayKolonne.Mitarbeiter.Contains(mitarbeiter))
                {
                    isLinked = true;
                    break;
                }
            }

            mitarbeiter.Status = (isLinked) ? MitarbeiterDayStatus.Busy : MitarbeiterDayStatus.Available;
        }

        public List<ProjektDetails> GetExistingProjektDetails()
        {
            List<ProjektDetails> projekte = new List<ProjektDetails>();

            foreach (Engagement engagement in this.Engagements.GetAll())
            {
                if (engagement.HasProjekte)
                {
                    foreach (var engProjekt in engagement.Projekte.GetAll())
                    {
                        // TODO|Projekte-Testen
                        ProjektDetails details = new ProjektDetails();
                        details.SingleProjekt = engProjekt.Projekt;
                        details.Mitarbeiter = engagement.Mitarbeiter.GetAll();
                        details.SingleKolonne = engagement.Kolonne;
                        projekte.Add(details);
                    }
                }
            }
            return projekte;
        }
    }
}
