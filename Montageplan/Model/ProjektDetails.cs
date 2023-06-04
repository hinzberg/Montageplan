using System.Collections.Generic;

namespace Montageplan.Model
{
    /// <summary>
    /// Wrapper, wird zum Drucken benötigt
    /// </summary>
    public class ProjektDetails
    {
        public ProjektDetails()
        {
            SingleProjekt = null;
            Mitarbeiter = null;
            SingleKolonne = null;
        }

        public Projekt SingleProjekt { get; set; }
        public List<DayMitarbeiter> Mitarbeiter { get; set; }
        public Kolonne SingleKolonne { get; set; }
    }
}