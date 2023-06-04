using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class Engagement
    {
        /// <summary>
        /// Erstellt Tages-Kolonnen-Projekte-Items. Am Anfang habe ich immer eine Kolonne, bevor 
        /// Mitarbeiter und Projekte der Kolonne zugewiesen werden. Hier werden diese Items erstellt, die Anzahl wird von
        /// den übergebenen Kolonnen bestimmt. Diese erstellten Items werden zurückgegeben.
        /// </summary>
        /// <param name="kolonnen">Kolonnen</param>
        /// <returns></returns>
        public static List<Engagement> CreateList(IEnumerable<Kolonne> kolonnen, DateTime day)
        {
            List<Engagement> containers = new List<Engagement>();
            foreach (var kol in kolonnen)
            {
                Engagement engagement = new Engagement(kol, day);
                engagement.SetKolonnenFuererInMitarbeiter();
                engagement.SetAdditionalMitarbeiter();
                containers.Add(engagement);
            }
            return containers;
        }

        public Engagement()
            : this(null, DateTime.MinValue)
        {
        }
        public Engagement(Kolonne kolonne, DateTime day)
        {
            this.Id = 0;
            this.Date = day;
            this.Kolonne = kolonne;
            this.Projekte = new EngagementProjektRepository();
            this.Mitarbeiter = new DayMitarbeiterRepository();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Kolonne Kolonne { get; set; }
        public bool HasProjekte
        {
            get { return this.Projekte.GetCount() > 0; }
        }
        public EngagementProjektRepository Projekte { get; set; }
        public DayMitarbeiterRepository Mitarbeiter { get; set; }
        public bool IsValid
        {
            get { return (this.HasProjekte && this.Kolonne != null && this.Mitarbeiter.GetCount() > 0); }
        }

        /// <summary>
        /// Überprüft, ob der übergebene Mitarbeiter bereits als Mitarbeiter dieser Kolonne zugeordnet wurde und gibt das Ergebnis zurück.
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiter</param>
        /// <returns></returns>
        public bool ContainsMitarbeiter(DayMitarbeiter mitarbeiter)
        {
            bool contains = this.Mitarbeiter.Contains(mitarbeiter);
            return contains;
        }

        /// <summary>
        /// Wenn bei einer Kolonne der Führer oder Mitarbeiter verändert wurden, soll dies in dem Termin aktualisiert werden.
        /// </summary>
        public void RefreshMitarbeiter()
        {
            this.Mitarbeiter.ClearAll();
            this.SetKolonnenFuererInMitarbeiter();
            this.SetAdditionalMitarbeiter();
        }

        public bool IsAvailableProjektTime(DateTime? start, DateTime? end)
        {
            bool isSpace = false;
            if (!this.HasProjekte)
                isSpace = true;
            else if (!start.HasValue || !end.HasValue) // Gänztägig, es sind aber schon projekte vorhanden
                isSpace = false;
            else
            {
                bool tempIsSpace = true;
                foreach (var proItem in this.Projekte.GetAllTimeSorted())
                {
                    tempIsSpace = !proItem.IntersectTime(start.Value, end.Value);
                    if (!tempIsSpace)
                        break;
                }
                isSpace = tempIsSpace;
            }
            return isSpace;
        }

        private void SetKolonnenFuererInMitarbeiter()
        {
            if (this.Kolonne != null && this.Kolonne.Fuehrer != null)
            {
                //this.Mitarbeiter.RemoveFuehrer();
                this.Mitarbeiter.Add(new DayMitarbeiter(this.Kolonne.Fuehrer, true));
            }
        }
        private void SetAdditionalMitarbeiter()
        {
            if (this.Kolonne != null && this.Kolonne.AdditionalMitarbeiter != null)
            {
                foreach (var adMitItem in this.Kolonne.AdditionalMitarbeiter.GetAll())
                {
                    //this.Mitarbeiter.Remove(adMitItem.Id);
                    this.Mitarbeiter.Add(new DayMitarbeiter(adMitItem, false));
                }
            }
        }


    }
}
