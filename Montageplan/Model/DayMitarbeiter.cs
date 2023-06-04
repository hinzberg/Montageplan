using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Montageplan.Model
{
    public class DayMitarbeiter
    {
        /// <summary>
        /// Erstellt Mitarbeiter-Tages-Items. Am Anfang habe ich immer Mitarbeiter, bevor 
        /// man abhängig vom jeweiligen Tag den Status setzen kann. Hier werden diese Items erstellt, die Anzahl wird von
        /// den übergebenen Mitarbeiter bestimmt. Diese erstellten Items werden zurückgegeben.
        /// </summary>
        /// <param name="mitarbeiter">Mitarbeiter</param>
        /// <returns></returns>
        public static List<DayMitarbeiter> CreateLinks(IEnumerable<Mitarbeiter> mitarbeiter)
        {
            List<DayMitarbeiter> links = new List<DayMitarbeiter>();
            foreach (var item in mitarbeiter)
            {
                links.Add(new DayMitarbeiter(item));
            }
            return links;
        }

        public DayMitarbeiter()
            : this(null)
        {
        }
        public DayMitarbeiter(Mitarbeiter mitarbeiter)
            : this(mitarbeiter, false)
        {
        }

        public DayMitarbeiter(Mitarbeiter mitarbeiter, bool istFuehrer)
        {
            this.Mitarbeiter = mitarbeiter;
            this.IstFuehrer = istFuehrer;
            this.Status = MitarbeiterDayStatus.Available;
        }

        public Mitarbeiter Mitarbeiter { get; set; }
        public MitarbeiterDayStatus Status { get; set; }
        public bool IstFuehrer { get; set; }

        public override bool Equals(object obj)
        {
            bool isEqual;
            DayMitarbeiter comparingObj = (DayMitarbeiter)obj;
            isEqual = (this.Mitarbeiter.GetFullName() == comparingObj.Mitarbeiter.GetFullName()); // TODO (Vllt. eine ID und noch den Tag als verhleich?)

            return isEqual;
        }

    }
}
