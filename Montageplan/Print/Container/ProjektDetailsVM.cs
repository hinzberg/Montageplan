using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Montageplan.Model;

namespace Montageplan.Print.Container
{
    public class ProjektDetailsVM
    {
        private ProjektDetails details;

        public ProjektDetailsVM(ProjektDetails det)
        {
            this.details = det;
        }

        public SolidColorBrush ProjektColor { get { return this.details.SingleProjekt.ProjectColorBrush; } }
        public string Projekt { get { return string.Format("{0}", this.details.SingleProjekt.Name); } }
        public string Kolonne { get { return  this.details.SingleKolonne.Bezeichnung; } }
        public List<DayMitarbeiter> ProjektMitarbeiter { get { return this.details.Mitarbeiter; } }

        public string KolonneNummer
        {
            get
            {
                // Ob vor den Kolonnenbezeichnung jetzt "Kolonne" 
                // steht kann in den Optionen eingestellt werden.
                string prefix = "";
                if (AppConfig.Settings.ShowKolonnePrefix)
                    prefix = "Kolonne: ";

                return string.Format("{0}{1}",prefix, this.details.SingleKolonne.Nummer); 
            }
        }
        
    }
}
